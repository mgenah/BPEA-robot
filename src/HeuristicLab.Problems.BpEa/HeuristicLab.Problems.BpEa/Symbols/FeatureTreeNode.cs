using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    public class FeatureTreeNode : SymbolicExpressionTreeTerminalNode
    { 
        private int min;
        private int max;
        private double weight;
        private string name;

        [Storable]
        public int Min {
            get { return min; }
            private set { this.min = value; }
        }

        [Storable]
        public int Max {
            get { return max; }
            private set { this.max = value; }
        }

        [Storable]
        public double Weight {
            get { return weight; }
            private set { this.weight = value; }
        }
        [Storable]
        public string Name {
            get { return name; }
            private set { this.name = value; }
        }

        [StorableConstructor]
        private FeatureTreeNode(bool deserializing) : base(deserializing) { }
        private FeatureTreeNode(FeatureTreeNode original, Cloner cloner)
          : base(original, cloner)
        {
            this.min = original.min;
            this.max = original.max;
            this.name = original.name;
            this.weight = original.weight;
        }

        public FeatureTreeNode() : base(new Feature())
        {
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new FeatureTreeNode(this, cloner);
        }
        
        public override bool HasLocalParameters {
            get { return true; }
        }
        
        public override void ResetLocalParameters(IRandom random)
        {
            name = "Fire";
            weight = random.NextDouble() * (max - min) + min;
        }
    }
}