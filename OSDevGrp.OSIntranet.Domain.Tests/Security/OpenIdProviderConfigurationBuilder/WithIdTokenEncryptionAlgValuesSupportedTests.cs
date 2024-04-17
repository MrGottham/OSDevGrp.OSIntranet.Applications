﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithIdTokenEncryptionAlgValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdTokenEncryptionAlgValuesSupported_WhenIdTokenEncryptionAlgValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithIdTokenEncryptionAlgValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("idTokenEncryptionAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdTokenEncryptionAlgValuesSupported_WhenIdTokenEncryptionAlgValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithIdTokenEncryptionAlgValuesSupported(CreateIdTokenEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdTokenEncryptionAlgValuesSupported_WhenIdTokenEncryptionAlgValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithIdTokenEncryptionAlgValuesSupported(CreateIdTokenEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}