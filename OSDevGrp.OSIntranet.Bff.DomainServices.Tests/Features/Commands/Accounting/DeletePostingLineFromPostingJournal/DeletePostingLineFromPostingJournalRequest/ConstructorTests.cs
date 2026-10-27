using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using DeletePostingLineRequest = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.DeletePostingLineFromPostingJournalRequest;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.DeletePostingLineFromPostingJournal.DeletePostingLineFromPostingJournalRequest;

[TestFixture]
public class ConstructorTests
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_StoresHumanVerificationCredentials()
    {
        // Arrange
        string verificationKey = _fixture!.Create<string>();
        string verificationCode = _fixture.Create<string>();
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");

        // Act
        DeletePostingLineRequest sut = CreateRequest(verificationKey, verificationCode, formatProvider);

        // Assert
        Assert.That(sut, Is.InstanceOf<IHumanVerifiableRequest>());
        Assert.That(sut.VerificationKey, Is.EqualTo(verificationKey));
        Assert.That(sut.VerificationCode, Is.EqualTo(verificationCode));
        Assert.That(sut.FormatProvider, Is.SameAs(formatProvider));
    }

    #endregion

    #region Test Helpers

    private DeletePostingLineRequest CreateRequest(
        string verificationKey,
        string verificationCode,
        IFormatProvider formatProvider)
    {
        return new DeletePostingLineRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            verificationKey,
            verificationCode,
            formatProvider,
            new Mock<ISecurityContext>().Object);
    }

    #endregion
}