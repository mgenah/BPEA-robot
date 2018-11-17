using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa
{
    [Item("Feature", "A collection of enemy robots for the Robocode genetic programming problem.")]
    [StorableClass]
    public class FeatureType : Item
    {
        private IntValue min;
        private IntValue max;
        private StringValue name;

        [Storable]
        public IntValue Min {
            get { return min; }
            private set { this.min = value; }
        }

        [Storable]
        public IntValue Max {
            get { return max; }
            private set { this.max = value; }
        }

        [Storable]
        public StringValue Name {
            get { return name; }
            private set { this.name = value; }
        }

        [StorableConstructor]
        protected FeatureType(bool deserializing) : base(deserializing) { }
        public FeatureType(IntValue min, IntValue max, StringValue name)
        {
            this.Min = min;
            this.Max = max;
            this.Name = name;
        }
        protected FeatureType(FeatureType original, Cloner cloner)
          : base(original, cloner)
        {
        }
        public FeatureType() : base() { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new FeatureType(this, cloner);
        }
    }
}
