using AutoFixture;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Exceptions.ValidationExceptionBase;

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
    public void Constructor_WhenCalledWithMessage_StoresMessage()
    {
        // Arrange
        string message = _fixture.Create<string>();
        
        // Act
        var sut = new TestValidationException(message);

        // Assert
        Assert.That(sut.Message, Is.EqualTo(message));
    }

    [Test]
    public void Constructor_WhenCalledWithMessage_IsException()
    {
        // Arrange
        string message = _fixture.Create<string>();
        
        // Act
        var sut = new TestValidationException(message);

        // Assert
        Assert.That(sut, Is.InstanceOf<Exception>());
    }

    #endregion

    #region Nested classes

    private sealed class TestValidationException : Interfaces.Exceptions.ValidationExceptionBase
    {
        public TestValidationException(string message) : base(message)
        {
        }
    }

    #endregion
}