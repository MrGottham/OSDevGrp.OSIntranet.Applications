﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithNonceTests : IdTokenContentBuilderTestBase
    {
        #region Private variables

        private Mock<IClaimsSelector> _claimSelectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimSelectorMock = new Mock<IClaimsSelector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        protected override Mock<IClaimsSelector> ClaimsSelectorMock => _claimSelectorMock;

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithNonce(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsNotNullEmptyOrWhiteSpace_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithNonce(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }
    }
}