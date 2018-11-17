using System;
using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEa
{
    [StorableClass]
    [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 360)]
    [Item("Robocode Problem", "Evolution of a robocode program in java using genetic programming.")]
    public class Problem : SymbolicExpressionTreeProblem
    {
        #region Parameter Names
        private const string RobocodePathParamaterName = "RobocodePath";
        private const string NrOfRoundsParameterName = "NrOfRounds";
        private const string EnemiesParameterName = "Enemies";
        private const string FeaturesParameterName = "Features";
        private readonly IList<Robot> enemies;
        private readonly Robot robot = new Robot("BPjsRobot", "il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot");

        #endregion

        #region Parameters
        public IFixedValueParameter<DirectoryValue> RobocodePathParameter {
            get { return (IFixedValueParameter<DirectoryValue>)Parameters[RobocodePathParamaterName]; }
        }
        public IFixedValueParameter<IntValue> NrOfRoundsParameter {
            get { return (IFixedValueParameter<IntValue>)Parameters[NrOfRoundsParameterName]; }
        }
        public IValueParameter<EnemyCollection> EnemiesParameter {
            get { return (IValueParameter<EnemyCollection>)Parameters[EnemiesParameterName]; }
        }

        public IValueParameter<EnemyCollection> FeaturesParameter {
            get { return (IValueParameter<EnemyCollection>)Parameters[FeaturesParameterName]; }
        }

        public string RobocodePath {
            get { return RobocodePathParameter.Value.Value; }
            set { RobocodePathParameter.Value.Value = value; }
        }

        public int NrOfRounds {
            get { return NrOfRoundsParameter.Value.Value; }
            set { NrOfRoundsParameter.Value.Value = value; }
        }

        public EnemyCollection Enemies {
            get { return EnemiesParameter.Value; }
            set { EnemiesParameter.Value = value; }
        }
        #endregion

        [StorableConstructor]
        protected Problem(bool deserializing) : base(deserializing) { }
        protected Problem(Problem original, Cloner cloner)
          : base(original, cloner)
        {
            RegisterEventHandlers();
        }

        public Problem()
          : base()
        {
            DirectoryValue robocodeDir = new DirectoryValue { Value = @"c:\Thesis\robocode" };

            EnemyCollection robotList = EnemyCollection.ReloadEnemies(robocodeDir.Value);
            FeatureCollection features = FeatureCollection.ReloadFeatures("");
            robotList.RobocodePath = robocodeDir.Value;

            Parameters.Add(new FixedValueParameter<DirectoryValue>(RobocodePathParamaterName, "Path of the Robocode installation.", robocodeDir));
            Parameters.Add(new FixedValueParameter<IntValue>(NrOfRoundsParameterName, "Number of rounds a robot has to fight against each opponent.", new IntValue(1)));
            Parameters.Add(new ValueParameter<EnemyCollection>(EnemiesParameterName, "The enemies that should be battled.", robotList));
            Parameters.Add(new ValueParameter<FeatureCollection>(FeaturesParameterName, "The enemies that should be battled.", features));

            Encoding = new SymbolicExpressionTreeEncoding(new Grammar(), 1000, 10);
            Encoding.FunctionArguments = 0;
            Encoding.FunctionDefinitions = 0;

            RegisterEventHandlers();
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Problem(this, cloner);
        }

        [StorableHook(HookType.AfterDeserialization)]
        private void AfterDeserialization() { RegisterEventHandlers(); }

        public override double Evaluate(ISymbolicExpressionTree tree, IRandom random)
        {

            return Interpreter.Evaluate(tree, RobocodePath, robot, Enemies, null, false, NrOfRounds);
        }

        public override void Analyze(ISymbolicExpressionTree[] trees, double[] qualities, ResultCollection results, IRandom random)
        {
            // find the tree with the best quality
            double maxQuality = double.NegativeInfinity;
            ISymbolicExpressionTree bestTree = null;
            for (int i = 0; i < qualities.Length; i++)
            {
                if (qualities[i] > maxQuality)
                {
                    maxQuality = qualities[i];
                    bestTree = trees[i];
                }
            }

            // create a solution instance
            var bestSolution = new Solution(bestTree, RobocodePath, NrOfRounds);

            // also add the best solution as a result to the result collection
            // or alternatively update the existing result
            if (!results.ContainsKey("BestSolution"))
            {
                results.Add(new Result("BestSolution", "The best tank program", bestSolution));
            }
            else
            {
                results["BestSolution"].Value = bestSolution;
            }
        }

        public override bool Maximization {
            get { return true; }
        }

        private void RegisterEventHandlers()
        {
            RobocodePathParameter.Value.StringValue.ValueChanged += RobocodePathParameter_ValueChanged;
        }

        void RobocodePathParameter_ValueChanged(object sender, System.EventArgs e)
        {
            EnemiesParameter.Value.RobocodePath = RobocodePathParameter.Value.Value;
        }
    }
}
