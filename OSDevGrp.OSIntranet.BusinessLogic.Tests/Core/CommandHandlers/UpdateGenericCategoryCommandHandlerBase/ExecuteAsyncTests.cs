using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers.UpdateGenericCategoryCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands.IUpdateGenericCategoryCommand<OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>, OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.CommandHandlers.UpdateGenericCategoryCommandHandlerBase
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
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateGenericCategoryCommand<IGenericCategory>> updateGenericCategoryCommandMock = CreateUpdateGenericCategoryCommandMock();
            await sut.ExecuteAsync(updateGenericCategoryCommandMock.Object);

            updateGenericCategoryCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.IsNotNull<Func<int, Task<IGenericCategory>>>()),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateGenericCategoryCommand<IGenericCategory>> updateGenericCategoryCommandMock = CreateUpdateGenericCategoryCommandMock();
            await sut.ExecuteAsync(updateGenericCategoryCommandMock.Object);

            updateGenericCategoryCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnUpdateGenericCategoryCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(CreateUpdateGenericCategoryCommand());

            Assert.That(((MyUpdateGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnUpdateGenericCategoryCommandHandlerBaseWithSameGenericCategoryFromUpdateGenericCategoryCommand()
        {
            CommandHandler sut = CreateSut();

            IGenericCategory genericCategory = new Mock<IGenericCategory>().Object;
            await sut.ExecuteAsync(CreateUpdateGenericCategoryCommand(genericCategory));

            Assert.That(((MyUpdateGenericCategoryCommandHandler)sut).ManageRepositoryAsyncWasCalledWithGenericCategory, Is.SameAs(genericCategory));
        }

        private CommandHandler CreateSut()
        {
            return new MyUpdateGenericCategoryCommandHandler(_validatorMock.Object);
        }

        private IUpdateGenericCategoryCommand<IGenericCategory> CreateUpdateGenericCategoryCommand(IGenericCategory genericCategory = null)
        {
            return CreateUpdateGenericCategoryCommandMock(genericCategory).Object;
        }

        private Mock<IUpdateGenericCategoryCommand<IGenericCategory>> CreateUpdateGenericCategoryCommandMock(IGenericCategory genericCategory = null)
        {
            Mock<IUpdateGenericCategoryCommand<IGenericCategory>> updateGenericCategoryCommandMock = new Mock<IUpdateGenericCategoryCommand<IGenericCategory>>();
            updateGenericCategoryCommandMock.Setup(m => m.ToDomain())
                .Returns(genericCategory ?? new Mock<IGenericCategory>().Object);
            return updateGenericCategoryCommandMock;
        }

        private class MyUpdateGenericCategoryCommandHandler : CommandHandler
        {
            #region Constructor

            public MyUpdateGenericCategoryCommandHandler(IValidator validator)
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