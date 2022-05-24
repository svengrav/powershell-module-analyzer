using ModuleAnalyzer.Core.Model;
using System.Management.Automation.Language;

namespace ModuleAnalyzer.Core.Visitor;

public class ModuleDefaultCommandVisitor : ModuleCommandVisitor
{
    private ModuleCommandCall? _moduleCommandCall;
    private IModuleCommand? _moduleTargetCommand;

    public ModuleDefaultCommandVisitor(IModuleCommand moduleCommand, ModuleDefinition moduleDefinition) :
        base(moduleCommand, moduleDefinition)
    {

    }

    public override AstVisitAction VisitCommand(CommandAst commandAst)
    {
        _moduleTargetCommand = RegisterModuleCommand(commandAst);

        _moduleCommandCall = CreateModuleCommandCall(_moduleTargetCommand);

        _moduleRepository.AddModuleCommandCall(_moduleCommandCall);

        return AstVisitAction.Continue;
    }

    public override AstVisitAction VisitCommandParameter(CommandParameterAst parameterAst)
    {
        RegisterModuleCommandCallParameter(parameterAst.ParameterName);

        return AstVisitAction.Continue;
    }

    private void RegisterModuleCommandCallParameter(string parameterName)
    {
        if(_moduleCommandCall is not null)
        {
            var moduleCommandParameter = _moduleRepository.GetModuleCommandParameter(_moduleCommandCall.Target.Name, parameterName);

            if (moduleCommandParameter is not null)
                _moduleCommandCall.Parameters.Add(moduleCommandParameter);
        }
    }

    private IModuleCommand RegisterModuleCommand(CommandAst commandAst)
    {
        var targetCommandName = commandAst.GetCommandName();

        IModuleCommand? targetCommand = _moduleRepository.GetModuleCommand(targetCommandName);

        if (targetCommand is null)
            targetCommand = _moduleRepository.AddExternalModuleCommand(targetCommandName);

        return targetCommand;
    }

    private ModuleCommandCall CreateModuleCommandCall(IModuleCommand targetCommand)
        => new ModuleCommandCall(_moduleCommand, targetCommand);

}

