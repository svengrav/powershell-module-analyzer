using ModuleAnalyzer.Core.Model;
using ModuleAnalyzer.Core.Repository;
using ModuleAnalyzer.Core.Visitor;

namespace ModuleAnalyzer.Core.Factory
{
    public abstract class CommandVisitorFactory
    {
        public abstract CommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository);
    }
}
