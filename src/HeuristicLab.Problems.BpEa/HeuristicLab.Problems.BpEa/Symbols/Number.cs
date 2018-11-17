using System.Collections.Generic;
using System.Globalization;
using HeuristicLab.Common;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    [StorableClass]
    public class Number : Symbol
    {
        public override int MinimumArity { get { return 0; } }
        public override int MaximumArity { get { return 0; } }

        [StorableConstructor]
        protected Number(bool deserializing) : base(deserializing) { }
        protected Number(Number original, Cloner cloner) : base(original, cloner) { }

        public Number() : base("Number", "A number.") { }

        public override ISymbolicExpressionTreeNode CreateTreeNode()
        {
            return new NumberTreeNode(this);
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Number(this, cloner);
        }
    }
}
