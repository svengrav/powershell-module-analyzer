using Microsoft.Extensions.Logging;
using ModuleAnalyzer.Core.Exceptions;
using ModuleAnalyzer.Core.Runner;
using System.Management.Automation;
using System.Text.RegularExpressions;
using MathNet.Numerics.Statistics;

namespace ModuleAnalyzer.Core.Model;

public class ModuleDefinition
{
    private readonly string[] DEFAULT_PARAMETERS = new[]
    {
        "Debug", "Verbose", "ErrorAction", "WarningAction", "InformationAction", "WarningVariable", "ErrorVariable", "OutVariable", "InformationVariable" ,"OutBuffer","PipelineVariable","WhatIf"
    };

    private readonly PSModuleInfo _psModuleInfo;
    private readonly ILogger? _logger;

    public readonly string Name;
    public readonly string Path;

    public readonly HashSet<ModuleCommand> ModuleCommands = new();
    public readonly HashSet<ModuleNamespace> ModuleNamespaces = new();
    public readonly HashSet<ModuleCommandCall> ModuleCommandCalls = new();
    public readonly HashSet<ModuleCommandParameter> ModuleCommandParameters = new();
    public readonly HashSet<ModuleExternalCommand> ModuleExternalCommand = new();

    public double LineOfCodeMedian => CalculateLineOfCodeMedian();
    public double CommandParameterMedian => CalculateCommandParameterMedian();


    private double CalculateLineOfCodeMedian()
        => ModuleCommands.Select(command => (double) command.LinesOfCode).Median();

    private double CalculateCommandParameterMedian()
       => ModuleCommands.Select(command => (double) command.Parameters.Count).Median();

    public ModuleDefinition(string modulePath, ILogger? logger = null)
    {
        _logger = logger;
        _psModuleInfo = LoadModuleFromPath(modulePath);

        Name = _psModuleInfo.Name;
        Path = _psModuleInfo.ModuleBase;

        ModuleCommands = CreateModuleCommandList(_psModuleInfo);
    }

    public ModuleCommand? GetModuleCommand(string name) 
        => ModuleCommands.FirstOrDefault(command => command.Name == name);

    public ModuleExternalCommand? GetModuleExternalCommand(string name) 
        => ModuleExternalCommand.FirstOrDefault(command => command.Name == name);

    public ModuleNamespace AddModuleNamespace(ModuleCommand command)
    {
        var namespaceString = ExtractNamespaceString(command);

        var moduleNamespace = ModuleNamespaces.FirstOrDefault(moduleNamespace => moduleNamespace.Id == namespaceString);
        if (moduleNamespace is null)
            moduleNamespace = CreateModuleNamespace(namespaceString);

        command.Namespace = moduleNamespace;

        return moduleNamespace;
    }

    public HashSet<ModuleCommand> AddModuleCommands(HashSet<ModuleCommand> moduleCommands)
    {
        _logger?.LogDebug("{object}: Module {count} commands added", nameof(ModuleDefinition), moduleCommands.Count);

        foreach (var command in moduleCommands)
        {
            ModuleCommands.Add(command);
        }
        return ModuleCommands;
    }

    public ModuleExternalCommand AddExternalModuleCommand(string name)
    {
        var moduleCommand = GetModuleExternalCommand(name);
        if (moduleCommand is null)
        {
            moduleCommand = new ModuleExternalCommand(name);
            ModuleExternalCommand.Add(moduleCommand);
        }

        return moduleCommand;
    }

    public ModuleCommandCall AddModuleCommandCall(ModuleCommandCall moduleCommand)
    {
        ModuleCommandCalls.Add(moduleCommand);
        return moduleCommand;
    }

    public ModuleCommandParameter? GetModuleCommandParameter(string commandName, string parameterName)
    {
        var moduleCommand = GetModuleCommand(commandName);
        if (moduleCommand != null)
            return moduleCommand.Parameters.FirstOrDefault(param => param.Name == parameterName);

        var externalCommand = GetModuleExternalCommand(commandName);
        if (externalCommand != null)
            return externalCommand.Parameters.FirstOrDefault(param => param.Name == parameterName);

        return null;
    }

    public ModuleExternalCommand AddModuleExternalCommandCall(string name, IModuleCommand calledBy)
    {
        var moduleCommand = GetModuleExternalCommand(name);
        if (moduleCommand is null)
        {
            moduleCommand = new ModuleExternalCommand(name);
            ModuleExternalCommand.Add(moduleCommand);
        }

        ModuleCommandCalls.Add(new ModuleCommandCall(calledBy, moduleCommand));
        return moduleCommand;
    }

    private string ExtractNamespaceString(ModuleCommand command) => Regex.Match(command.CommandFile, @$"(?<={Regex.Escape(Path)}).*(?=\\.+\.ps1)").Value.Trim('\\');

    private PSModuleInfo LoadModuleFromPath(string modulePath)
    {
        _logger?.LogDebug("{object}: Try to create powershell and import module {modulePath}", nameof(ModuleDefinition), modulePath);
        var powerShell = PowerShellRunner.CreatePowerShell(_logger);

        powerShell.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted; Get-ExecutionPolicy;");
        powerShell.AddScript($"Import-Module {modulePath} -PassThru");

        var moduleInfo = powerShell.Invoke<PSModuleInfo>();

        if (moduleInfo.FirstOrDefault() is null)
            throw new ModuleInvalidException();

        return moduleInfo.First();
    }

    private ModuleNamespace CreateModuleNamespace(string namespaceString)
    {
        var moduleNamespace = new ModuleNamespace(namespaceString);
        ModuleNamespaces.Add(moduleNamespace);
        _logger?.LogTrace("{object}: Namespace {name} added", nameof(ModuleDefinition), namespaceString);
        return moduleNamespace;
    }

    private HashSet<ModuleCommand> CreateModuleCommandList(PSModuleInfo moduleInfo)
    {
        var moduleCommandList = new HashSet<ModuleCommand>();
        foreach (var function in moduleInfo.ExportedFunctions)
        {
            var command = function.Value;
            var moduleCommand = new ModuleCommand(command.Name, command.Definition, command.ScriptBlock.File) 
            {
                LinesOfCode = CalculateLinesOfCode(command)
            };

            AddModuleNamespace(moduleCommand);
            CreateModuleCommandParameterList(command, moduleCommand);

            moduleCommandList.Add(moduleCommand);
        }
        return moduleCommandList;
    }

    private static int CalculateLinesOfCode(FunctionInfo command) 
        => (command.ScriptBlock.StartPosition.EndLine - command.ScriptBlock.StartPosition.StartLine);

    private void CreateModuleCommandParameterList(FunctionInfo command, ModuleCommand moduleCommand)
    {
        try
        {
            foreach (var param in command.Parameters)
            {
                if (DEFAULT_PARAMETERS.Contains(param.Key))
                    continue;

                var moduleCommandParameter = new ModuleCommandParameter(moduleCommand, param.Value.Name, param.Value.ParameterType.Name);
                moduleCommand.Parameters.Add(moduleCommandParameter);
                ModuleCommandParameters.Add(moduleCommandParameter);

                _logger?.LogTrace("{object}: Parameter {parameterName} added to {commandName}",
                    nameof(ModuleDefinition), param.Value.Name, command.Name);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogDebug("{object}: Exception while trying to get command parameter details [{message}]", nameof(ModuleDefinition), ex.Message);
        }
    }
}
