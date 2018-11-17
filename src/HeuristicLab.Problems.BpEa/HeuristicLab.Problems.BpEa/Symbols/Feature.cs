using HeuristicLab.Common;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa.Symbols
{
    [StorableClass]
    public class Feature : Symbol
    {
        public override int MinimumArity { get { return 0; } }
        public override int MaximumArity { get { return 0; } }

        [StorableConstructor]
        private Feature(bool deserializing) : base(deserializing) { }

        public Feature() : base("Feature", "Weighted feature of a robot.")
        {
        }

        protected Feature(Symbol original, Cloner cloner) : base(original, cloner)
        {
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Feature(this, cloner);
        }

        public override ISymbolicExpressionTreeNode CreateTreeNode()
        {
            return new FeatureTreeNode();
        }
    }
}
