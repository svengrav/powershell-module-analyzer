using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;
using OpenSoftware.DgmlTools;
using OpenSoftware.DgmlTools.Analyses;
using OpenSoftware.DgmlTools.Builders;
using OpenSoftware.DgmlTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleAnalyzer.DgmlFormatter
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
            var builder = new DgmlBuilder(new HubNodeAnalysis(), new NodeReferencedAnalysis(), new CategoryColorAnalysis())
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
            var graph = builder.Build(commandCalls);
            graph.WriteToFile(_destinationFile);
        }
    }
}
