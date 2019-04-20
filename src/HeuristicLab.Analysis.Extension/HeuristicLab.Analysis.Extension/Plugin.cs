using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Analysis {
  /// <summary>
  /// Plugin class for HeuristicLab.Analysis plugin.
  /// </summary>
  [Plugin("HeuristicLab.Analysis.Extension", "3.3.0.0")]
  [PluginFile("HeuristicLab.Analysis.Extension-3.3.dll", PluginFileType.Assembly)]
  [PluginDependency("HeuristicLab.Collections", "3.3")]
  [PluginDependency("HeuristicLab.Common", "3.3")]
  [PluginDependency("HeuristicLab.Core", "3.3")]
  [PluginDependency("HeuristicLab.Data", "3.3")]
  [PluginDependency("HeuristicLab.Operators", "3.3")]
  [PluginDependency("HeuristicLab.Optimization", "3.3")]
  [PluginDependency("HeuristicLab.Parameters", "3.3")]
  [PluginDependency("HeuristicLab.Persistence", "3.3")]

  public class HeuristicLabAnalysisExtensionPlugin : PluginBase {
  }
}