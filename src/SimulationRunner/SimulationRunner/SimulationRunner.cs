﻿using Renci.SshNet;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace SimulationRunner
{
    public class SimulationRunner
    {
        private const string SERVER = "vmsgemaster.cs.bgu.ac.il";
        private const string USER = "genahm";
        private const string PASSWORD = "Mg165705";
        private const string ROOT_DIR = "/home/cluster/users/genahm";

        public static void Main(string[] args)
        {
            string individual = args[0];
            string individualId = args[1];
            UploadFile(individual, individualId);

            using (var client = new SshClient(SERVER, USER, PASSWORD))
            {
                client.Connect();
                ShellStream shellStream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

                // Execute commands under root account
                WriteStream("cd " + ROOT_DIR, shellStream);

                string answer = ReadStream(shellStream);
                int index = answer.IndexOf(Environment.NewLine);
                answer = answer.Substring(index + Environment.NewLine.Length);
                Console.WriteLine("Command output: " + answer.Trim());

                WriteStream("./createEnv.sh", shellStream);
                WriteStream("/opt/sge/bin/lx-amd64/qsub -q sipper.q -cwd simple.sh", shellStream);

                answer = ReadStream(shellStream);
                index = answer.IndexOf(Environment.NewLine);
                answer = answer.Substring(index + Environment.NewLine.Length);
                Console.WriteLine("Command output: " + answer.Trim());

                answer = ReadStream(shellStream);
                index = answer.IndexOf(Environment.NewLine);
                answer = answer.Substring(index + Environment.NewLine.Length);
                Console.WriteLine("Command output: " + answer.Trim());

                Console.ReadKey();
                client.Disconnect();
            }
        }

        private static void UploadFile(string data, string tempFileName)
        {
            using (var client = new ScpClient(SERVER, 22, USER, PASSWORD))
            {
                client.RemotePathTransformation = RemotePathTransformation.ShellQuote;
                client.Connect();

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data ?? "")))
                {
                    client.Upload(ms, ROOT_DIR + "/" + tempFileName);
                }
            }
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
