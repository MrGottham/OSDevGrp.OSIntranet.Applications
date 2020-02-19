using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.Contact
{
    [TestFixture]
    public class CalculateIdentifierTests
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
        public void CalculateIdentifier_WhenCalled_AssertDisplayNameWasCalledOnName()
        {
            Mock<IName> nameMock = _fixture.BuildNameMock();
            IContact sut = CreateSut(nameMock.Object);

            sut.CalculateIdentifier();

            nameMock.Verify(m => m.DisplayName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateIdentifier_WhenCalled_ReturnsBase64String()
        {
            IContact sut = CreateSut();

            string result = sut.CalculateIdentifier();

            Assert.That(result.IsBase64String(), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateIdentifier_WhenCalled_ReturnsBase64StringBasedOnContact()
        {
            string displayName = _fixture.Create<string>();
            IName name = _fixture.BuildNameMock(displayName).Object;
            string mailAddress = _fixture.Create<string>();
            string primaryPhone = _fixture.Create<string>();
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(name, mailAddress, primaryPhone, secondaryPhone);

            string result = sut.CalculateIdentifier();

            using MemoryStream sourceMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes($"{displayName.Trim().Replace(" ", "")}|{mailAddress.Trim().Replace(" ", "")}|{primaryPhone.Trim().Replace(" ", "")}|{secondaryPhone.Trim().Replace(" ", "")}"));

            using MemoryStream targetMemoryStream = new MemoryStream();
            using GZipStream gZipStream = new GZipStream(targetMemoryStream, CompressionMode.Compress);
            gZipStream.Write(sourceMemoryStream.ToArray());
            gZipStream.Flush();

            targetMemoryStream.Seek(0, SeekOrigin.Begin);

            Assert.That(result, Is.EqualTo(Convert.ToBase64String(targetMemoryStream.ToArray())));
        }

        private IContact CreateSut(IName name = null, string mailAddress = null, string primaryPhone = null, string secondaryPhone = null)
        {
            return new Domain.Contacts.Contact(name ?? _fixture.BuildPersonNameMock().Object)
            {
                MailAddress = mailAddress ?? _fixture.Create<string>(),
                PrimaryPhone = primaryPhone ?? _fixture.Create<string>(),
                SecondaryPhone = secondaryPhone ?? _fixture.Create<string>()
            };
        }
    }
}