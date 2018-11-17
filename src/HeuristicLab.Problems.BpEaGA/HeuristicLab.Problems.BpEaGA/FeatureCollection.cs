using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEaGA
{
    [Item("FeatureCollection", "A collection of enemy robots for the Robocode genetic programming problem.")]
    [StorableClass]
    public class FeatureCollection : CheckedItemList<FeatureType>
    {
        [StorableConstructor]
        protected FeatureCollection(bool deserializing) : base(deserializing) { }
        protected FeatureCollection(FeatureCollection original, Cloner cloner)
          : base(original, cloner)
        {
        }
        public FeatureCollection() : base() { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new FeatureCollection(this, cloner);
        }

        public static FeatureCollection ReloadFeatures(string robocodeDir)
        {
            FeatureCollection features = new FeatureCollection();
            FeatureType feature1 = new FeatureType(new IntValue(-1), new IntValue(1), new StringValue("Fire"));
            features.Add(feature1);

            return features;
        }

    }
}
