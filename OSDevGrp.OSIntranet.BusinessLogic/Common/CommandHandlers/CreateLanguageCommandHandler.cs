using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
	internal class CreateLanguageCommandHandler : CreateGenericCategoryCommandHandlerBase<ICreateLanguageCommand, ILanguage>
	{
		#region Private variables

		private readonly ICommonRepository _commonRepository;

		#endregion

		#region Constructor

		public CreateLanguageCommandHandler(IValidator validator, ICommonRepository commonRepository)
			: base(validator)
		{
			NullGuard.NotNull(commonRepository, nameof(commonRepository));

			_commonRepository = commonRepository;
		}

		#endregion

		#region Methods

		protected override Task<ILanguage> GetGenericCategoryAsync(int number) => _commonRepository.GetLanguageAsync(number);

		protected override Task ManageRepositoryAsync(ILanguage language)
		{
			NullGuard.NotNull(language, nameof(language));

			return _commonRepository.CreateLanguageAsync(language);
		}

		#endregion
	}
}