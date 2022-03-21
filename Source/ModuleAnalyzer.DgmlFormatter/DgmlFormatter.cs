using OpenSoftware.DgmlTools;
using OpenSoftware.DgmlTools.Analyses;
using OpenSoftware.DgmlTools.Builders;
using OpenSoftware.DgmlTools.Model;
using PsModuleAnalyzer.Core.Interfaces;
using PsModuleAnalyzer.Core.Model;

namespace PsModuleAnalyzer.DgmlFormatter
{
    public class DgmlFormatter : IAnalyzerOutput
    {
        private readonly string _destinationFile;

        public DgmlFormatter(string destinationFile)
        {
            _destinationFile = destinationFile;
        }

        public void CreateAnalyzerOutupt(List<ModuleCommandCall> commandCalls)
        {
            commandCalls = commandCalls.Where(commands => !commands.Target.IsExternal).ToList();
            DgmlBuilder? builder = new DgmlBuilder(new HubNodeAnalysis(), new NodeReferencedAnalysis(), new CategoryColorAnalysis())
            {

                CategoryBuilders = new CategoryBuilder[]
                {
                    new CategoryBuilder<ModuleCommandCall>(x => new Category
                    {
                        Id = x.Source.Namespace,
                        Label = x.Source.Namespace
                    })
                },
                NodeBuilders = new NodeBuilder[]
                {
                    new NodeBuilder<ModuleCommandCall>(
                        x => new Node
                        {
                            Id = x.Source.Name,
                            Label = x.Source.Name,
                            Category = x.Source.Namespace,
                            Group = x.Source.Namespace,
                        })
                },
                LinkBuilders = new LinkBuilder[]
                {
                    new LinkBuilder<ModuleCommandCall>(
                            x => new Link
                            {
                                Source = x.Source.Name,
                                Target = x.Target.Name,
                                Stroke = "#2269E0"
                            })
                }
            };
            DirectedGraph? graph = builder.Build(commandCalls);
            graph.WriteToFile(_destinationFile);
        }
    }
}
