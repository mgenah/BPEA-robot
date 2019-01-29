using System;
using System.Diagnostics;
using System.Threading;

namespace HeuristicLab.Problems.BpEaGA
{
    public class ProcessUtils
    {
        public static string ExecuteCommand(string command, params string[] args)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;


            Process process = new Process();
            process.EnableRaisingEvents = true; // required to be notified of exit
            process.StartInfo = processInfo;
            process.Start();
            
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            int exitCode = process.ExitCode;

//            Thread.Sleep(15000);
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();

            return output;
        }
    }
}
