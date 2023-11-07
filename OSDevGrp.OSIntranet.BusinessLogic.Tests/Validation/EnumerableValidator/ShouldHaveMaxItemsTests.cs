using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.EnumerableValidator
{
	[TestFixture]
	public class ShouldHaveMaxItems
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
		public void ShouldHaveMaxItems_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IEnumerableValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), _fixture.Create<int>(), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IEnumerableValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), _fixture.Create<int>(), GetType(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IEnumerableValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), _fixture.Create<int>(), GetType(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IEnumerableValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), _fixture.Create<int>(), GetType(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValueIsNullAndNullIsAllowed_ReturnsNotNull()
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems<object>(null, _fixture.Create<int>(), GetType(), _fixture.Create<string>(), true);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValueIsNullAndNullIsAllowed_ReturnsEnumerableValidator()
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems<object>(null, _fixture.Create<int>(), GetType(), _fixture.Create<string>(), true);

			Assert.That(result, Is.TypeOf<BusinessLogic.Validation.EnumerableValidator>());
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValueIsNullAndNullIsAllowed_ReturnsSameEnumerableValidator()
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems<object>(null, _fixture.Create<int>(), GetType(), _fixture.Create<string>(), true);

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ShouldHaveMaxItems_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
		{
			IEnumerableValidator sut = CreateSut();

			Type validatingType = GetType();
			string validatingField = _fixture.Create<string>();
			IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMaxItems<object>(null, _fixture.Create<int>(), validatingType, validatingField));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
			Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
			Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(3, 5)]
		[TestCase(4, 5)]
		[TestCase(5, 5)]
		public void ShouldHaveMaxItems_WhenValueFulfillsRequirementsForMaxItems_ReturnsNotNull(int numberOfItems, int maxItems)
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(numberOfItems).ToArray(), maxItems, GetType(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(3, 5)]
		[TestCase(4, 5)]
		[TestCase(5, 5)]
		public void ShouldHaveMaxItems_WhenValueFulfillsRequirementsForMaxItems_ReturnsEnumerableValidator(int numberOfItems, int maxItems)
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(numberOfItems).ToArray(), maxItems, GetType(), _fixture.Create<string>());

			Assert.That(result, Is.TypeOf<BusinessLogic.Validation.EnumerableValidator>());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(3, 5)]
		[TestCase(4, 5)]
		[TestCase(5, 5)]
		public void ShouldHaveMaxItems_WhenValueFulfillsRequirementsForMaxItems_ReturnsSameEnumerableValidator(int numberOfItems, int maxItems)
		{
			IEnumerableValidator sut = CreateSut();

			IValidator result = sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(numberOfItems).ToArray(), maxItems, GetType(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(4, 3)]
		[TestCase(5, 3)]
		[TestCase(6, 3)]
		public void ShouldHaveMaxItems_WhenValueDoesNotFulfillRequirementsForMaxItems_ThrowsIntranetValidationException(int numberOfItems, int maxItems)
		{
			IEnumerableValidator sut = CreateSut();

			Type validatingType = GetType();
			string validatingField = _fixture.Create<string>();
			IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMaxItems(_fixture.CreateMany<object>(numberOfItems).ToArray(), maxItems, validatingType, validatingField));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.Not.Null);
			Assert.That(result.Message, Is.Not.Empty);
			Assert.That(result.Message.Contains(maxItems.ToString()), Is.True);
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldContainMaxItems));
			Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
			Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
		}

		private IEnumerableValidator CreateSut()
		{
			return new BusinessLogic.Validation.EnumerableValidator();
		}
	}
}