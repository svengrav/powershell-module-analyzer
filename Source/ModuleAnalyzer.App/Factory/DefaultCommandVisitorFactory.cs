using PsModuleAnalyzer.App.Visitor;
using PsModuleAnalyzer.Core.Factory;
using PsModuleAnalyzer.Core.Model;
using PsModuleAnalyzer.Core.Repository;

namespace PsModuleAnalyzer.App.Factory
{
    internal class DefaultCommandVisitorFactory : CommandVisitorFactory
    {
        public override DefaultCommandVisitor Create(ModuleCommand moduleCommand, ModuleDefinition moduleRepository)
        {
            return new DefaultCommandVisitor(moduleCommand, moduleRepository);
        }
    }
}
