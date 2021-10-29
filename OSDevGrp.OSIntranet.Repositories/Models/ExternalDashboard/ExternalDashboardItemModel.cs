using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.ExternalData;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;

namespace OSDevGrp.OSIntranet.Repositories.Models.ExternalDashboard
{
    [DataContract]
    internal class ExternalDashboardItemModel
    {
        #region Properties

        [DataMember(Name = "identifier", IsRequired = true, EmitDefaultValue = true)]
        public string Identifier { get; set; }

        [DataMember(Name = "timestamp", IsRequired = true, EmitDefaultValue = true)]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "information", IsRequired = true, EmitDefaultValue = true)]
        public string Information { get; set; }

        [DataMember(Name = "details", IsRequired = false, EmitDefaultValue = false)]
        public string Details { get; set; }

        [DataMember(Name = "provider", IsRequired = false, EmitDefaultValue = false)]
        public string Provider { get; set; }

        [DataMember(Name = "author", IsRequired = false, EmitDefaultValue = false)]
        public string Author { get; set; }

        [DataMember(Name = "sourceUrl", IsRequired = false, EmitDefaultValue = false)]
        public string SourceUrl { get; set; }

        [DataMember(Name = "imageUrl", IsRequired = false, EmitDefaultValue = false)]
        public string ImageUrl { get; set; }

        #endregion
    }

    internal static class ExternalDashboardItemModelExtensions
    {
        internal static INews ToDomain(this ExternalDashboardItemModel externalDashboardItemModel, IConverter externalDashboardConverter)
        {
            NullGuard.NotNull(externalDashboardItemModel, nameof(externalDashboardItemModel))
                .NotNull(externalDashboardConverter, nameof(externalDashboardConverter));

            return new News(
                externalDashboardItemModel.Identifier,
                externalDashboardItemModel.Timestamp.ToLocalTime(),
                externalDashboardItemModel.Information,
                externalDashboardItemModel.Details,
                externalDashboardItemModel.Provider,
                externalDashboardItemModel.Author,
                string.IsNullOrWhiteSpace(externalDashboardItemModel.SourceUrl) ? null : externalDashboardConverter.Convert<string, Uri>(externalDashboardItemModel.SourceUrl),
                string.IsNullOrWhiteSpace(externalDashboardItemModel.ImageUrl) ? null : externalDashboardConverter.Convert<string, Uri>(externalDashboardItemModel.ImageUrl));
        }
    }
}