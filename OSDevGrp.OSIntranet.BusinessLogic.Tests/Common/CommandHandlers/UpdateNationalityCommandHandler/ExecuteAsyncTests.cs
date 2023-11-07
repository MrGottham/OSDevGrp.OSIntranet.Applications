using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.UpdateNationalityCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IUpdateNationalityCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateNationalityCommand()
		{
			ICommandHandler<IUpdateNationalityCommand> sut = CreateSut();

			Mock<IUpdateNationalityCommand> updateNationalityCommandMock = BuildUpdateNationalityCommandMock();
			await sut.ExecuteAsync(updateNationalityCommandMock.Object);

			updateNationalityCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<INationality>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateNationalityCommand()
		{
			ICommandHandler<IUpdateNationalityCommand> sut = CreateSut();

			Mock<IUpdateNationalityCommand> updateNationalityCommandMock = BuildUpdateNationalityCommandMock();
			await sut.ExecuteAsync(updateNationalityCommandMock.Object);

			updateNationalityCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateNationalityAsyncWasCalledOnCommonRepositoryWithNationalityFromUpdateNationalityCommand()
		{
			ICommandHandler<IUpdateNationalityCommand> sut = CreateSut();

			INationality nationality = _fixture.BuildNationalityMock().Object;
			await sut.ExecuteAsync(BuildUpdateNationalityCommand(nationality));

			_commonRepositoryMock.Verify(m => m.UpdateNationalityAsync(It.Is<INationality>(value => value != null && value == nationality)), Times.Once());
		}

		private ICommandHandler<IUpdateNationalityCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.UpdateNationalityAsync(It.IsAny<INationality>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.UpdateNationalityCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateNationalityCommand BuildUpdateNationalityCommand(INationality nationality = null)
		{
			return BuildUpdateNationalityCommandMock(nationality).Object;
		}

		private Mock<IUpdateNationalityCommand> BuildUpdateNationalityCommandMock(INationality nationality = null)
		{
			Mock<IUpdateNationalityCommand> updateNationalityCommandMock = new Mock<IUpdateNationalityCommand>();
			updateNationalityCommandMock.Setup(m => m.ToDomain())
				.Returns(nationality ?? _fixture.BuildNationalityMock().Object);
			return updateNationalityCommandMock;
		}
	}
}