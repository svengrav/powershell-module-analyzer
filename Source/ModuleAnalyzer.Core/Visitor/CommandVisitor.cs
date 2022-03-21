using ModuleAnalyzer.Core.Model;
using ModuleAnalyzer.Core.Repository;
using System.Management.Automation.Language;

namespace ModuleAnalyzer.Core.Visitor
{
    public abstract class CommandVisitor : AstVisitor2
    {
        protected readonly ModuleDefinition _moduleRepository;
        protected readonly ModuleCommand _moduleCommand;

        public CommandVisitor(ModuleCommand moduleCommand, ModuleDefinition moduleRepository)
        {
            _moduleRepository = moduleRepository;
            _moduleCommand = moduleCommand;
        }
    }
}
