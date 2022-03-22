using PsModuleAnalyzer.Core.Model;
using PsModuleAnalyzer.Core.Visitor;
using System.Management.Automation.Language;

namespace PsModuleAnalyzer.App.Visitor
{
    public class DefaultCommandVisitor : CommandVisitor
    {
        public DefaultCommandVisitor(ModuleCommand moduleCommand, ModuleDefinition moduleDefinition) :
            base(moduleCommand, moduleDefinition)
        {

        }

        public override AstVisitAction VisitCommand(CommandAst commandAst)
        {
            string? targetCommandName = commandAst.GetCommandName();

            IModuleCommand? targetCommand = _moduleRepository.GetModuleCommand(targetCommandName);

            if (targetCommand == null)
            {
                targetCommand = _moduleRepository.AddExternalModuleCommand(targetCommandName);
            }

            ModuleCommandCall? moduleCommandCall = new ModuleCommandCall(_moduleCommand, targetCommand, _moduleRepository);

            _moduleRepository.AddModuleCommandCall(moduleCommandCall);

            return AstVisitAction.Continue;
        }

        public override AstVisitAction VisitCommandParameter(CommandParameterAst parameterAst)
        {
            var currentCall = _moduleRepository.ModuleCommandCalls.Last();
            var currentParam = _moduleRepository.GetModuleCommandParameter(currentCall.Target.Name, parameterAst.ParameterName);
            
            if(currentParam != null)
                currentCall.Parameters.Add(currentParam);

            return AstVisitAction.Continue;
        }

        public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst commandAst)
        {
            string? targetCommandName = commandAst.Name;

            IModuleCommand? targetCommand = _moduleRepository.GetModuleCommand(targetCommandName);

            if (targetCommand == null)
            {
                targetCommand = _moduleRepository.AddExternalModuleCommand(targetCommandName);
            }

            ModuleCommandCall? moduleCommandCall = new ModuleCommandCall(_moduleCommand, targetCommand, _moduleRepository);
            if(commandAst.Parameters != null)
            {
                foreach(var cmd in commandAst.Parameters)
                {
                    var cmdParam = new ModuleCommandParameter(_moduleCommand, cmd.Name.ToString(), cmd.StaticType.Name);
                    moduleCommandCall.Parameters.Add(cmdParam);
                }
            }

            _moduleRepository.AddModuleCommandCall(moduleCommandCall);

            return AstVisitAction.Continue;
        }
    }
}
