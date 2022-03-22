using PsModuleAnalyzer.Core.Factory;
using PsModuleAnalyzer.Core.Interfaces;
using PsModuleAnalyzer.Core.Model;
using PsModuleAnalyzer.Core.Visitor;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace PsModuleAnalyzer.Core.Anaylzer
{
    public class ModuleAnalyzer
    {
        public CommandVisitorFactory Factory { get; set; }
        private readonly ModuleDefinition _moduleDefinition;
        private readonly List<IAnalyzerOutput> AnalyzerOutputFormatters = new();

        internal ModuleAnalyzer(string modulePath)
        {
            _moduleDefinition = new ModuleDefinition(modulePath);
        }

        public void AddOutputFormat(IAnalyzerOutput output)
        {
            AnalyzerOutputFormatters.Add(output);
        }

        public void SetCommandVisitorFactory(CommandVisitorFactory factory)
        {
            Factory = factory;
        }

        public void Analyze()
        {
            foreach (ModuleCommand? command in _moduleDefinition.ModuleCommands)
            {
                ScriptBlockAst ast = Parser.ParseInput(
                    command.Definition,
                    out Token[] tokens,
                    out ParseError[] errors
                                );

                List<ParameterAst> paramastList = new();
                var newAst = (ScriptBlockAst) ast.Copy();
                if(ast.ParamBlock != null)
                {
                    foreach (var param in ast.ParamBlock.Parameters)
                        paramastList.Add((ParameterAst)param.Copy());
                }

                FunctionDefinitionAst fast = new FunctionDefinitionAst(newAst.Extent, false, false, command.Name, paramastList, newAst);
                CommandVisitor? commandVisitor = Factory.Create(command, _moduleDefinition);
                fast.Visit(commandVisitor);
            }

            CreateOutput();
        }

        private void CreateOutput()
        {
            AnalyzerOutputFormatters.ForEach(output => output.CreateAnalyzerOutupt(_moduleDefinition));
        }
    }
}
