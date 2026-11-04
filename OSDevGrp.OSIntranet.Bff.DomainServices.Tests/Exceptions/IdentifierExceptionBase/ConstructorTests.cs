using AutoFixture;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Exceptions.IdentifierExceptionBase;

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
    public void Constructor_WhenCalledWithMessageAndIdentifier_StoresMessage()
    {
        // Arrange
        string message = _fixture.Create<string>();
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new TestIdentifierException(message, identifier);

        // Assert
        Assert.That(sut.Message, Is.EqualTo(message));
    }

    [Test]
    public void Constructor_WhenCalledWithMessageAndIdentifier_StoresIdentifier()
    {
        // Arrange
        string message = _fixture.Create<string>();
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new TestIdentifierException(message, identifier);

        // Assert
        Assert.That(sut.Identifier, Is.EqualTo(identifier));
    }

    [Test]
    public void Constructor_WhenCalledWithMessageAndIdentifier_IsValidationExceptionBase()
    {
        // Arrange
        string message = _fixture.Create<string>();
        Guid identifier = _fixture.Create<Guid>();

        // Act
        var sut = new TestIdentifierException(message, identifier);

        // Assert
        Assert.That(sut, Is.InstanceOf<OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions.ValidationExceptionBase>());
        Assert.That(sut, Is.InstanceOf<Exception>());
    }

    #endregion

    #region Nested classes

    private sealed class TestIdentifierException : Interfaces.Exceptions.IdentifierExceptionBase
    {
        public TestIdentifierException(string message, Guid identifier) : base(message, identifier)
        {
        }
    }

    #endregion
}