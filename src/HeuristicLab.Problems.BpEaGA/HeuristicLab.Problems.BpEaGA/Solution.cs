using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEaGA
{
    [StorableClass]
    [Item("Solution", "Robocode program and configuration.")]
    public sealed class Solution : Item
    {
        [Storable] public RealVector FeatureValues { get; set; }

        [Storable] public string Path { get; set; }

        [Storable] public int NrOfRounds { get; set; }

        [StorableConstructor]
        private Solution(bool deserializing) : base(deserializing)
        {
        }

        private Solution(Solution original, Cloner cloner)
            : base(original, cloner)
        {
            FeatureValues = cloner.Clone(original.FeatureValues);
            Path = (string) original.Path.Clone();
            NrOfRounds = original.NrOfRounds;
        }

        public Solution(RealVector featureValues, string path, int nrOfRounds)
        {
            this.FeatureValues = featureValues;
            this.Path = path;
            this.NrOfRounds = nrOfRounds;
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Solution(this, cloner);
        }
    }
}