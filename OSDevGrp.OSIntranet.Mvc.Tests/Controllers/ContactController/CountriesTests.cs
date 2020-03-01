using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CountriesTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Countries_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.Countries();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Countries_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Countries();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Countries_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToCountries()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Countries();

            Assert.That(result.ViewName, Is.EqualTo("Countries"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Countries_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfCountryViewModel()
        {
            IEnumerable<ICountry> countryMockCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(countryMockCollection);

            ViewResult result = (ViewResult) await sut.Countries();

            Assert.That(result.Model, Is.TypeOf<List<CountryViewModel>>());

            List<CountryViewModel> countryViewModelCollection = (List<CountryViewModel>) result.Model;

            Assert.That(countryViewModelCollection, Is.Not.Null);
            Assert.That(countryViewModelCollection, Is.Not.Empty);
            Assert.That(countryViewModelCollection.Count, Is.EqualTo(countryMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<ICountry> countryCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => countryCollection ?? _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}