using PSModuleAnalyzer.Core.Factory;
using PSModuleAnalyzer.Core.Interfaces;
using PSModuleAnalyzer.Core.Model;
using PSModuleAnalyzer.Core.Repository;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace PSModuleAnalyzer.Core.Anaylzer
{
    public class ModuleAnalyzer
    {
        public CommandVisitorFactory Factory { get; set; }
        private readonly ModuleDefinition _moduleDefinition;
        private readonly List<IAnalyzerOutput> AnalyzerOutputFormatters = new();

        internal ModuleAnalyzer(string modulePath)
        {
            PSModuleInfo? moduleInfo = LoadModuleFromPath(modulePath);
            _moduleDefinition = new ModuleDefinition(moduleInfo.Name, moduleInfo.ModuleBase);

            var commands = CreateModuleCommandList(moduleInfo).ToHashSet();
            _moduleDefinition.AddModuleCommands(commands);
        }

        public void AddOutputFormat(IAnalyzerOutput output)
        {
            AnalyzerOutputFormatters.Add(output);
        }

        public void SetFactory(CommandVisitorFactory factory)
        {
            Factory = factory;
        }

        public void Analyze()
        {
            foreach (ModuleCommand? command in _moduleDefinition.ModuleCommands)
            {
                ScriptBlockAst? ast = Parser.ParseInput(
                    command.Definition,
                    out Token[] tokens,
                    out ParseError[] errors
                                );

                Visitor.CommandVisitor? commandVisitor = Factory.Create(command, _moduleDefinition);
                ast.Visit(commandVisitor);
            }

            CreateOutput(_moduleDefinition.ModuleCommandCalls);
        }

        private void CreateOutput(List<ModuleCommandCall> commandCalls)
        {
            AnalyzerOutputFormatters.ForEach(output => output.CreateAnalyzerOutupt(commandCalls));
        }

        private static PSModuleInfo LoadModuleFromPath(string modulePath)
        {
            PowerShell? ps = PowerShell.Create();
            ps.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted; Get-ExecutionPolicy");
            ps.Commands.AddScript($"Import-Module {modulePath} -PassThru");
            return ps.Invoke<PSModuleInfo>().First();
        }

        private string GetCommandNamespace(string name, string abosoluteCommandPath)
        {
            return abosoluteCommandPath
                .Replace(_moduleDefinition.Path, "")
                .Replace($"{name}.ps1", "")
                .Trim('\\')
                .Replace("\\", ".");
        }

        private List<ModuleCommand> CreateModuleCommandList(PSModuleInfo moduleInfo)
        {
            List<ModuleCommand>? moduleCommandList = new List<ModuleCommand>();
            foreach (KeyValuePair<string, FunctionInfo> command in moduleInfo.ExportedFunctions)
            {
                ModuleCommand? moduleCommand = new ModuleCommand(command.Value.Name, command.Value.Definition, GetCommandNamespace(command.Value.Name, command.Value.ScriptBlock.File));
                try
                {
                    foreach (KeyValuePair<string, ParameterMetadata> param in command.Value.Parameters)
                    {
                        ModuleCommandParameter? moduleCommandParam = new ModuleCommandParameter(moduleCommand, param.Value.Name, param.Value.ParameterType.Name);
                        moduleCommand.Parameters.Add(moduleCommandParam);
                    }
                }
                catch
                {
                    Console.WriteLine("Param Error");
                }


                moduleCommandList.Add(moduleCommand);
            }
            return moduleCommandList;
        }
    }
}
