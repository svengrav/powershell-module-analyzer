﻿using PSModuleAnalyzer.Core.Model;
using PSModuleAnalyzer.Core.Repository;
using System.Management.Automation.Language;

namespace PSModuleAnalyzer.Core.Visitor
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
