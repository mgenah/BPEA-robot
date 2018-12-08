using System;
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

        public static FeatureCollection ReloadFeatures(DateTime time)
        {
            FeatureCollection features = new FeatureCollection
            {
                new FeatureType(0, 1, "fire"),
                new FeatureType(0, 1, "power"),
                new FeatureType(0, 1, "intelligence"),
                new FeatureType(0, 1, "strategy"),
                new FeatureType(0, 1, "completeOp"),
                new FeatureType(0, 1, "ram"),
                new FeatureType(0, 1, "aim"),
                new FeatureType(0, 1, "avoidHit")
            };

            return features;
        }

    }
}
