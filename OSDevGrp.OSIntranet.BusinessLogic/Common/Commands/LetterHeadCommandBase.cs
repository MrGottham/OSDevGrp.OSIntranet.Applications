using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public abstract class LetterHeadCommandBase : LetterHeadIdentificationCommandBase, ILetterHeadCommand
    {
        #region Properties

        public string Name { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string Line5 { get; set; }

        public string Line6 { get; set; }

        public string Line7 { get; set; }

        public string CompanyIdentificationNumber { get ; set; }
        
        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, commonRepository)
                .String.ShouldNotBeNullOrWhiteSpace(Name, GetType(), nameof(Name))
                .String.ShouldHaveMinLength(Name, 1, GetType(), nameof(Name))
                .String.ShouldHaveMaxLength(Name, 256, GetType(), nameof(Name))
                .String.ShouldNotBeNullOrWhiteSpace(Line1, GetType(), nameof(Line1))
                .String.ShouldHaveMinLength(Line1, 1, GetType(), nameof(Line1))
                .String.ShouldHaveMaxLength(Line1, 64, GetType(), nameof(Line1))
                .String.ShouldHaveMinLength(Line2, 1, GetType(), nameof(Line2), true)
                .String.ShouldHaveMaxLength(Line2, 64, GetType(), nameof(Line2), true)
                .String.ShouldHaveMinLength(Line3, 1, GetType(), nameof(Line3), true)
                .String.ShouldHaveMaxLength(Line3, 64, GetType(), nameof(Line3), true)
                .String.ShouldHaveMinLength(Line4, 1, GetType(), nameof(Line4), true)
                .String.ShouldHaveMaxLength(Line4, 64, GetType(), nameof(Line4), true)
                .String.ShouldHaveMinLength(Line5, 1, GetType(), nameof(Line5), true)
                .String.ShouldHaveMaxLength(Line5, 64, GetType(), nameof(Line5), true)
                .String.ShouldHaveMinLength(Line6, 1, GetType(), nameof(Line6), true)
                .String.ShouldHaveMaxLength(Line6, 64, GetType(), nameof(Line6), true)
                .String.ShouldHaveMinLength(Line7, 1, GetType(), nameof(Line7), true)
                .String.ShouldHaveMaxLength(Line7, 64, GetType(), nameof(Line7), true)
                .String.ShouldHaveMinLength(CompanyIdentificationNumber, 1, GetType(), nameof(CompanyIdentificationNumber), true)
                .String.ShouldHaveMaxLength(CompanyIdentificationNumber, 32, GetType(), nameof(CompanyIdentificationNumber), true);
        }

        public ILetterHead ToDomain()
        {
            return new LetterHead(Number, Name, Line1)
            {
                Line2 = Line2,
                Line3 = Line3,
                Line4 = Line4,
                Line5 = Line5,
                Line6 = Line6,
                Line7 = Line7,
                CompanyIdentificationNumber = CompanyIdentificationNumber
            };
        }

        #endregion
    }
}