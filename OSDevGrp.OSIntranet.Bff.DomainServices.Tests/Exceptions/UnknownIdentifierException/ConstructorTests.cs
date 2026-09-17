using AutoFixture;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Exceptions.UnknownIdentifierException;

[TestFixture]
[Category("UnitTest")]
public class ConstructorTests
{
    #region Private variables

    private Fixture _fixture = null!;

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
    public void Constructor_WhenCalledWithIdentifier_StoresIdentifier()
    {
        // Arrange
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new Interfaces.Exceptions.UnknownIdentifierException(identifier);

        // Assert
        Assert.That(sut.Identifier, Is.EqualTo(identifier));
    }

    [Test]
    public void Constructor_WhenCalledWithIdentifier_UsesDefaultMessage()
    {
        // Arrange
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new Interfaces.Exceptions.UnknownIdentifierException(identifier);

        // Assert
        Assert.That(sut.Message, Is.EqualTo("The identifier is unknown."));
    }

    [Test]
    public void Constructor_WhenCalledWithIdentifierAndCustomMessage_StoresCustomMessage()
    {
        // Arrange
        Guid identifier = _fixture.Create<Guid>();
        string customMessage = _fixture.Create<string>();

        // Act
        var sut = new Interfaces.Exceptions.UnknownIdentifierException(identifier, customMessage);

        // Assert
        Assert.That(sut.Message, Is.EqualTo(customMessage));
    }

    [Test]
    public void Constructor_WhenCalled_IsIdentifierExceptionBase()
    {
        // Arrange
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new Interfaces.Exceptions.UnknownIdentifierException(identifier);

        // Assert
        Assert.That(sut, Is.InstanceOf<Interfaces.Exceptions.IdentifierExceptionBase>());
    }

    #endregion
}