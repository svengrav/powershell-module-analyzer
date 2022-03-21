using PSModuleAnalyzer.Core.Model;
using PSModuleAnalyzer.Core.Repository;
using PSModuleAnalyzer.Core.Visitor;

namespace PSModuleAnalyzer.Core.Factory
{
    public abstract class CommandVisitorFactory
    {
        public abstract CommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository);
    }
}
