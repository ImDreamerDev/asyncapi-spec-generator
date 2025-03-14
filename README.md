# asyncapi-spec-generator

This is a simple tool to generate an AsyncAPI specification from a C# project.

## Usage

```bash
dotnet build

dotnet run -- --projectPath "./AsyncApiSpecGenerator.Tests" --projectName Demo --projectType BC --version 1.0
```

Then it will generate a file named `{projectType}-{projectName}-{version}-asyncapi.yaml` and a file named `{projectType}-{projectName}-{version}-asyncapi-backstage.yaml`