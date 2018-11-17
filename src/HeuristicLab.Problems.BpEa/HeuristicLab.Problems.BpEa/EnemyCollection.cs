using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa
{
    [Item("EnemyCollection", "A collection of enemy robots for the Robocode genetic programming problem.")]
    [StorableClass]
    public class EnemyCollection : CheckedItemList<StringValue>
    {
        private const string sampleRobotToSelect = "sample.Crazy";

        [Storable]
        public string RobocodePath { get; set; }

        [StorableConstructor]
        protected EnemyCollection(bool deserializing) : base(deserializing) { }
        protected EnemyCollection(EnemyCollection original, Cloner cloner)
          : base(original, cloner)
        {
            RobocodePath = original.RobocodePath;
        }
        public EnemyCollection() : base() { }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new EnemyCollection(this, cloner);
        }

        public static EnemyCollection ReloadEnemies(string robocodeDir)
        {
            EnemyCollection robotList = new EnemyCollection();

            try
            {
                var robotsDir = new DirectoryInfo(Path.Combine(robocodeDir, "robots"));
                var robotFiles = robotsDir.GetFiles("*", SearchOption.AllDirectories)
                  .Where(x => x.Extension == ".class" || x.Extension == ".jar");
                var robotSet = new Dictionary<string, StringValue>();
                foreach (var robot in robotFiles)
                {
                    string robotName = Path.Combine(robot.DirectoryName, Path.GetFileNameWithoutExtension(robot.FullName));
                    robotName = robotName.Remove(0, robotsDir.FullName.Length);
                    string[] nameParts = robotName.Split(new[] { Path.DirectorySeparatorChar },
                      StringSplitOptions.RemoveEmptyEntries);
                    robotName = string.Join(".", nameParts);
                    robotSet[robot.FullName] = new StringValue(robotName);
                }
                robotList.AddRange(robotSet.Values);

                foreach (var robot in robotList)
                {
                    robotList.SetItemCheckedState(robot, false);
                }
            }
            catch { }

            //select one robot so that if a user tries out the Robocode problem it works with the default settings
            if (robotList.Exists(x => x.Value == sampleRobotToSelect))
            {
                StringValue sampleRobot = robotList.Single(x => x.Value == sampleRobotToSelect);
                robotList.SetItemCheckedState(sampleRobot, true);
            }

            return robotList;
        }
    }
}