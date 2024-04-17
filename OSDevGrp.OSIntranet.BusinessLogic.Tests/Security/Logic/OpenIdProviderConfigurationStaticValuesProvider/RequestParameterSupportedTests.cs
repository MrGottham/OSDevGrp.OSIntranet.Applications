﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class RequestParameterSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
    {
        #region Private variables

        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void RequestParameterSupported_WhenCalled_ReturnsFalse()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            bool result = sut.RequestParameterSupported;

            Assert.That(result, Is.False);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}