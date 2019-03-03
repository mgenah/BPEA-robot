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
                indStr += featureWeights[i] + "*" + features.CheckedItems.ToArray()[i].Value.Name;
                if (i < features.Count - 1)
                {
                    indStr += "+";
                }
            }

            return RunGamesLocaly(@"c:\Thesis\robocode", indStr, robot.FullName, enemies.ToArray()[0].Value.Value, robotName, nrOfRounds);
        }

        private static double RunGamesLocaly(string path, string tree, String robot, String enemy, string robotName, int nrOfRounds)
        {
            string robotDataDir = @"C:\Thesis\robocode\robots\.data\il\ac\bgu\cs\bp\bpjsrobot\BPjsRobot.data";
            CleanPreviousRobotPolicy(robotDataDir);
            string robotsPath = Path.Combine(path, "robots", "Evaluation");
            string srcRobotPath = Path.Combine(robotDataDir, robotName + ".txt");
            File.WriteAllText(srcRobotPath, tree, Encoding.Default);
            string javaCmd = @"java -Xmx512M -DNOSECURITY=true -Dsun.io.useCanonCaches=false -cp .;C:\Thesis\BPEA-robot\src\HeuristicLab.Problems.BpEaGA\HeuristicLab.Problems.BpEaGA;c:/thesis/robocode/libs/robocode.jar;C:/Users/meytal/.m2/repository/org/apache/commons/commons-jexl3/3.1/commons-jexl3-3.1.jar;C:/Users/meytal/.m2/repository/commons-logging/commons-logging/1.2/commons-logging-1.2.jar;C:/Users/meytal/.m2/repository/com/github/bthink-bgu/BPjs/0.9.6/BPjs-0.9.6.jar;C:/Users/meytal/.m2/repository/org/mozilla/rhino/1.7.9/rhino-1.7.9.jar BattleRunner " + robot + " c:\\Thesis\\robocode false "+ nrOfRounds + " " + enemy;
            String res = ProcessUtils.ExecuteCommand(javaCmd);
            if (res.Equals("NaN"))
                return -3.0;
            return Double.Parse(res);
        }

        private static void CleanPreviousRobotPolicy(string robotDataDir)
        {
            DirectoryInfo dir = new DirectoryInfo(robotDataDir);
            foreach (FileInfo fi in dir.GetFiles("robot*.txt"))
            {
                fi.Delete();
            }
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