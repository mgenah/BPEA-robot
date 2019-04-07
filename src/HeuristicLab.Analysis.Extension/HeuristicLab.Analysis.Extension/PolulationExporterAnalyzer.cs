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
  [Item("PolulationExporterAnalyzer", "An operator that extracts the entire population every K generations.")]
  [StorableClass]
  public class PolulationExporterAnalyzer : ValuesCollector, IAnalyzer {
    public virtual bool EnabledByDefault {
      get { return true; }
    }

    public IValueParameter<ScopeList> ExportedPopulationsParameter {
      get { return (IValueParameter<ScopeList>)Parameters["ExportedPopulations"]; }
    }
    public IValueLookupParameter<IntValue> SkipParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["Skip"]; }
    }
    public ILookupParameter<IntValue> GenerationsParameter {
      get { return (ILookupParameter<IntValue>)Parameters["Generations"]; }
    }
    public ScopeTreeLookupParameter<DoubleValue> QualityParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>)Parameters["Quality"]; }
    }

    #region Storing & Cloning
    [StorableConstructor]
    protected PolulationExporterAnalyzer(bool deserializing) : base(deserializing) { }
    protected PolulationExporterAnalyzer(PolulationExporterAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new PolulationExporterAnalyzer(this, cloner);
    }
    #endregion
    public PolulationExporterAnalyzer()
      : base() {
      Parameters.Add(new ValueLookupParameter<IntValue>("Skip", "Every Skip generations, the best individual will be stored. Skip > 0."));
      Parameters.Add(new LookupParameter<IntValue>("Generations", "The current generation id."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>("Quality", "The qualities of the solutions."));
      Parameters.Add(new ValueParameter<ScopeList>("ExportedPopulations", "The exported populations."));
    }

    public override IOperation Apply() {
      if (SkipParameter.ActualValue == null) throw new InvalidOperationException("BestScopeSolutionExporterAnalyzer: Parameter " + SkipParameter.ActualName + " could not be found.");
      if (SkipParameter.ActualValue.Value == 0) throw new InvalidOperationException("BestScopeSolutionExporterAnalyzer: Parameter " + SkipParameter.ActualName + " must be larger than 0.");

      IEnumerable<IScope> scopes = new IScope[] { ExecutionContext.Scope };
      for (int j = 0; j < QualityParameter.Depth; j++)
        scopes = scopes.Select(x => (IEnumerable<IScope>)x.SubScopes).Aggregate((a, b) => a.Concat(b));
      IScope generation = new Scope(String.Format("Generation {0:0000}", GenerationsParameter.ActualValue.Value));
      foreach (IScope scope in scopes)
        generation.SubScopes.Add(scope);

      foreach (IParameter param in CollectedValues) {
        IItem value = param.ActualValue;
        if (value != null) {
          ILookupParameter lookupParam = param as ILookupParameter;
          string name = lookupParam != null ? lookupParam.TranslatedName : param.Name;
          generation.Variables.Add(new Variable(name, value.Clone() as IItem));
        }
      }

      if (GenerationsParameter.ActualValue.Value == 0) ExportedPopulationsParameter.ActualValue = new ScopeList();

      if (GenerationsParameter.ActualValue.Value % SkipParameter.ActualValue.Value == 0)
        (ExportedPopulationsParameter.ActualValue as ScopeList).Add(generation);

      return base.Apply();
    }
  }
}
