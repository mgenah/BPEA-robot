using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HeuristicLab.Collections;
using HeuristicLab.Data;
using HeuristicLab.Encodings.BpEa.RealVector;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Optimization;

namespace HeuristicLab.Problems.BpEaGA
{
    public static class Interpreter
    {
        private static readonly object syncRoot = new object();

        public static double Evaluate(Individual individual, FeatureCollection features, string path, Robot robot,
            IEnumerable<IndexedItem<StringValue>> enemies, string robotName = null, bool showUI = false, int nrOfRounds = 200)
        {
            if (robotName == null)
                robotName = GenerateRobotName();
            RealVector realVector = individual.BpEaRealVector();
            double[] featureWeights = realVector.ToArray();
            String indStr = "";
            for (int i = 0; i < features.Count ; i++)
            {
                indStr += featureWeights[0] + "*" + features.CheckedItems.ToArray()[i].Value.Name;
                if (i < features.Count - 1)
                {
                    indStr += "+";
                }
            }

            return RunGamesLocaly(@"c:\Thesis\robocode", indStr, robot.FullName, enemies.ToArray()[0].Value.Value, robotName, nrOfRounds);
        }

        private static double RunGamesLocaly(string path, string tree, String robot, String enemy, string robotName, int nrOfRounds)
        {
            string robotsPath = Path.Combine(path, "robots", "Evaluation");
            string srcRobotPath = Path.Combine(robotsPath, robotName + ".txt");
            File.WriteAllText(srcRobotPath, tree, Encoding.Default);
            String res = ProcessUtils.ExecuteCommand(
                @"C:\Thesis\BPEA-robot\src\HeuristicLab.Problems.BpEaGA\HeuristicLab.Problems.BpEaGA\runBattle.bat",
                robot, enemy, "" + nrOfRounds);
            if (res.Equals("NaN"))
                return -3.0;
            return Double.Parse(res);
        }

        private static double RunGamesRemotely(string tree, string robotName)
        {
            SshUtils.UploadFile(tree, robotName);

            string res = SshUtils.RunScript();
            if (res.Equals("NaN"))
                return -3.0;
            return Double.Parse(res);
        }

        private static void DeleteRobotFiles(string path, string outputname)
        {
            File.Delete(path + @"\robots\Evaluation\" + outputname + ".java");
            File.Delete(path + @"\robots\Evaluation\" + outputname + ".class");
        }

        private static string GetFileName(string path, string pattern)
        {
            string fileName = string.Empty;
            try
            {
                fileName = Directory.GetFiles(path, pattern).First();
            }
            catch
            {
                throw new Exception("Error finding required Robocode files.");
            }

            return fileName;
        }

        private static string GenerateRobotName()
        {
            // Robocode class names are 32 char max and 
            // Java class names have to start with a letter
            string outputname = Guid.NewGuid().ToString();
            outputname = outputname.Remove(8, 1);
            outputname = outputname.Remove(12, 1);
            outputname = outputname.Remove(16, 1);
            outputname = outputname.Remove(20, 1);
            outputname = outputname.Remove(0, 1);
            outputname = outputname.Insert(0, "robot");
            return outputname;
        }
    }
}