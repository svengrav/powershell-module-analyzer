using PsModuleAnalyzer.App.Visitor;
using PsModuleAnalyzer.Core.Factory;
using PsModuleAnalyzer.Core.Model;

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
