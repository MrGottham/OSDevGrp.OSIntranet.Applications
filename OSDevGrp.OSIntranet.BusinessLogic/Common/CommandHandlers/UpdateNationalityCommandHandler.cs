using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
	internal class UpdateNationalityCommandHandler : UpdateGenericCategoryCommandHandlerBase<IUpdateNationalityCommand, INationality>
	{
		#region Private variables

		private readonly ICommonRepository _commonRepository;

		#endregion

		#region Constructor

		public UpdateNationalityCommandHandler(IValidator validator, ICommonRepository commonRepository)
			: base(validator)
		{
			NullGuard.NotNull(commonRepository, nameof(commonRepository));

			_commonRepository = commonRepository;
		}

		#endregion

		#region Methods

		protected override Task<INationality> GetGenericCategoryAsync(int number) => _commonRepository.GetNationalityAsync(number);

		protected override Task ManageRepositoryAsync(INationality nationality)
		{
			NullGuard.NotNull(nationality, nameof(nationality));

			return _commonRepository.UpdateNationalityAsync(nationality);
		}

		#endregion
	}
}