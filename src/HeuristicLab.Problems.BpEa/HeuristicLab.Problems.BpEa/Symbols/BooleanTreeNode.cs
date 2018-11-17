using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    [StorableClass]
    public class BooleanTreeNode : SymbolicExpressionTreeTerminalNode
    {
        private bool value;
        [Storable]
        public bool Value {
            get { return value; }
            private set { this.value = value; }
        }

        [StorableConstructor]
        protected BooleanTreeNode(bool deserializing) : base(deserializing) { }
        protected BooleanTreeNode(BooleanTreeNode original, Cloner cloner)
          : base(original, cloner)
        {
            value = original.value;
        }

        public BooleanTreeNode(BooleanValue boolValueSy) : base(boolValueSy) { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new BooleanTreeNode(this, cloner);
        }

        public override bool HasLocalParameters {
            get { return true; }
        }

        public override void ResetLocalParameters(IRandom random)
        {
            // initialization
            value = random.NextDouble() > 0.5;
        }

        public override void ShakeLocalParameters(IRandom random, double shakingFactor)
        {
            // mutation: flip value with 5% probability
            if (random.NextDouble() < 0.05) value = !value;
        }
    }
}
