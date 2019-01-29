using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Problems.BpEaGA;

namespace ProcessUtilsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //String res = ProcessUtils.ExecuteCommand(
            //    "\"C:\\Thesis\\BPEA-robot\\src\\HeuristicLab.Problems.BpEaGA\\HeuristicLab.Problems.BpEaGA\\runBattle.bat\"",
            //    new[] {"il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot", "sample.SittingDuck", "1" });
            //            String res = ProcessUtils.ExecuteCommand("ipconfig");
            String res = ProcessUtils.ExecuteCommand(@"java -Xmx512M -DNOSECURITY=true -Dsun.io.useCanonCaches=false -cp .;C:\Thesis\BPEA-robot\src\HeuristicLab.Problems.BpEaGA\HeuristicLab.Problems.BpEaGA;c:/thesis/robocode/libs/robocode.jar;C:/Users/meytal/.m2/repository/org/apache/commons/commons-jexl3/3.1/commons-jexl3-3.1.jar;C:/Users/meytal/.m2/repository/commons-logging/commons-logging/1.2/commons-logging-1.2.jar;C:/Users/meytal/.m2/repository/com/github/bthink-bgu/BPjs/0.9.6/BPjs-0.9.6.jar;C:/Users/meytal/.m2/repository/org/mozilla/rhino/1.7.9/rhino-1.7.9.jar BattleRunner  sample.Tracker c:\\Thesis\\robocode false 1 sample.SittingDuck");
            Console.Out.WriteLine(res);
            Console.Out.WriteLine(Double.Parse(res));
            Console.In.Read();
        }
    }
}
