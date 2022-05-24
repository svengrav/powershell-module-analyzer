using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Core.Visitor;

public class ModuleDefaultCommandVisitorFactory : ModuleCommandVisitorFactory
{
    public override ModuleDefaultCommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository)
    {
        return new ModuleDefaultCommandVisitor(moduleCommand, moduleRepository);
    }
}
