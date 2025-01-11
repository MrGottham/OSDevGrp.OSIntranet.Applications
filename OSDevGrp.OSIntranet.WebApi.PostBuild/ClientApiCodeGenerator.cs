using Microsoft.OpenApi.Writers;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

internal class ClientApiCodeGenerator
{
    #region Methods

    public async Task GenerateAsync(PostBuildExecutorContext context)
    {
        using (StringWriter stringWriter = new StringWriter())
        {
             OpenApiJsonWriter openApiJsonWriter = new OpenApiJsonWriter(stringWriter);
             context.OpenApiDocument.SerializeAsV3(openApiJsonWriter);
             await context.NotifyAsync(stringWriter.ToString());
        }
    }

    #endregion
}