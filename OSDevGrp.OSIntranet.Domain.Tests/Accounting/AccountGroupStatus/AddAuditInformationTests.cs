using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
    public class AddAuditInformationTests
    {
        #region Properties

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
        public void AddAuditInformation_WhenCalled_ThrowsNotSupportedException()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.Throws<NotSupportedException>(() => sut.AddAuditInformation(DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>(), DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>()));
        }

        private IAccountGroupStatus CreateSut()
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, _fixture.BuildAccountCollectionMock().Object);
        }
    }
}