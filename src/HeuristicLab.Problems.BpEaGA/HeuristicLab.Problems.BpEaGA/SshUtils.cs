using Renci.SshNet;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace HeuristicLab.Problems.BpEaGA
{
    public static class SshUtils
    {
        private const string SERVER = "vmsgemaster.cs.bgu.ac.il";
        private const string USER = "genahm";
        private const string PASSWORD = "Mzuta1!@";
        private const string ROOT_DIR = "/home/cluster/users/genahm";
        private const string RUN_DIR = "/";

        public static void UploadFile(string data, string tempFileName)
        {
            using (var client = new ScpClient(SERVER, 22, USER, PASSWORD))
            {
                client.RemotePathTransformation = RemotePathTransformation.ShellQuote;
                client.Connect();

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data ?? "")))
                {
                    client.Upload(ms, ROOT_DIR + "/" + tempFileName + ".txt");
                }
            }

        }

        public static string RunScript(String dirName)
        {
            string res = "0.0";
            using (var client = new SshClient(SERVER, USER, PASSWORD))
            {
                client.Connect();
                ShellStream shellStream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

                // Execute commands under root account
                //WriteStream("cd " + ROOT_DIR, shellStream);
                client.CreateCommand("cd " + ROOT_DIR).Execute();

                //string answer = ReadStream(shellStream);
                //int index = answer.IndexOf(Environment.NewLine);
                //answer = answer.Substring(index + Environment.NewLine.Length);
                //Console.WriteLine("Command output: " + answer.Trim());

                client.CreateCommand("./createEnv.sh").Execute();
                client.CreateCommand("mkdir -p " + ROOT_DIR + "/" + dirName).Execute();
                client.CreateCommand("chmod 777 " + ROOT_DIR + "/" + dirName).Execute();
                //client.CreateCommand("cd " + dirName).Execute();
                client.CreateCommand("cp -r " + ROOT_DIR + "/robocode " + ROOT_DIR + "/" + dirName + "/").Execute();
                client.CreateCommand("cp -r " + ROOT_DIR + "/CommonLibs " + ROOT_DIR + "/" + dirName + "/").Execute();
                client.CreateCommand("cp " + ROOT_DIR + "/" + dirName + ".txt " + ROOT_DIR + "/" + dirName + "/").Execute();
                client.CreateCommand("cp " + ROOT_DIR + "/runRobocodeBattle.sh " + ROOT_DIR + "/" + dirName + "/").Execute();
                client.CreateCommand("chmod 777  " + ROOT_DIR + "/" + dirName + "/CommonLibs/queueBattle.sh").Execute();
                client.CreateCommand("chmod 777  " + ROOT_DIR + "/" + dirName + "/CommonLibs/runScript.sh").Execute();
                //client.CreateCommand("cd CommonLibs").Execute();
                SshCommand command = client.CreateCommand("cd " + ROOT_DIR + " / " + dirName + " /CommonLibs; ./queueBattle.sh");

                command.Execute();
                res = command.Result;

                //WriteStream("./createEnv.sh", shellStream);
                //WriteStream("mkdir -p " + dirName, shellStream);
                //WriteStream("chmod 777 " + dirName, shellStream);
                //WriteStream("cd " + dirName, shellStream);
                //WriteStream("cp -r " + ROOT_DIR + "/robocode .", shellStream);
                //WriteStream("cp -r " + ROOT_DIR + "/CommonLibs .", shellStream);
                //WriteStream("cp " + ROOT_DIR + "/"+dirName+".txt .", shellStream);
                //WriteStream("cd CommonLibs", shellStream);
                //WriteStream("./queueBattle", shellStream);
                //WriteStream("/opt/sge/bin/lx-amd64/qsub -q sipper.q -cwd simple.sh", shellStream);

                //answer = ReadStream(shellStream);
                //index = answer.IndexOf(Environment.NewLine);
                //answer = answer.Substring(index + Environment.NewLine.Length);
                //Console.WriteLine("Command output: " + answer.Trim());
                //
                //answer = ReadStream(shellStream);
                //index = answer.IndexOf(Environment.NewLine);
                //answer = answer.Substring(index + Environment.NewLine.Length);
                //res = answer.Trim();
                //Console.WriteLine("Command output: " + res);

                //Console.ReadKey();
                client.Disconnect();
            }

            return res;
        }

        private static void WriteStream(string cmd, ShellStream stream)
        {
            stream.WriteLine(cmd + "; echo this-is-the-end");
            while (stream.Length == 0)
                Thread.Sleep(500);
        }

        private static string ReadStream(ShellStream stream)
        {
            StringBuilder result = new StringBuilder();

            string line;
            while ((line = stream.ReadLine()) != "this-is-the-end")
                result.AppendLine(line);

            return result.ToString();
        }
    }
}
