using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.ClientApi;

internal partial class WebApiClient
{
    #region Methods

    static partial void UpdateJsonSerializerSettings(JsonSerializerOptions settings)
    {
        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }

    #endregion
}