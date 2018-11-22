using System;
using System.IO;
using System.Linq;
using System.Text;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Optimization;

namespace HeuristicLab.Problems.BpEaGA
{
    public static class Interpreter
    {
        private static readonly object syncRoot = new object();

        public static double Evaluate(Individual individual, FeatureCollection features, string path, Robot robot,
            EnemyCollection enemies, string robotName = null, bool showUI = false, int nrOfRounds = 200)
        {
            if (robotName == null)
                robotName = GenerateRobotName();
            RealVector realVector = individual.RealVector();
            double[] featureWeights = realVector.ToArray();
            String indStr = "";
            for (int i = 0; i < featureWeights.Length; i++)
            {
                indStr += featureWeights[i] + "*" + features.ToArray()[i].Name;
            }

            return RunGamesLocaly(@"c:\Thesis\robocode", indStr, robotName);
        }

        private static double RunGamesLocaly(string path, string tree, string robotName)
        {
            string robotsPath = Path.Combine(path, "robots", "Evaluation");
            string srcRobotPath = Path.Combine(robotsPath, robotName + ".txt");

            File.WriteAllText(srcRobotPath, tree, Encoding.Default);
            return Double.Parse(ProcessUtils.ExecuteCommand(
                @"C:\Studies\Thesis\Project\bp-ea\src\HeuristicLab.Problems.BpEa\HeuristicLab.Problems.BpEa\runBattle.bat",
                ""));
        }

        private static double RunGamesRemotely(string tree, string robotName)
        {
            SshUtils.UploadFile(tree, robotName);

            string res = SshUtils.RunScript();
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
            outputname = outputname.Insert(0, "R");
            return outputname;
        }
    }
}