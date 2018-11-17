using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa
{
    [StorableClass]
    [Item("Solution", "Robocode program and configuration.")]
    public sealed class Solution : Item
    {
        [Storable]
        public ISymbolicExpressionTree Tree { get; set; }

        [Storable]
        public string Path { get; set; }

        [Storable]
        public int NrOfRounds { get; set; }

        [StorableConstructor]
        private Solution(bool deserializing) : base(deserializing) { }
        private Solution(Solution original, Cloner cloner)
          : base(original, cloner)
        {
            Tree = cloner.Clone(original.Tree);
            Path = (string)original.Path.Clone();
            NrOfRounds = original.NrOfRounds;
        }

        public Solution(ISymbolicExpressionTree tree, string path, int nrOfRounds)
          : base()
        {
            this.Tree = tree;
            this.Path = path;
            this.NrOfRounds = nrOfRounds;
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Solution(this, cloner);
        }
    }
}
