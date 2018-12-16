using System;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Encodings.BpEa.RealVector
{
    [StorableClass]
    [Item("BpEaRealVector", "Represents a BP EA vector of real values.")]
    public class BpEaRealVector : Encodings.RealVectorEncoding.RealVector
    {
        [StorableConstructor]
        protected BpEaRealVector(bool deserializing) : base(deserializing) { }
        protected BpEaRealVector(BpEaRealVector original, Cloner cloner) : base(original, cloner) { }
        public BpEaRealVector() : base() { }
        public BpEaRealVector(int length) : base(length) { }
        public BpEaRealVector(int length, IRandom random, double min, double max) : base(length, random, min, max) { }
        public BpEaRealVector(double[] elements) : base(elements) { }
        public BpEaRealVector(DoubleArray elements)
          : base(elements) { }
        public BpEaRealVector(BpEaRealVector other) : this(other.array) { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new BpEaRealVector(this, cloner);
        }

        public double DotProduct(BpEaRealVector other)
        {
            if (other.Length != Length) throw new ArgumentException("Vectors are of unequal length.");
            var dotProd = 0.0;
            for (var i = 0; i < Length; i++)
                dotProd += this[i] * other[i];
            return dotProd;
        }
    }
}
