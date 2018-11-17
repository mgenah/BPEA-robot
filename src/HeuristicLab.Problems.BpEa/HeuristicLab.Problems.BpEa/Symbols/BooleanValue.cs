using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    [StorableClass]
    public class BooleanValue : Symbol
    {
        public override int MinimumArity { get { return 0; } }
        public override int MaximumArity { get { return 0; } }

        [StorableConstructor]
        protected BooleanValue(bool deserializing) : base(deserializing) { }
        protected BooleanValue(BooleanValue original, Cloner cloner) : base(original, cloner) { }

        public BooleanValue() : base("BooleanValue", "A Boolean value.") { }

        public override ISymbolicExpressionTreeNode CreateTreeNode()
        {
            return new BooleanTreeNode(this);
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new BooleanValue(this, cloner);
        }
    }
}
