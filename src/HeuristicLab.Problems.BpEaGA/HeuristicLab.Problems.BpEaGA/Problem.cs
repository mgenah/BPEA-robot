using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.BpEaGA
{
    [StorableClass]
    [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 360)]
    [Item("Robocode Problem", "Evolution of a robocode program in java using genetic programming.")]
    public class Problem : SingleObjectiveBasicProblem<RealVectorEncoding>
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

        public IValueParameter<FeatureCollection> FeaturesParameter {
            get { return (IValueParameter<FeatureCollection>)Parameters[FeaturesParameterName]; }
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

        public FeatureCollection Features {
            get { return FeaturesParameter.Value; }
            set { FeaturesParameter.Value = value; }
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
        {
            DirectoryValue robocodeDir = new DirectoryValue { Value = @"c:\Thesis\robocode" };

            EnemyCollection robotList = EnemyCollection.ReloadEnemies(robocodeDir.Value);
            FeatureCollection features = FeatureCollection.ReloadFeatures("");
            robotList.RobocodePath = robocodeDir.Value;

            Parameters.Add(new FixedValueParameter<DirectoryValue>(RobocodePathParamaterName, "Path of the Robocode installation.", robocodeDir));
            Parameters.Add(new FixedValueParameter<IntValue>(NrOfRoundsParameterName, "Number of rounds a robot has to fight against each opponent.", new IntValue(1)));
            Parameters.Add(new ValueParameter<EnemyCollection>(EnemiesParameterName, "The enemies that should be battled.", robotList));
            Parameters.Add(new ValueParameter<FeatureCollection>(FeaturesParameterName, "The enemies that should be battled.", features));

            Encoding = new RealVectorEncoding("FeaturesWeights", 1, new List<double>{-5}, new List<double>{5});

            RegisterEventHandlers();
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Problem(this, cloner);
        }

        [StorableHook(HookType.AfterDeserialization)]
        private void AfterDeserialization() { RegisterEventHandlers(); }

        public override double Evaluate(Individual individual, IRandom random)
        {
            return Interpreter.Evaluate(individual, Features, RobocodePath, robot, Enemies, null, false, NrOfRounds);
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
