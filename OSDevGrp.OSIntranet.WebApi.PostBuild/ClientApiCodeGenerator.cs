using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using System.Text;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

internal class ClientApiCodeGenerator
{
    #region Methods

    public async Task GenerateAsync(PostBuildExecutorContext context)
    {
        CompilationUnitSyntax sourceCode = await GenerateSourceCodeAsync(await ConvertAsync(context.OpenApiDocument), context.GeneratedCodeNamespace, context.GeneratedCodeClassName, context.CancellationToken);

        FileInfo sourceFileInfo = new FileInfo($"{context.SolutionDirectory.FullName}{Path.DirectorySeparatorChar}{context.GeneratedCodeNamespace}{Path.DirectorySeparatorChar}{context.GeneratedCodeClassName}.generated.cs");
        await WriteAsync(sourceFileInfo, sourceCode.ToFullString(), context.NotifyAsync);
    }

    private async Task<CompilationUnitSyntax> GenerateSourceCodeAsync(OpenApiDocument openApiDocument, string @namespace, string className, CancellationToken cancellationToken)
    {
        CSharpClientGeneratorSettings settings = CreateSettings(@namespace, className);
        CSharpClientGenerator generator = new CSharpClientGenerator(openApiDocument, settings);

        return await ModifyAsync(generator.GenerateFile(), cancellationToken);
    }

    private static async Task<OpenApiDocument> ConvertAsync(Microsoft.OpenApi.Models.OpenApiDocument openApiDocument)
    {
        using StringWriter jsonWriter = new StringWriter();

        Microsoft.OpenApi.Writers.OpenApiJsonWriter openApiJsonWriter = new Microsoft.OpenApi.Writers.OpenApiJsonWriter(jsonWriter);
        openApiDocument.SerializeAsV3(openApiJsonWriter);

        return await OpenApiDocument.FromJsonAsync(jsonWriter.ToString());
    }

    private static Task<CompilationUnitSyntax> ModifyAsync(string generatedCode, CancellationToken cancellationToken)
    {
        return Task.Run(() => 
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(generatedCode, cancellationToken: cancellationToken);
            CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot(cancellationToken: cancellationToken);

            return compilationUnitSyntax;
        });
    }

    private static async Task WriteAsync(FileInfo sourceFileInfo, string sourceCode, Func<string, Task> notifier)
    {
        sourceCode = NormalizeLineEndings(sourceCode);

        string? existingSourceCode = null;
        sourceFileInfo.Refresh();
        if (sourceFileInfo.Exists)
        {
            existingSourceCode = NormalizeLineEndings(await File.ReadAllTextAsync(sourceFileInfo.FullName, Encoding.UTF8));
        }

        if (sourceCode == existingSourceCode)
        {
            return;
        }

        await notifier($"Writing {sourceFileInfo.Name} in {sourceFileInfo.Directory!.FullName}");
        await File.WriteAllTextAsync(sourceFileInfo.FullName, sourceCode, Encoding.UTF8);
    }

    private static CSharpClientGeneratorSettings CreateSettings(string @namespace, string className)
    {
        CSharpClientGeneratorSettings settings = new CSharpClientGeneratorSettings
        {
            ClassName = className,
            GenerateClientInterfaces = true,
            GenerateExceptionClasses = true,
            ExceptionClass = $"{className}Exception",
            InjectHttpClient = true,
            UseBaseUrl = false,
            ClientClassAccessModifier = "internal",
            ParameterDateFormat = typeof(DateTimeOffset).Name,
            QueryNullValue = "null"
        };
        settings.CSharpGeneratorSettings.Namespace = @namespace;
        settings.CSharpGeneratorSettings.ClassStyle = CSharpClassStyle.Record;
        settings.CSharpGeneratorSettings.DateType = typeof(DateTimeOffset).Name;
        settings.CSharpGeneratorSettings.DateTimeType = typeof(DateTimeOffset).Name;
        settings.CSharpGeneratorSettings.TimeType = typeof(TimeSpan).Name;
        settings.CSharpGeneratorSettings.TimeSpanType = typeof(TimeSpan).Name;
        settings.CSharpGeneratorSettings.NumberType = typeof(decimal).Name;
        settings.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.SystemTextJson;
        settings.CSharpGeneratorSettings.GenerateOptionalPropertiesAsNullable = true;
        settings.CSharpGeneratorSettings.GenerateNullableReferenceTypes = true;
        return settings;
    }

    public static string NormalizeLineEndings(string value)
    {
        return string.Join(Environment.NewLine, value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
    }

    #endregion
}