using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using ApplyRequest = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.ApplyPostingJournalRequest;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.ApplyPostingJournal.ApplyPostingJournalRequest;

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
    public void Constructor_WhenCalled_StoresFormatProvider()
    {
        // Arrange
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        ApplyRequest sut = CreateSut(formatProvider);

        // Act & Assert
        Assert.That(sut.FormatProvider, Is.SameAs(formatProvider));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ApplyRequest? sut = null;

        // Act & Assert
        Assert.That(() => sut = CreateSut(null!), Throws.ArgumentNullException.With.Property("ParamName").EqualTo("formatProvider"));
        Assert.That(sut, Is.Null);
    }

    #endregion

    #region Test Helpers

    private ApplyRequest CreateSut(IFormatProvider formatProvider)
    {
        return new ApplyRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            formatProvider,
            new Mock<ISecurityContext>().Object);
    }

    #endregion
}