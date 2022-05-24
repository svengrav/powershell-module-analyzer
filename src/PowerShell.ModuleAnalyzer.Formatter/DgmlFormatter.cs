using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;
using OpenSoftware.DgmlTools;
using OpenSoftware.DgmlTools.Analyses;
using OpenSoftware.DgmlTools.Builders;
using OpenSoftware.DgmlTools.Model;

namespace ModuleAnalyzer.Formatter;

internal class DgmlFormatter : IAnalyzerOutputFormatter
{
    private readonly string _destinationFile;

    internal DgmlFormatter(string destinationFile)
    {
        _destinationFile = destinationFile;
    }

    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition)
    {
        var commandCalls = FilterInternalModuleCalls(moduleDefinition);
        var builder = new DgmlBuilder(new HubNodeAnalysis(),
            new NodeReferencedAnalysis(),
            new CategoryColorAnalysis(),
            new DgmlLinkReferenceWeightAnalyzer()) {
            CategoryBuilders = new CategoryBuilder[]
            {
                new CategoryBuilder<ModuleCommandCall>(x => new Category
                {
                    Id = x.Source.Namespace.Id,
                    Label = x.Source.Namespace.Id
                })
            },
            NodeBuilders = new NodeBuilder[]
            {
                new NodeBuilder<ModuleCommandCall>(
                    x => new Node
                    {
                        Id = x.Source.Name,
                        Label = x.Source.Name,
                        Category = x.Source.Namespace.Id,
                        Group = x.Source.Namespace.Id,
                    })
            },
            LinkBuilders = new LinkBuilder[]
            {
                new LinkBuilder<ModuleCommandCall>(
                        x => new Link
                        {
                            Source = x.Source.Name,
                            Target = x.Target.Name,
                            Stroke = "#6BB3FF"
                        })
            }
        };
        var graph = builder.Build(commandCalls);

        graph.WriteToFile(_destinationFile);
    }

    private static List<ModuleCommandCall> FilterInternalModuleCalls(ModuleDefinition moduleDefinition)
    {
        return moduleDefinition.ModuleCommandCalls
                   .Where(commands => !commands.Target.IsExternal)
                   .ToList();
    }
}
