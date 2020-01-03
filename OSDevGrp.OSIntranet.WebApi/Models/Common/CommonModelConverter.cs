using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    internal class CommonModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<ILetterHead, LetterHeadIdentificationModel>();

            mapperConfiguration.CreateMap<ILetterHead, LetterHeadModel>();
        }

        #endregion
    }
}
