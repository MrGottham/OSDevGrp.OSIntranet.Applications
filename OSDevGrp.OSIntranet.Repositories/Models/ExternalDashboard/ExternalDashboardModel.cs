using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;

namespace OSDevGrp.OSIntranet.Repositories.Models.ExternalDashboard
{
    [DataContract]
    internal class ExternalDashboardModel
    {
        #region Properties

        [DataMember(Name = "items", IsRequired = true, EmitDefaultValue = true)]
        public ExternalDashboardItemModel[] Items { get; set; }

        #endregion
    }

    internal static class ExternalDashboardModelExtensions
    {
        internal static IEnumerable<INews> ToDomain(this ExternalDashboardModel externalDashboardModel, IConverter externalDashboardConverter)
        {
            NullGuard.NotNull(externalDashboardModel, nameof(externalDashboardModel))
                .NotNull(externalDashboardConverter, nameof(externalDashboardConverter));

            return (externalDashboardModel.Items ?? Array.Empty<ExternalDashboardItemModel>())
                .AsParallel()
                .Select(externalDashboardItemModel => externalDashboardConverter.Convert<ExternalDashboardItemModel, INews>(externalDashboardItemModel))
                .ToArray();
        }
    }
}