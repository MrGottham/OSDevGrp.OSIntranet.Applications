using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ResolvePostalCodeTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

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
            _fixture.Customize<IPostalCode>(builder => builder.FromFactory(() => _fixture.BuildPostalCodeMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithCountryCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithCountryCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithCountryCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithPostalCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithPostalCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ResolvePostalCode_WhenCalledWithPostalCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ResolvePostalCode(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolvePostalCode_WhenCalledWithCountryCodeAndPostalCode_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            await sut.ResolvePostalCode(countryCode, postalCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostalCodeQuery, IPostalCode>(It.Is<IGetPostalCodeQuery>(value => string.CompareOrdinal(value.CountryCode, countryCode.ToUpper()) == 0 && string.CompareOrdinal(value.PostalCode, postalCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolvePostalCode_WhenCalledWithCountryCodeAndPostalCode_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolvePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolvePostalCode_WhenCalledWithCountryCodeAndPostalCode_ReturnsOkObjectResultWhereValueIsPostalCodeViewModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) await sut.ResolvePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.Value, Is.TypeOf<PostalCodeViewModel>());
        }

        private Controller CreateSut(IPostalCode postalCode = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetPostalCodeQuery, IPostalCode>(It.IsAny<IGetPostalCodeQuery>()))
                .Returns(Task.Run(() => postalCode ?? _fixture.BuildPostalCodeMock().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}