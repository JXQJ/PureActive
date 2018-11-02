using System;
using PureActive.Network.Abstractions.PureObject;

namespace PureActive.Network.Devices.PureObjectGraph
{
    public class  PureObjectEdge<T>: IComparable<PureObjectEdge<T>> where T : IPureObject
    {
        /// <summary>
        /// Gets the edge's ID.
        /// </summary>
        /// <remarks>
        /// The ID is a tuple of Tail.ID, directed indicator (0 means undirected and 1 means directed), and Head.ID.
        /// </remarks>
        public (Guid tail, int directed, Guid head) Id { get; internal set; }

        /// <summary>
        /// Gets the endpoints of the edge.
        /// </summary>
        public Tuple<PureObjectVertex<T>, PureObjectVertex<T>> EndPoints { get; }

        /// <summary>
        /// Gets the tail of the edge.
        /// </summary>
        public PureObjectVertex<T> Tail
        {
            get => EndPoints.Item1;
        }

        /// <summary>
        /// Gets the head of the edge.
        /// </summary>
        public PureObjectVertex<T> Head
        {
            get => EndPoints.Item2;
        }

        /// <summary>
        /// Gets or sets the weight of the edge.
        /// </summary>
        public double Weight { get; set; }


        private bool _directed;

        /// <summary>
        /// Gets or sets if the edge is directed (true) or undirected (false).
        /// </summary>
        public bool Directed
        {
            get => _directed;
            set
            {
                if (_directed != value)
                {
                    _directed = value;
                    SetID();
                }
            }
        }

        /// <summary>
        /// Creates an instance of the edge.
        /// </summary>
        /// <param name="tail">The tail vertex.</param>
        /// <param name="head">The head vertex.</param>
        /// <param name="directed">Indicates if the edge is directed (true) or undirected (false).</param>
        /// <param name="weight">The weight of the edge. Default is 0 (unweighted).</param>
        private PureObjectEdge(PureObjectVertex<T> tail, PureObjectVertex<T> head, bool directed = false, double weight = 0)
        {
            EndPoints = new Tuple<PureObjectVertex<T>, PureObjectVertex<T>>(tail, head);
            _directed = directed;
            Weight = weight;
            SetID();
        }

        /// <summary>
        /// Creates an edge.
        /// </summary>
        /// <param name="tail">The tail vertex.</param>
        /// <param name="head">The head vertex.</param>
        /// <param name="directed">Indicates if the edge is directed or not.</param>
        /// <param name="weight">The weight of the edge. Default is 0 (unweighted).</param>
        /// <param name="label">The label of the edge.</param>
        /// <returns>The edge created.</returns>
        internal static PureObjectEdge<T> Create(PureObjectVertex<T> tail, PureObjectVertex<T> head, bool directed = false, double weight = 0) => new PureObjectEdge<T>(tail, head, directed, weight);

        /// <summary>
        /// Deconstructs an edge to its ID parts.
        /// </summary>
        /// <param name="tail">The tail ID.</param>
        /// <param name="directed">An indicator if directed (1) or undirected (0).</param>
        /// <param name="head">The head ID.</param>
        public void Deconstruct(out Guid tail, out int directed, out Guid head)
        {
            tail = EndPoints.Item1.Id;
            directed = ToDirectedIndicator(_directed);
            head = EndPoints.Item2.Id;
        }

        /// <summary>
        /// Sets the edge's ID.
        /// </summary>
        private void SetID()
        {
            (Guid tail, int directed, Guid head) = this;
            Id = (tail, directed, head);
        }

        /// <summary>
        /// Translates a directed flag to its indicator equivalent.
        /// </summary>
        /// <param name="directed">The directed flag.</param>
        /// <returns>The directed flag's indicator equivalent.</returns>
        public static int ToDirectedIndicator(bool directed) => (directed ? 1 : 0);

        public int CompareTo(PureObjectEdge<T> other)
        {
            return Id.CompareTo(other.Id);
        }

    }


}
