using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers.DeleteGenericCategoryCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands.IDeleteGenericCategoryCommand<OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>, OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.CommandHandlers.DeleteGenericCategoryCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("command"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteGenericCategoryCommand<IGenericCategory>> deleteGenericCategoryCommandMock = CreateDeleteGenericCategoryCommandMock();
            await sut.ExecuteAsync(deleteGenericCategoryCommandMock.Object);

            deleteGenericCategoryCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IGenericCategory>>>()),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteGenericCategoryCommand<IGenericCategory>> deleteGenericCategoryCommandMock = CreateDeleteGenericCategoryCommandMock();
            await sut.ExecuteAsync(deleteGenericCategoryCommandMock.Object);

            deleteGenericCategoryCommandMock.Verify(m => m.Number, Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnDeleteGenericCategoryCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(CreateDeleteGenericCategoryCommand());

            Assert.That(((MyDeleteGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnDeleteGenericCategoryCommandHandlerBaseWithNumberFromDeleteGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.ExecuteAsync(CreateDeleteGenericCategoryCommand(number));

            Assert.That(((MyDeleteGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalledWithNumber, Is.EqualTo(number));
        }

        private CommandHandler CreateSut()
        {
            return new MyDeleteGenericCategoryCommandHandler(_validatorMock.Object);
        }

        private IDeleteGenericCategoryCommand<IGenericCategory> CreateDeleteGenericCategoryCommand(int? number = null)
        {
            return CreateDeleteGenericCategoryCommandMock(number).Object;
        }

        private Mock<IDeleteGenericCategoryCommand<IGenericCategory>> CreateDeleteGenericCategoryCommandMock(int? number = null)
        {
            Mock<IDeleteGenericCategoryCommand<IGenericCategory>> deleteGenericCategoryCommandMock = new Mock<IDeleteGenericCategoryCommand<IGenericCategory>>();
            deleteGenericCategoryCommandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return deleteGenericCategoryCommandMock;
        }

        private class MyDeleteGenericCategoryCommandHandler : CommandHandler
        {
            #region Constructor

            public MyDeleteGenericCategoryCommandHandler(IValidator validator)
                : base(validator)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public int ManageRepositoryAsyncWasCalledWithNumber { get; private set; }

            #endregion

            #region Methods

            protected override Task<IGenericCategory> GetGenericCategoryAsync(int number) => throw new NotSupportedException();

            protected override Task ManageRepositoryAsync(int number)
            {
                ManageRepositoryAsyncWasCalled = true;
                ManageRepositoryAsyncWasCalledWithNumber = number;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}