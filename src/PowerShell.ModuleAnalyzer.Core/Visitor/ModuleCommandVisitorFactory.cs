using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Core.Visitor;

public abstract class ModuleCommandVisitorFactory
{
    public abstract ModuleCommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository);
}
