using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    [StorableClass]
    public class NumberTreeNode : SymbolicExpressionTreeTerminalNode
    {
        private int value;
        [Storable]
        public int Value {
            get { return value; }
            private set { this.value = value; }
        }

        [StorableConstructor]
        protected NumberTreeNode(bool deserializing) : base(deserializing) { }
        protected NumberTreeNode(NumberTreeNode original, Cloner cloner)
          : base(original, cloner)
        {
            this.value = original.value;
        }

        public NumberTreeNode(Number numberSy) : base(numberSy) { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new NumberTreeNode(this, cloner);
        }

        public override bool HasLocalParameters {
            get { return true; }
        }

        public override void ResetLocalParameters(IRandom random)
        {
            // random initialization
            value = random.Next(-360, 360);
        }

        public override void ShakeLocalParameters(IRandom random, double shakingFactor)
        {
            // random mutation (cyclic)
            value = value + random.Next(-20, 20);
            if (value < -360) value += 2 * 360;
            else if (value > 359) value -= 2 * 360;
        }
    }
}
