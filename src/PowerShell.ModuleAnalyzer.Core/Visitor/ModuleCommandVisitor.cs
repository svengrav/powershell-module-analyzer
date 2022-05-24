using ModuleAnalyzer.Core.Model;
using System.Management.Automation.Language;

namespace ModuleAnalyzer.Core.Visitor;

public abstract class ModuleCommandVisitor : AstVisitor2
{
    protected readonly ModuleDefinition _moduleRepository;
    protected readonly IModuleCommand _moduleCommand;

    public ModuleCommandVisitor(IModuleCommand moduleCommand, ModuleDefinition moduleRepository)
    {
        _moduleRepository = moduleRepository;
        _moduleCommand = moduleCommand;
    }
}
