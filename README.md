# AsyncAPI Spec Generator for C#

## Overview

AsyncAPI Spec Generator is a .NET tool that automatically generates **AsyncAPI specifications** and **Backstage scripts
** from C# code using the `AsyncEvent` attribute. This simplifies documentation and service discovery for event-driven
architectures.

## Installation

To install the tool globally, run:

```sh
dotnet tool install --global IAmDreamerDev.AsyncApiSpecGenerator
```

To use the `AsyncEvent` attribute in your project, install the corresponding NuGet package:

```sh
dotnet add package IAmDreamerDev.AsyncApiSpecGenerator.Attributes
```

## Usage

Run the generator with:

```sh
AsyncApiSpecGenerator --projectPath Example.ADPT.Project.Api --projectPath Another.Project.Api --projectName Project --projectType ADPT --version 1.0
```

### Available Options:

| Option          | Description                                                                              |
|-----------------|------------------------------------------------------------------------------------------|
| `--projectPath` | Path(s) to the project(s) containing event definitions. Can be specified multiple times. |
| `--projectName` | Name of the project.                                                                     |
| `--projectType` | Type/category of the project.                                                            |
| `--version`     | Version of the specification being generated.                                            |
| `--json`        | Outputs the AsyncAPI specification in JSON format (default is YAML).                     |
| `--env`         | Specifies the environment (e.g., dev, test, prod) (optional).                            |
| `--lifecycle`   | Defines the project lifecycle (e.g., dev, test, prod) (optional).                        |
| `--owner`       | Sets the project owner (e.g., team name) (optional).                                     |
| `--noValidate`  | Disables validation of the generated AsyncAPI specification.                             |
| `--help`        | Displays help information.                                                               |

🔧 Automatic Project Build
If the tool cannot find a compiled DLL for your project, it will automatically attempt to build the project before
generating the AsyncAPI specification and Backstage script.

## Defining an Async Event

Annotate your event classes with the `AsyncEvent` attribute:

```csharp
using IAmDreamerDev.AsyncApiSpecGenerator.Attributes;
using System;

[AsyncEvent("cpq-events", AsyncApiOperationType.Send)]
internal class EventDemo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly Date { get; set; }
    public int Number { get; set; }
}
```

### Attribute Parameters:

- First argument: **Event Channel Name** (e.g., `"cpq-events"`)
- Second argument: **Operation Type** (`Send`, `Receive`, etc.)

## Output

The tool generates:

- **AsyncAPI Specification** (`asyncapi.yaml` or `asyncapi.json` with `--json`)
- **Backstage Entity Script** for service catalog integration.

## Contributing

Contributions, bug reports, and feature requests are welcome! Feel free to submit an issue or pull request.

## License

This project is licensed under the MIT License.