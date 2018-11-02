using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using PureActive.Network.Abstractions.PureObject;

namespace PureActive.Network.Devices.PureObjectGraph
{
    public class PureObjectGraph<T> where T : IPureObject
    {
        private readonly object _oLock = new object();
        private readonly object _ivLock = new object();
        private readonly object _ieLock = new object();
        private volatile int _directedEdgeCount = 0;

        private readonly ConcurrentDictionary<Guid, PureObjectVertex<T>> _vertices = new ConcurrentDictionary<Guid, PureObjectVertex<T>>();

        /// <summary>
        /// Gets the vertices of the graph.
        /// </summary>
        public ReadOnlyDictionary<Guid, PureObjectVertex<T>> Vertices { get; }

        private PureObjectVertex<T>[] _indexedVertices = null;

        /// <summary>
        /// Gets the order of the graph.
        /// </summary>
        public int Order
        {
            get => _vertices.Count;
        }

        private ConcurrentDictionary<(Guid tail, int directed, Guid head), PureObjectEdge<T>> _edges = new ConcurrentDictionary<(Guid tail, int directed, Guid head), PureObjectEdge<T>>();
        private readonly ReadOnlyDictionary<(Guid tail, int directed, Guid head), PureObjectEdge<T>> _readOnlyEdges;
        /// <summary>
        /// Gets the edges of the graph.
        /// </summary>
        public ReadOnlyDictionary<(Guid tail, int directed, Guid head), PureObjectEdge<T>> Edges
        {
            get => _readOnlyEdges;
        }

        private PureObjectEdge<T>[] _indexedEdges = null;

        /// <summary>
        /// Gets the size of the graph.
        /// </summary>
        public int Size
        {
            get => _edges.Count;
        }

        /// <summary>
        /// Gets the count of directed edges.
        /// </summary>
        public int DirectedEdgeCount
        {
            get => _directedEdgeCount;
        }

        /// <summary>
        /// Constructor to create an instance of the graph.
        /// </summary>
        public PureObjectGraph()
        {
            Vertices = new ReadOnlyDictionary<Guid, PureObjectVertex<T>>(_vertices);
            _readOnlyEdges = new ReadOnlyDictionary<(Guid tail, int directed, Guid head), PureObjectEdge<T>>(_edges);
        }

        /// <summary>
        /// Creates a vertex given its value and adds it to the graph.
        /// </summary>
        /// <param name="value">The value of the vertex to create and add.</param>
        /// <returns>True if successful, otherwise, false.</returns>
        public bool AddVertex(T value) => AddVertex(new PureObjectVertex<T>(value));

        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        /// <returns>True if successful, otherwise, false.</returns>
        public bool AddVertex(PureObjectVertex<T> vertex)
        {
            lock (_oLock)
            {
                if (_vertices.TryAdd(vertex.Id, vertex))
                {
                    lock (_ivLock)
                    {
                        _indexedVertices = null;
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="id">The ID of the vertex to remove.</param>
        /// <returns>The vertex removed.</returns>
        /// <remarks>Incident edges are also removed from the graph.</remarks>
        public PureObjectVertex<T> RemoveVertex(Guid id)
        {
            lock (_oLock)
            {
                if (_vertices.TryRemove(id, out PureObjectVertex<T> removedVertex))
                {
                    lock (_ivLock)
                    {
                        _indexedVertices = null;
                    }
                    bool removed = false;
                    if (_edges.Count > 0)
                    {
                        object obj = new object();
                        _edges
                            .AsParallel()
                            .Where((edge) => edge.Value.Tail.Id == id || edge.Value.Head.Id == id)
                            .ForAll((edge) =>
                            {
                                lock (obj)
                                {
                                    var e = RemoveEdge(edge.Key);
                                    if (!removed && e != null)
                                    {
                                        removed = true;
                                    }
                                }
                            });
                    }
                    if (removed)
                    {
                        lock (_ieLock)
                        {
                            _indexedEdges = null;
                        }
                    }
                }
                return removedVertex;
            }
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        /// <returns>The vertex removed.</returns>
        public PureObjectVertex<T> RemoveVertex(PureObjectVertex<T> vertex) => RemoveVertex(vertex.Id);

        /// <summary>
        /// Returns a Vertex given a vertex ID.
        /// </summary>
        /// <param name="id">The vertex ID.</param>
        /// <returns>The Vertex with the given vertex ID.</returns>
        public PureObjectVertex<T> GetVertex(Guid id) => _vertices[id];

        /// <summary>
        /// Creates and adds an edge to the graph.
        /// </summary>
        /// <param name="tailID">The vertex ID of the tail.</param>
        /// <param name="headID">The vertex ID of the head.</param>
        /// <param name="directed">Indicates if the edge is directed or not.</param>
        /// <param name="weight">The weight of the edge. Default is 0 (unweighted).</param>
        /// <returns>True if the edge is added successfully. Otherwise, false.</returns>
        public bool AddEdge(Guid tailID, Guid headID, bool directed = false, double weight = 0) => AddEdge(PureObjectEdge<T>.Create(_vertices[tailID], _vertices[headID], directed, weight));

        public bool AddEdge(T tail, T head, bool directed = false, double weight = 0)
        {
            if (tail == null) throw new ArgumentNullException(nameof(tail));
            if (head == null) throw new ArgumentNullException(nameof(head));

            return AddEdge(tail.ObjectId, head.ObjectId, directed, weight);
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        /// <returns>True if the edge is added successfully. Otherwise, false.</returns>
        private bool AddEdge(PureObjectEdge<T> edge)
        {
            lock (_oLock)
            {
                if (_edges.TryAdd(edge.Id, edge))
                {
                    lock (_ieLock)
                    {
                        _indexedEdges = null;
                    }
                    if (edge.Directed)
                    {
                        Interlocked.Increment(ref _directedEdgeCount);
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Removes an edge from the graph.
        /// </summary>
        /// <param name="id">The ID of the edge to remove.</param>
        /// <returns>The edge removed.</returns>
        public PureObjectEdge<T> RemoveEdge((Guid tail, int directed, Guid head) id)
        {
            lock (_oLock)
            {
                if (_edges.TryRemove(id, out PureObjectEdge<T> removedEdge))
                {
                    lock (_ieLock)
                    {
                        _indexedEdges = null;
                    }
                    if (removedEdge.Directed)
                    {
                        Interlocked.Decrement(ref _directedEdgeCount);
                    }
                }
                return removedEdge;
            }
        }

        /// <summary>
        /// Removes an edge from the graph.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        /// <returns>The edge removed.</returns>
        public PureObjectEdge<T> RemoveEdge(PureObjectEdge<T> edge) => RemoveEdge(edge.Id);

        /// <summary>
        /// Returns an Edge given an edge id.
        /// </summary>
        /// <param name="id">The edge ID.</param>
        /// <returns>The Edge with the given edge id.</returns>
        public PureObjectEdge<T> GetEdge((Guid tail, int directed, Guid head) id) => _edges[id];

        /// <summary>
        /// Creates an index of vertices.
        /// </summary>
        /// <returns>The indexed vertices.</returns>
        /// <remarks>
        /// It is recommended that IndexVertices() is called after all Add*()/Remove*() calls are done.
        /// </remarks>
        public PureObjectVertex<T>[] IndexVertices()
        {
            lock (_ivLock)
            {
                if (_indexedVertices == null)
                {
                    _indexedVertices = _vertices.Values.ToArray<PureObjectVertex<T>>();
                   Array.Sort(_indexedVertices);
                }
                return _indexedVertices;
            }
        }

        /// <summary>
        /// Creates an index of edges.
        /// </summary>
        /// <returns>The indexed edges.</returns>
        /// <remarks>
        /// It is recommended that IndexEdges() is called after all Add*()/Remove*() calls are done.
        /// </remarks>
        public PureObjectEdge<T>[] IndexEdges()
        {
            lock (_ieLock)
            {
                if (_indexedEdges == null)
                {
                    _indexedEdges = _edges.Values.ToArray<PureObjectEdge<T>>();
                    Array.Sort(_indexedEdges);
                }
                return _indexedEdges;
            }
        }
    }
}
