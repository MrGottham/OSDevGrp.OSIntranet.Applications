using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Logic.CountryHelper
{
    [TestFixture]
    public class ApplyLogicForPrincipalTests
    {
        #region Private variables

        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCountryIsNull_ThrowsArgumentNullException()
        {
            ICountryHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ApplyLogicForPrincipal((ICountry) null));

            Assert.That(result.ParamName, Is.EqualTo("country"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountry_AssertGetCountryCodeWasCalledOnClaimResolver()
        {
            ICountryHelper sut = CreateSut();

            sut.ApplyLogicForPrincipal(_fixture.Create<ICountry>());

            _claimResolverMock.Verify(m => m.GetCountryCode(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountry_AssertApplyDefaultForPrincipalWasCalledOnCountry()
        {
            string countryCode = _fixture.Create<string>();
            ICountryHelper sut = CreateSut(countryCode);

            Mock<ICountry> countryMock = _fixture.BuildCountryMock();
            sut.ApplyLogicForPrincipal(countryMock.Object);

            countryMock.Verify(m => m.ApplyDefaultForPrincipal(It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountry_ReturnsCountry()
        {
            ICountryHelper sut = CreateSut();

            ICountry country = _fixture.Create<ICountry>();
            ICountry result = sut.ApplyLogicForPrincipal(country);

            Assert.That(result, Is.EqualTo(country));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCountryCollectionIsNull_ThrowsArgumentNullException()
        {
            ICountryHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ApplyLogicForPrincipal((IEnumerable<ICountry>) null));

            Assert.That(result.ParamName, Is.EqualTo("countryCollection"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountryCollection_AssertGetCountryCodeWasCalledOnClaimResolver()
        {
            ICountryHelper sut = CreateSut();

            sut.ApplyLogicForPrincipal(_fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList());

            _claimResolverMock.Verify(m => m.GetCountryCode(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountryCollection_AssertApplyDefaultForPrincipalWasCalledOnEachCountry()
        {
            string countryCode = _fixture.Create<string>();
            ICountryHelper sut = CreateSut(countryCode);

            IEnumerable<Mock<ICountry>> countryMockCollection = new List<Mock<ICountry>>
            {
                _fixture.BuildCountryMock(),
                _fixture.BuildCountryMock(),
                _fixture.BuildCountryMock()
            };
            sut.ApplyLogicForPrincipal(countryMockCollection.Select(countryMock => countryMock.Object).ToList());

            foreach (Mock<ICountry> countryMock in countryMockCollection)
            {
                countryMock.Verify(m => m.ApplyDefaultForPrincipal(It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0)), Times.Once());
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithCountryCollection_ReturnsCountryCollection()
        {
            ICountryHelper sut = CreateSut();

            IEnumerable<ICountry> countryCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            IEnumerable<ICountry> result = sut.ApplyLogicForPrincipal(countryCollection);

            Assert.That(result, Is.EqualTo(countryCollection));
        }

        private ICountryHelper CreateSut(string countryCode = null)
        {
            _claimResolverMock.Setup(m => m.GetCountryCode())
                .Returns(countryCode ?? _fixture.Create<string>());

            return new BusinessLogic.Contacts.Logic.CountryHelper(_claimResolverMock.Object);
        }
    }
}
