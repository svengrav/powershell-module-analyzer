using Microsoft.Extensions.Logging;
using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;
using ModuleAnalyzer.Core.Visitor;
using System.Management.Automation.Language;

namespace ModuleAnalyzer.Core.Anaylzer;

/// <summary>
/// This class is responsible to load the target PowerShell module, 
/// execute the analyzation and to call the output formatters.
/// </summary>
public class ModuleAnalyzer
{
    private readonly List<IAnalyzerOutputFormatter> _analyzerOutputFormatters = new();
    private readonly ILogger? _logger;

    public ModuleCommandVisitorFactory? Factory { get; set; }
    public ModuleDefinition ModuleDefinition { get; private set; }

    internal ModuleAnalyzer(string modulePath, ILogger? logger = null)
    {
        ModuleDefinition = new ModuleDefinition(modulePath, logger);
        _logger = logger;
    }

    /// <summary>
    /// Add an output formatter to the analyzer. 
    /// Its possible to add multiple analyzer. 
    /// </summary>
    /// <param name="outputFormatter">Output formatter</param>
    public void AddOutputFormat(IAnalyzerOutputFormatter outputFormatter) 
        => _analyzerOutputFormatters.Add(outputFormatter);

    public void SetCommandVisitorFactory(ModuleCommandVisitorFactory factory) 
        => Factory = factory;

    /// <summary>
    /// Executes the analysis and traverses the script tree.
    /// The result will be stored in the <see cref="ModuleDefinition">Module Definition</see>.
    /// </summary>
    public void Analyze()
    {
        try
        {
            _logger?.LogInformation("Start to analyze module {name}", ModuleDefinition.Name);
            foreach (var command in ModuleDefinition.ModuleCommands)
            {
                var scriptBlockAst = Parser.ParseInput(command.Definition, out var tokens, out var errors);
                var scriptBlockAstCopy = (ScriptBlockAst) scriptBlockAst.Copy();

                var paramastList = CreateParameterListFromScriptBlock(scriptBlockAst);

                var functionDefinitionAst = new FunctionDefinitionAst(scriptBlockAstCopy.Extent, false, false, command.Name, paramastList, scriptBlockAstCopy);

                var commandVisitor = Factory.Create(command, ModuleDefinition);

                functionDefinitionAst.Visit(commandVisitor);
            }

            _logger?.LogInformation("Analysis finished. Write results");
            CreateOutput();

            _logger?.LogInformation("Done");
        }
        catch (Exception ex)
        {
            _logger?.LogCritical("-------------------------------------------------");
            _logger?.LogCritical(exception: ex, "Error during analysis.");
            _logger?.LogCritical("-------------------------------------------------");
        }

    }

    private static List<ParameterAst> CreateParameterListFromScriptBlock(ScriptBlockAst scriptBlockAst)
    {
        var paramastList = new List<ParameterAst>();
        if (scriptBlockAst.ParamBlock is not null)
            foreach (var param in scriptBlockAst.ParamBlock.Parameters)
                paramastList.Add((ParameterAst)param.Copy());

        return paramastList;
    }

    /// <summary>
    /// This function will loop through the output writers.
    /// </summary>
    private void CreateOutput() 
        => _analyzerOutputFormatters.ForEach(output => output.CreateAnalyzerOutupt(ModuleDefinition));
}
