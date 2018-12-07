using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Problems.BpEaGA {
  [Plugin("HeuristicLab.Problems.BpEaGA", "BP EA GA robocode problem", "1.0.0")]
  [PluginFile("HeuristicLab.Problems.BpEaGA-1.0.0.dll", PluginFileType.Assembly)]
    [PluginDependency("HeuristicLab.Collections", "3.3")]
    [PluginDependency("HeuristicLab.Core", "3.3")]
    [PluginDependency("HeuristicLab.Common", "3.3")]
    [PluginDependency("HeuristicLab.Data", "3.3")]
    [PluginDependency("HeuristicLab.Persistence", "3.3")]
    [PluginDependency("HeuristicLab.Operators", "3.3")]
    [PluginDependency("HeuristicLab.Optimization", "3.3")]
    [PluginDependency("HeuristicLab.Optimization.Operators", "3.3")]
    [PluginDependency("HeuristicLab.Encodings.RealVectorEncoding", "3.3")]
    [PluginDependency("HeuristicLab.Parameters", "3.3")]
    [PluginDependency("HeuristicLab.Encodings.BpEa.RealVector", "1.0.0")]
    public class HeuristicLabProblemsBpEaPlugin : PluginBase {
  }
}
