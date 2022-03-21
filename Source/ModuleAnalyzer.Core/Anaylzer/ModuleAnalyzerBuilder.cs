using PsModuleAnalyzer.Core.Factory;
using PsModuleAnalyzer.Core.Interfaces;

namespace PsModuleAnalyzer.Core.Anaylzer
{
    public class ModuleAnalyzerBuilder
    {
        private readonly string _modulePath;
        private readonly List<IAnalyzerOutput> _analyzerOutputs = new();
        private CommandVisitorFactory _factory;

        private ModuleAnalyzerBuilder(string modulePath)
        {
            _modulePath = modulePath;
        }

        public static ModuleAnalyzerBuilder Create(string modulePath)
        {
            return new ModuleAnalyzerBuilder(modulePath);
        }

        public ModuleAnalyzerBuilder AddOutputWriter(IAnalyzerOutput output)
        {
            _analyzerOutputs.Add(output);
            return this;
        }

        public ModuleAnalyzerBuilder AddCommandVisitor<CommandVisitorFactoryClass>() where CommandVisitorFactoryClass : CommandVisitorFactory, new()
        {
            _factory = new CommandVisitorFactoryClass();
            return this;
        }

        public ModuleAnalyzer Build()
        {
            ModuleAnalyzer? analyzer = new ModuleAnalyzer(_modulePath);
            analyzer.SetFactory(_factory);

            _analyzerOutputs.ForEach(output => analyzer.AddOutputFormat(output));
            return analyzer;
        }
    }
}
