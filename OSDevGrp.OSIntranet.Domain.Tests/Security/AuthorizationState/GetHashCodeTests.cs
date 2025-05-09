﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class GetHashCodeTests : AuthorizationStateTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, true, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, false, false)]
        public void GetHashCode_WhenCalled_ReturnsHashCodeEqualToHashCodeFromToString(bool hasClientSecret, bool hasExternalState, bool hasNonce, bool hasAuthorizationCode)
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasNonce: hasNonce, hasAuthorizationCode: hasAuthorizationCode);

            int result = sut.GetHashCode();

            Assert.That(result, Is.EqualTo(sut.ToString()!.GetHashCode()));
        }

        private IAuthorizationState CreateSut(bool hasClientSecret = false, bool hasExternalState = true, bool hasNonce = true, bool hasAuthorizationCode = false)
        {
            return CreateSut(_fixture, _random, hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasNonce: hasNonce, hasAuthorizationCode: hasAuthorizationCode);
        }
    }
}