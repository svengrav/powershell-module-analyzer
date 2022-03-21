using ModuleAnalyzer.Core.Model;
using ModuleAnalyzer.Core.Repository;
using ModuleAnalyzer.Core.Visitor;
using System.Management.Automation.Language;

namespace ModuleAnalyzer.App.Implementation.Visitor
{
    public class DefaultCommandVisitor : CommandVisitor
    {
        public DefaultCommandVisitor(ModuleCommand moduleCommand, ModuleDefinition moduleRepository) :
            base(moduleCommand, moduleRepository)
        {

        }

        public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
        {
            return AstVisitAction.SkipChildren;
        }

        public override AstVisitAction VisitCommand(CommandAst commandAst)
        {
            string? targetCommandName = commandAst.GetCommandName();

            IModuleCommand? targetCommand = _moduleRepository.GetModuleCommand(targetCommandName);

            if (targetCommand == null)
            {
                targetCommand = _moduleRepository.AddExternalModuleCommand(targetCommandName);
            }

            ModuleCommandCall? moduleCommandCall = new ModuleCommandCall(_moduleCommand, targetCommand);

            _moduleRepository.AddModuleCommandCall(moduleCommandCall);

            return AstVisitAction.SkipChildren;
        }
    }
}
