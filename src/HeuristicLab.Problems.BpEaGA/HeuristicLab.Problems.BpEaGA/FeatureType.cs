using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEaGA
{
    [Item("Feature", "A collection of enemy robots for the Robocode genetic programming problem.")]
    [StorableClass]
    public class FeatureType : NamedItem
    {
        private IntValue min = new IntValue();
        private IntValue max = new IntValue();

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

        [StorableConstructor]
        protected FeatureType(bool deserializing) : base(deserializing) { }
        public FeatureType(int min, int max, string name)
            : base(name)
        {
            this.Min = new IntValue(min);
            this.Max = new IntValue(max);
        }
        protected FeatureType(FeatureType original, Cloner cloner)
          : base(original, cloner)
        {
        }
        public FeatureType() { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new FeatureType(this, cloner);
        }
    }
}
