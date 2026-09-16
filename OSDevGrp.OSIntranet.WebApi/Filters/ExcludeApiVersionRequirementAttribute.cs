using System;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    /// <summary>
    /// Marks an action method where the api-version header should be excluded from the Swagger contract.
    /// Use this on operations that don't require the api-version header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    internal class ExcludeApiVersionRequirementAttribute : Attribute
    {
    }
}