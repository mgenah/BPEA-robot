using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Problems.BpEa.Symbols;

namespace HeuristicLab.Problems.BpEa
{
    public static class Interpreter
    {
        private static readonly object syncRoot = new object();

        public static double Evaluate(ISymbolicExpressionTree tree, string path, Robot robot, EnemyCollection enemies, string robotName = null, bool showUI = false, int nrOfRounds = 200)
        {
            if (robotName == null)
                robotName = GenerateRobotName();

            string interpretedTree = InterpretProgramTree(tree.Root, robotName);

            return RunGamesLocaly(@"c:\Thesis\robocode", interpretedTree, robotName);
        }

        private static double RunGamesLocaly(string path, string tree, string robotName)
        {
            string robotsPath = Path.Combine(path, "robots", "Evaluation");
            string srcRobotPath = Path.Combine(robotsPath, robotName + ".txt");

            File.WriteAllText(srcRobotPath, tree, Encoding.Default);
            return Double.Parse(ProcessUtils.ExecuteCommand(@"C:\Studies\Thesis\Project\bp-ea\src\HeuristicLab.Problems.BpEa\HeuristicLab.Problems.BpEa\runBattle.bat", ""));
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

        public static string InterpretProgramTree(ISymbolicExpressionTreeNode node, string robotName)
        {
            var tankNode = node;
            while (tankNode.Symbol.Name != "NumericalExpression")
                tankNode = tankNode.GetSubtree(0);

            string result = Interpret(tankNode);
            result = result.Replace("class output", "class " + robotName);
            return result;
        }

        private static string Interpret(ISymbolicExpressionTreeNode node)
        {
            switch (node.Symbol.Name)
            {
                case "Statement": return InterpretStat(node);
                case "DoNothing": return string.Empty;
                case "EmptyEvent": return string.Empty;

                case "Addition": return InterpretBinaryOperator(" + ", node);
                case "Multiplication": return InterpretBinaryOperator(" * ", node);

                case "Equal": return InterpretComparison(" == ", node);
                case "LessThan": return InterpretComparison(" < ", node);
                case "LessThanOrEqual": return InterpretComparison(" <= ", node);
                case "GreaterThan": return InterpretComparison("  >", node);
                case "GreaterThanOrEqual": return InterpretComparison(" >= ", node);
                case "ConditionalAnd": return InterpretBinaryOperator(" && ", node);
                case "ConditionalOr": return InterpretBinaryOperator(" || ", node);
                case "Negation": return InterpretFunc1("!", node);

                case "IfThenElseStat": return InterpretIf(node);
                case "BooleanExpression": return InterpretChild(node);
                case "NumericalExpression": return InterpretChild(node);
                case "Number": return InterpretNumber(node);
                case "BooleanValue": return InterpretBoolValue(node);
                case "Feature": return InterpretFeature(node);

                default: throw new ArgumentException(string.Format("Found an unknown symbol {0} in a robocode solution", node.Symbol.Name));
            }
        }

        private static string InterpretFeature(ISymbolicExpressionTreeNode node)
        {
            var featureNode = (FeatureTreeNode)node;
            return string.Format(NumberFormatInfo.InvariantInfo, "{0}*{1}", featureNode.Name, featureNode.Weight).ToLower();
        }

        private static string InterpretBoolValue(ISymbolicExpressionTreeNode node)
        {
            var boolNode = (BooleanTreeNode)node;
            return string.Format(NumberFormatInfo.InvariantInfo, "{0}", boolNode.Value).ToLower();
        }

        private static string InterpretNumber(ISymbolicExpressionTreeNode node)
        {
            var numberNode = (NumberTreeNode)node;
            return string.Format(NumberFormatInfo.InvariantInfo, "{0}", numberNode.Value);
        }

        internal static string InterpretStat(ISymbolicExpressionTreeNode node)
        {
            // must only have one sub-tree
            Contract.Assert(node.SubtreeCount == 1);
            return Interpret(node.GetSubtree(0)) + " ;" + Environment.NewLine;
        }

        internal static string InterpretIf(ISymbolicExpressionTreeNode node)
        {
            ISymbolicExpressionTreeNode condition = null, truePart = null, falsePart = null;
            string[] parts = new string[3];

            if (node.SubtreeCount < 2 || node.SubtreeCount > 3)
                throw new Exception("Unexpected number of children. Expected 2 or 3 children.");

            condition = node.GetSubtree(0);
            truePart = node.GetSubtree(1);
            if (node.SubtreeCount == 3)
                falsePart = node.GetSubtree(2);

            parts[0] = Interpret(condition);
            parts[1] = Interpret(truePart);
            if (falsePart != null) parts[2] = Interpret(falsePart);

            return string.Format("if ({0}) {{ {1} }} else {{ {2} }}", Interpret(condition), Interpret(truePart),
              falsePart == null ? string.Empty : Interpret(falsePart));
        }

        public static string InterpretBinaryOperator(string opSy, ISymbolicExpressionTreeNode node)
        {
            if (node.SubtreeCount < 2)
                throw new ArgumentException(string.Format("Expected at least two children in {0}.", node.Symbol), "node");

            string result = string.Join(opSy, node.Subtrees.Select(Interpret));
            return "(" + result + ")";
        }

        public static string InterpretChild(ISymbolicExpressionTreeNode node)
        {
            if (node.SubtreeCount != 1)
                throw new ArgumentException(string.Format("Expected exactly one child in {0}.", node.Symbol), "node");

            return Interpret(node.GetSubtree(0));
        }

        public static string InterpretComparison(string compSy, ISymbolicExpressionTreeNode node)
        {
            ISymbolicExpressionTreeNode lhs = null, rhs = null;
            if (node.SubtreeCount != 2)
                throw new ArgumentException(string.Format("Expected exactly two children in {0}.", node.Symbol), "node");

            lhs = node.GetSubtree(0);
            rhs = node.GetSubtree(1);

            return Interpret(lhs) + compSy + Interpret(rhs);
        }

        public static string InterpretFunc1(string functionId, ISymbolicExpressionTreeNode node)
        {
            if (node.SubtreeCount != 1)
                throw new ArgumentException(string.Format("Expected 1 child in {0}.", node.Symbol.Name), "node");

            return string.Format("{0}({1})", functionId, Interpret(node.GetSubtree(0)));
        }
    }
}
