using System;
using PureActive.Network.Abstractions.PureObject;

namespace PureActive.Network.Devices.PureObjectGraph
{
    public class PureObjectVertex<T> : IComparable<PureObjectVertex<T>>
        where T : class, IPureObject, IComparable<IPureObject>
    {
        public PureObjectVertex(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public Guid Id
        {
            get => Value.ObjectId;
            set => Value.ObjectId = value;
        }

        public double Weight { get; set; }

        #region Overrides

        public override bool Equals(object obj)
        {
            if (!(obj is PureObjectVertex<T>))
                return false;

            return Id == ((PureObjectVertex<T>) obj).Id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Id.GetHashCode();

        #endregion

        #region Implements

        public int CompareTo(IPureObject other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(PureObjectVertex<T> other)
        {
            return Value.CompareTo(other.Value);
        }

        int IComparable<PureObjectVertex<T>>.CompareTo(PureObjectVertex<T> other)
        {
            return Value.CompareTo(other.Value);
        }

        #endregion
    }
}