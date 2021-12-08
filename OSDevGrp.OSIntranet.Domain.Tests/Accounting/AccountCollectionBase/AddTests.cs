using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollectionBase
{
    [TestFixture]
    public class AddTests
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
        public void Add_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IAccount) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("account"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenAccountIsNotNullAndDoesNotExistWithinCollection_AddsAccountToCollection()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            sut.Add(account);

            Assert.That(sut.Contains(account), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenAccountIsNotNullAndDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            sut.Add(account);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(account));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains(account.GetType().Name), Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenAccountCollectionIsNull_ThrowsArgumentNullException()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IEnumerable<IAccount>) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenAccountCollectionIsNotNullAndEachAccountDoesNotExistWithinCollection_AddsEachAccountToCollection()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };
            sut.Add(accountCollection);

            Assert.That(accountCollection.All(account => sut.Contains(account)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenAccountCollectionIsNotNullAndOneAccountDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IList<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };

            IAccount existingAccount = accountCollection[_random.Next(0, accountCollection.Count - 1)];
            sut.Add(existingAccount);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(accountCollection));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains(existingAccount.GetType().Name), Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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