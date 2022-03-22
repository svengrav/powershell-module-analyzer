using System.Management.Automation;

namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleDefinition
    {
        private readonly PSModuleInfo _psModuleInfo;

        public readonly string Name;
        public readonly string Path;
        public readonly HashSet<ModuleCommand> ModuleCommands;
        public readonly List<ModuleCommandCall> ModuleCommandCalls = new();
        public readonly List<ModuleCommandParameter> ModuleCommandParameters = new();
        public readonly List<ModuleExternalCommand> ModuleExternalCommand = new();

        public ModuleDefinition(string modulePath)
        {
            _psModuleInfo = LoadModuleFromPath(modulePath);

            Name = _psModuleInfo.Name;
            Path = _psModuleInfo.ModuleBase;

            ModuleCommands = CreateModuleCommandList(_psModuleInfo);
        }

        public ModuleCommand? GetModuleCommand(string name)
        {
            return ModuleCommands.FirstOrDefault(command => command.Name == name);
        }

        public ModuleExternalCommand? GetModuleExternalCommand(string name)
        {
            return ModuleExternalCommand.FirstOrDefault(command => command.Name == name);
        }

        public HashSet<ModuleCommand> AddModuleCommands(HashSet<ModuleCommand> moduleCommands)
        {
            foreach (var command in moduleCommands)
            {
                ModuleCommands.Add(command);
            }
            return ModuleCommands;
        }

        public ModuleExternalCommand AddExternalModuleCommand(string name)
        {
            ModuleExternalCommand? moduleCommand = GetModuleExternalCommand(name);
            if (moduleCommand == null)
            {
                moduleCommand = new ModuleExternalCommand(name);
                ModuleExternalCommand.Add(moduleCommand);
            }

            return moduleCommand;
        }

        public ModuleCommandCall AddModuleCommandCall(ModuleCommandCall moduleCommand)
        {
            ModuleCommandCalls.Add(moduleCommand);

            RefreshTargetCommandReferences(moduleCommand.Target);
            return moduleCommand;
        }

        public ModuleCommandParameter? GetModuleCommandParameter(string commandName, string parameterName)
        {
            var moduleCommand = GetModuleCommand(commandName);
            if(moduleCommand != null)
            {
                return moduleCommand.Parameters.FirstOrDefault(param => param.Name == parameterName);
            } 
            
            var externalCommand = GetModuleExternalCommand(commandName);
            if(externalCommand != null)
            {
                return externalCommand.Parameters.FirstOrDefault(param => param.Name == parameterName);
            }

            return null;
        }

        public ModuleExternalCommand AddModuleExternalCommandCall(string name, IModuleCommand calledBy)
        {
            ModuleExternalCommand? moduleCommand = GetModuleExternalCommand(name);
            if (moduleCommand == null)
            {
                moduleCommand = new ModuleExternalCommand(name);
                ModuleExternalCommand.Add(moduleCommand);
            }

            ModuleCommandCalls.Add(new ModuleCommandCall(calledBy, moduleCommand, this));
            return moduleCommand;
        }

        private void RefreshTargetCommandReferences(IModuleCommand moduleCommand)
        {
            moduleCommand.NumberOfReferencedBy = ModuleCommandCalls.Count(c => c.Target.Name == moduleCommand.Name);
        }

        private static PSModuleInfo LoadModuleFromPath(string modulePath)
        {
            PowerShell? ps = PowerShell.Create();
            ps.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted; Get-ExecutionPolicy");
            ps.Commands.AddScript($"Import-Module {modulePath} -PassThru");
            return ps.Invoke<PSModuleInfo>().First();
        }

        private HashSet<ModuleCommand> CreateModuleCommandList(PSModuleInfo moduleInfo)
        {
            var defaultParams = new List<string>
            {
                "Debug", "Verbose", "ErrorAction", "WarningAction", "InformationAction", "WarningVariable", "ErrorVariable", "OutVariable", "InformationVariable" ,"OutBuffer","PipelineVariable","WhatIf"
            };

            HashSet<ModuleCommand>? moduleCommandList = new ();
            foreach (KeyValuePair<string, FunctionInfo> function in moduleInfo.ExportedFunctions)
            {
                var command = function.Value;
                ModuleCommand? moduleCommand = new(command.Name, command.Definition, command.ScriptBlock.File, this);
                try
                {
                    foreach (KeyValuePair<string, ParameterMetadata> param in command.Parameters)
                    {
                        if (defaultParams.Contains(param.Key))
                            continue;

                        ModuleCommandParameter? moduleCommandParam = new(moduleCommand, param.Value.Name, param.Value.ParameterType.Name);
                        moduleCommand.Parameters.Add(moduleCommandParam);
                    }
                }
                catch
                {
                    Console.WriteLine("Param Error");
                }


                moduleCommandList.Add(moduleCommand);
            }
            return moduleCommandList;
        }

    }
}
