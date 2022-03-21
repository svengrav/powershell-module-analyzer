using PsModuleAnalyzer.Core.Interfaces;
using PsModuleAnalyzer.Core.Model;

namespace PsModuleAnalyzer.App.Output
{
    public class MermaidOutput : IAnalyzerOutput
    {
        private const string TEXT_INDENTION = "    ";
        private readonly string _destinationPath;

        public MermaidOutput(string destinationPath)
        {
            _destinationPath = destinationPath;
        }

        public void CreateAnalyzerOutupt(List<ModuleCommandCall> moduleCommandCalls)
        {
            var content = CreateMarkdownContent(moduleCommandCalls);
            WriteMarkdownFile(content);
        }

        private List<string> CreateMarkdownContent(List<ModuleCommandCall> moduleCommandCalls)
        {
            var markdownContent = new List<string>();
            markdownContent.Add("## Module Graph");
            markdownContent.Add("```mermaid");
            markdownContent.Add("graph TD;");

            moduleCommandCalls.Where(s => !s.Target.IsExternal).ToList().ForEach(command =>
            {
                markdownContent.Add(AddGraphConnection(command));
            });

            markdownContent.Add("```");
            return markdownContent;
        }

        private string AddGraphConnection(ModuleCommandCall commandCall)
        {
            return $"{TEXT_INDENTION}{commandCall.Source.Name}-->{commandCall.Target.Name}";
        }

        private void WriteMarkdownFile(List<string> markdownContent)
        {
            File.WriteAllLines(_destinationPath, markdownContent);
        }
    }
}
