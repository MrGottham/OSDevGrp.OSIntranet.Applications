using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers.CreateGenericCategoryCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands.ICreateGenericCategoryCommand<OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>, OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.CommandHandlers.CreateGenericCategoryCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
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
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateGenericCategoryCommand<IGenericCategory>> createGenericCategoryCommandMock = CreateCreateGenericCategoryCommandMock();
            await sut.ExecuteAsync(createGenericCategoryCommandMock.Object);

            createGenericCategoryCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.IsNotNull<Func<int, Task<IGenericCategory>>>()),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateGenericCategoryCommand<IGenericCategory>> createGenericCategoryCommandMock = CreateCreateGenericCategoryCommandMock();
            await sut.ExecuteAsync(createGenericCategoryCommandMock.Object);

            createGenericCategoryCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnCreateGenericCategoryCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(CreateCreateGenericCategoryCommand());

            Assert.That(((MyCreateGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnCreateGenericCategoryCommandHandlerBaseWithSameGenericCategoryFromCreateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            IGenericCategory genericCategory = new Mock<IGenericCategory>().Object;
            await sut.ExecuteAsync(CreateCreateGenericCategoryCommand(genericCategory));

            Assert.That(((MyCreateGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalledWithGenericCategory, Is.SameAs(genericCategory));
        }

        private CommandHandler CreateSut()
        {
            return new MyCreateGenericCategoryCommandHandler(_validatorMock.Object);
        }

        private ICreateGenericCategoryCommand<IGenericCategory> CreateCreateGenericCategoryCommand(IGenericCategory genericCategory = null)
        {
            return CreateCreateGenericCategoryCommandMock(genericCategory).Object;
        }

        private Mock<ICreateGenericCategoryCommand<IGenericCategory>> CreateCreateGenericCategoryCommandMock(IGenericCategory genericCategory = null)
        {
            Mock<ICreateGenericCategoryCommand<IGenericCategory>> createGenericCategoryCommandMock = new Mock<ICreateGenericCategoryCommand<IGenericCategory>>();
            createGenericCategoryCommandMock.Setup(m => m.ToDomain())
                .Returns(genericCategory ?? new Mock<IGenericCategory>().Object);
            return createGenericCategoryCommandMock;
        }

        private class MyCreateGenericCategoryCommandHandler : CommandHandler
        {
            #region Constructor

            public MyCreateGenericCategoryCommandHandler(IValidator validator)
                : base(validator)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public IGenericCategory ManageRepositoryAsyncWasCalledWithGenericCategory { get; private set; }

            #endregion

            #region Methods

            protected override Task<IGenericCategory> GetGenericCategoryAsync(int number) => throw new NotSupportedException();

            protected override Task ManageRepositoryAsync(IGenericCategory genericCategory)
            {
                NullGuard.NotNull(genericCategory, nameof(genericCategory));

                ManageRepositoryAsyncWasCalled = true;
                ManageRepositoryAsyncWasCalledWithGenericCategory = genericCategory;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}