using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Analysis {
  /// <summary>
  /// An operator that extracts (clones) the scope containing the best quality.
  /// </summary>
  [Item("BestScopeSolutionExporterAnalyzer", "An operator that extracts the scope containing the best quality every K generations.")]
  [StorableClass]
  public class BestScopeSolutionExporterAnalyzer : ValuesCollector, IAnalyzer {
    public virtual bool EnabledByDefault {
      get { return true; }
    }

    public LookupParameter<BoolValue> MaximizationParameter {
      get { return (LookupParameter<BoolValue>)Parameters["Maximization"]; }
    }
    public ScopeTreeLookupParameter<DoubleValue> QualityParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>)Parameters["Quality"]; }
    }
    public IValueParameter<ResultCollection> BestSolutionsParameter {
      get { return (IValueParameter<ResultCollection>)Parameters["BestSolutions"]; }
    }
    public IValueLookupParameter<IntValue> SkipParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["Skip"]; }
    }
    public ILookupParameter<IntValue> GenerationsParameter {
      get { return (ILookupParameter<IntValue>)Parameters["Generations"]; }
    }

    #region Storing & Cloning
    [StorableConstructor]
    protected BestScopeSolutionExporterAnalyzer(bool deserializing) : base(deserializing) { }
    protected BestScopeSolutionExporterAnalyzer(BestScopeSolutionExporterAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new BestScopeSolutionExporterAnalyzer(this, cloner);
    }
    #endregion
    public BestScopeSolutionExporterAnalyzer()
      : base() {
      Parameters.Add(new LookupParameter<BoolValue>("Maximization", "True if the problem is a maximization problem."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>("Quality", "The qualities of the solutions."));
      Parameters.Add(new ValueParameter<ResultCollection>("BestSolutions", "The best solutions."));
      Parameters.Add(new ValueLookupParameter<IntValue>("Skip", "Every Skip generations, the best individual will be stored. Skip > 0."));
      Parameters.Add(new LookupParameter<IntValue>("Generations", "The current generation id."));
    }

    public override IOperation Apply() {
      if (SkipParameter.ActualValue == null) throw new InvalidOperationException("BestScopeSolutionExporterAnalyzer: Parameter " + SkipParameter.ActualName + " could not be found.");
      if (SkipParameter.ActualValue.Value == 0) throw new InvalidOperationException("BestScopeSolutionExporterAnalyzer: Parameter " + SkipParameter.ActualName + " must be larger than 0.");

      ItemArray<DoubleValue> qualities = QualityParameter.ActualValue;
      bool max = MaximizationParameter.ActualValue.Value;

      int i = -1;
      if (!max)
        i = qualities.Select((x, index) => new { index, x.Value }).OrderBy(x => x.Value).First().index;
      else i = qualities.Select((x, index) => new { index, x.Value }).OrderByDescending(x => x.Value).First().index;

      IEnumerable<IScope> scopes = new IScope[] { ExecutionContext.Scope };
      for (int j = 0; j < QualityParameter.Depth; j++)
        scopes = scopes.Select(x => (IEnumerable<IScope>)x.SubScopes).Aggregate((a, b) => a.Concat(b));
      IScope currentBestScope = scopes.ToList()[i].Clone() as IScope;

      foreach (IParameter param in CollectedValues) {
        IItem value = param.ActualValue;
        if (value != null) {
          ILookupParameter lookupParam = param as ILookupParameter;
          string name = lookupParam != null ? lookupParam.TranslatedName : param.Name;
          currentBestScope.Variables.Add(new Variable(name, value.Clone() as IItem));
        }
      }

      if (GenerationsParameter.ActualValue.Value == 0) BestSolutionsParameter.ActualValue = new ResultCollection();

      if (GenerationsParameter.ActualValue.Value % SkipParameter.ActualValue.Value == 0) {
        (BestSolutionsParameter.ActualValue as ResultCollection).Add(new Result(String.Format("{0:0000}", GenerationsParameter.ActualValue.Value), currentBestScope));
      }

      return base.Apply();
    }
  }
}
