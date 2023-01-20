using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetNationalityQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetNationalityQuery()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut();

            Mock<IGetNationalityQuery> getNationalityQueryMock = CreateGetNationalityQueryMock();
            await sut.QueryAsync(getNationalityQueryMock.Object);

            getNationalityQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetNationalityQuery()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut();

            Mock<IGetNationalityQuery> getNationalityQueryMock = CreateGetNationalityQueryMock();
            await sut.QueryAsync(getNationalityQueryMock.Object);

            getNationalityQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetNationalityAsyncWasCalledOnCommonRepository()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetNationalityQuery(number));

            _commonRepositoryMock.Verify(m => m.GetNationalityAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNationalityWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut();

            INationality result = await sut.QueryAsync(CreateGetNationalityQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNationalityWasReturnedFromCommonRepository_ReturnsNationalityFromCommonRepository()
        {
            INationality nationality = _fixture.BuildNationalityMock().Object;
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut(nationality: nationality);

            INationality result = await sut.QueryAsync(CreateGetNationalityQuery());

            Assert.That(result, Is.EqualTo(nationality));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoNationalityWasReturnedFromCommonRepository_ReturnsNull()
        {
            IQueryHandler<IGetNationalityQuery, INationality> sut = CreateSut(false);

            INationality result = await sut.QueryAsync(CreateGetNationalityQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetNationalityQuery, INationality> CreateSut(bool hasNationality = true, INationality nationality = null)
        {
            _commonRepositoryMock.Setup(m => m.GetNationalityAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasNationality? nationality ?? _fixture.BuildNationalityMock().Object : null));

            return new BusinessLogic.Common.QueryHandlers.GetNationalityQueryHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private IGetNationalityQuery CreateGetNationalityQuery(int? number = null)
        {
            return CreateGetNationalityQueryMock(number).Object;
        }

        private Mock<IGetNationalityQuery> CreateGetNationalityQueryMock(int? number = null)
        {
            Mock<IGetNationalityQuery> getNationalityQueryMock = new Mock<IGetNationalityQuery>();
            getNationalityQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getNationalityQueryMock;
        }
    }
}