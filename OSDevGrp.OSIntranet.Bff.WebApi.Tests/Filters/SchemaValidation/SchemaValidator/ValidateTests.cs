using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.SchemaValidation.SchemaValidator;

[TestFixture]
public class ValidateTests
{
    #region Private variables

    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoIsNull_ThrowsSchemaValidationException()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = null,
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoIsNull_ThrowsSchemaValidationExceptionWhereMessageContainsName()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = null,
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("Name"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoIsNull_ThrowsSchemaValidationExceptionWhereMessageContainsRequired()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = null,
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("required"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoIsNull_ThrowsSchemaValidationExceptionWhereInnerExceptionIsNull()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = null,
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoDoesNotMeetLengthRequirements_ThrowsSchemaValidationException()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoDoesNotMeetLengthRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsName()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("Name"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoDoesNotMeetLengthRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsLength()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("length"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenNameAtTestDtoDoesNotMeetLengthRequirements_ThrowsSchemaValidationExceptionWhereInnerExceptionIsNull()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenAgeAtTestDtoDoesNotMeetRangeRequirements_ThrowsSchemaValidationException()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateValidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenAgeAtTestDtoDoesNotMeetRangeRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsAge()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateValidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("Age"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenAgeAtTestDtoDoesNotMeetRangeRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsBetwwen()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateValidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("between 15 and 75"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenAgeAtTestDtoDoesNotMeetRangeRequirements_ThrowsSchemaValidationExceptionWhereInnerExceptionIsNull()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateValidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoDoesNotMeetRequirements_ThrowsSchemaValidationException()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoDoesNotMeetRequirements_ThrowsSchemaValidationExceptionWhereMessageStartsWithSpecifiedText()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.StartsWith("Multiple values does not satisfy the specified requirements:"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoDoesNotMeetRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsNameAndAge()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("Name"), Is.True);
            Assert.That(ex.Message.Contains("Age"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoDoesNotMeetRequirements_ThrowsSchemaValidationExceptionWhereMessageContainsLengthAndBetween()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.Message.Contains("length"), Is.True);
            Assert.That(ex.Message.Contains("between 15 and 75"), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoDoesNotMeetRequirements_ThrowsSchemaValidationExceptionWhereInnerExceptionIsNull()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateInvalidName(),
            Age = CreateInvalidAge()
        };
        try
        {
            sut.Validate(testDto);

            Assert.Fail("A SchemaValidationException should have been thrown.");
        }
        catch (SchemaValidationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public void Validate_WhenBothNameAndAgeAtTestDtoMeetsRequirements_AssertchemaValidationExceptionWasNotThrown()
    {
        ISchemaValidator sut = CreateSut();

        TestDto testDto = new TestDto
        {
            Name = CreateValidName(),
            Age = CreateValidAge()
        };
        try
        {
            sut.Validate(testDto);
        }
        catch (SchemaValidationException)
        {
            Assert.Fail("A SchemaValidationException should not have been thrown.");
        }
    }

    private ISchemaValidator CreateSut()
    {
        return new WebApi.Filters.SchemaValidation.SchemaValidator();
    }

    private class TestDto
    {
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }

        [System.ComponentModel.DataAnnotations.Range(15, 75)]
        public int Age { get; set; }
    }

    private string CreateValidName()
    {
        return _fixture!.Create<string>();
    }

    private string CreateInvalidName()
    {
        return _fixture!.Create<string>().Substring(0, 2);
    }

    private int CreateValidAge()
    {
        return _random!.Next(15, 75);
    }

    private int CreateInvalidAge()
    {
        return _random!.Next(7, 14);
    }
}