# PowerShell Module Anaylzer

I developed this tool to analyze a PSModule I developed with my team for around 1 1/2 years. As the time went by, it become clear that the amount of technical debt has become very large and that we needed some way to analyze and identify critical hotspots in the tool.

## Description

The PowerShell analyzer is a little prototype which is made for analyzing PowerShell module commands, parameters and function calls with the purpose to make technical debt visible.

- The analyzer will list all module functions, how often they are referenced by and referencing other functions.
- In addition the parameters and how often they are used are also considered.
- It calculates the stability index. This index describes from 0 (low) to 1 (high) how many other functions are relying on a specific function.
  - A high stability means, that a change of the function comes with high cost.

---
## HowTo
### 
Build tool:
- dotnet build "./src/" -o "./output/" 

Start tool:
- & ".\output\*.exe" -p "D:\Repositories\Go\Source\Go.psd1" -o "."

### Startup Arguments
  
| Parameter | Parameter (long) | Required | Description                                                |
| --------- | ---------------- | -------- | ---------------------------------------------------------- |
| -p        | --path           | true     | Path to the PowerShell module                              |
| -d        | --debug          | true     | Enables debug logging                                      |
| -o        | --outputdir      | true     | Path to a directory for the output of the analysis results |
| -n        | --outputname     | true     | Identifier for the output                                  |


### Output

The analyzer comes with multiple output formatters and is able to write the results as CSV, JSON or as Directed Graph Markup Language (dgml).
The DGML Output helps you to visualize the module and shows the function dependencies as a graph.

#### CSV Compact

- All commands from the module and the basic stats are listed here.

| Column             | Description                                                                                                |
| ------------------ | ---------------------------------------------------------------------------------------------------------- |
| Name               | Name of the function.                                                                                      |
| Namespace          | The namespace is the directory structure.                                                                  |
| Invokes (Module)   | Number of your own module functions that are executed by this function.                                    |
| Invokes (External) | Number of external functions performed by this function. Such as "Get-Item".                               |
| InvokedBy          | Number of invocations by other module functions.                                                           |
| Invokes (Unique)   | Number of unique  functions executed by this function. This means that each function is counted only once. |
| InvokedBy (Unique) | Number of your own unique module functions executed by this function.                                      |
| StabilityIndex     | Number of unique module functions that call this function.                                                 |
| LinesOfCode        | Number of lines of code for this function.                                                                 |
| Parameters         | List of parameters and how often they are used.                                                            |

#### CSV Invokes

- Lists all invocations between the module functions.

| Column          | Description                                                             |
| --------------- | ----------------------------------------------------------------------- |
| Name            | Name of the function.                                                   |
| Namespace       | The namespace is the directory structure.                               |
| Target (Module) | Number of your own module functions that are executed by this function. |
| Parameters      | List of parameters that are used while executing the command.           |


#### DGML

- DGML is used to visualise all invocations between the module functions. This format is experimental and can be displayed e.g. with Visual Studio.
