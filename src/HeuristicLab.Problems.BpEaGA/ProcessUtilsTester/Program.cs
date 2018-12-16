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
            String res = ProcessUtils.ExecuteCommand(@"C:\Thesis\BPEA-robot\src\HeuristicLab.Problems.BpEaGA\HeuristicLab.Problems.BpEaGA\runBattle.bat", new[] { "il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot", "sample.SittingDuck", "1" });
            Console.Out.WriteLine(res);
        }
    }
}
