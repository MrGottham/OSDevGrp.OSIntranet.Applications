using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollectionBase
{
    [TestFixture]
    public class DeletableTests
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
        public void Deletable_WhenApplyProtectionHasNotBeenCalled_AssertDeletableWasCalledAtMostOnceOnOneOrMoreAccount()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            sut.Add(accountMockCollection.Select(m => m.Object).ToArray());

            // ReSharper disable UnusedVariable
            bool result = sut.Deletable;
            // ReSharper restore UnusedVariable

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.Deletable, Times.AtMostOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalled_AssertDeletableWasNotCalledOnAnyAccount()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            sut.Add(accountMockCollection.Select(m => m.Object).ToArray());

            sut.ApplyProtection();

            // ReSharper disable UnusedVariable
            bool result = sut.Deletable;
            // ReSharper restore UnusedVariable

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.Deletable, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndNoAccountHasBeenAdded_ReturnsTrue()
        {
            IProtectable sut = CreateSut();

            bool result = sut.Deletable;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndAllAccountsIsDeletable_ReturnsTrue()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount[] accountCollection =
            {
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object
            };
            sut.Add(accountCollection);

            bool result = sut.Deletable;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndOneOrMoreAccountsIsNotDeletable_ReturnsFalse()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount[] accountCollection =
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock(deletable: false).Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };
            sut.Add(accountCollection);

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndNoAccountBaseHasBeenAdded_ReturnsFalse()
        {
            IProtectable sut = CreateSut();

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndAllAccountsIsDeletable_ReturnsFalse()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount[] accountCollection =
            {
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object,
                _fixture.BuildAccountMock(deletable: true).Object
            };
            sut.Add(accountCollection);

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndOneOrMoreAccountsIsNotDeletable_ReturnsFalse()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount[] accountCollection =
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock(deletable: false).Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };
            sut.Add(accountCollection);

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        private IAccountCollectionBase<IAccount, Sut> CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.AccountCollectionBase<IAccount, Sut>
        {
            #region Methods

            protected override Sut Calculate(DateTime statusDate, IReadOnlyCollection<IAccount> calculatedAccountCollection) => throw new NotSupportedException();

            protected override Sut AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}