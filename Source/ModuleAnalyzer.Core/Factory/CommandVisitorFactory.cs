using PsModuleAnalyzer.Core.Model;
using PsModuleAnalyzer.Core.Visitor;

namespace PsModuleAnalyzer.Core.Factory
{
    public abstract class CommandVisitorFactory
    {
        public abstract CommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository);
    }
}
