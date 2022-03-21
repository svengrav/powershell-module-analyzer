using ModuleAnalyzer.App.Implementation.Visitor;
using ModuleAnalyzer.Core.Factory;
using ModuleAnalyzer.Core.Model;
using ModuleAnalyzer.Core.Repository;

namespace ModuleAnalyzer.App.Implementation.Factory
{
    internal class DefaultCommandVisitorFactory : CommandVisitorFactory
    {
        public override DefaultCommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository)
        {
            return new DefaultCommandVisitor(moduleCommand, moduleRepository);
        }
    }
}
