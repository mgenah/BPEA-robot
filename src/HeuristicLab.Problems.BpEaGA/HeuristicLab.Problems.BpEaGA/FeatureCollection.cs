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
                new FeatureType(0, 1, "energy"),
                new FeatureType(0, 1, "velocity"),
                new FeatureType(0, 1, "gunHeat"),
                new FeatureType(0, 1, "bodyTurnRemaining"),
                new FeatureType(0, 1, "radarTurnRemaining"),
                new FeatureType(0, 1, "gunTurnRemaining"),
                new FeatureType(0, 1, "distanceRemaining"),
                new FeatureType(0, 1, "enemyEnergy"),
                new FeatureType(0, 1, "enemyVelocity"),
                new FeatureType(0, 1, "distanceToEnemy")
            };

            return features;
        }

    }
}
