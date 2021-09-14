using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Logic.KeyGenerator
{
    [TestFixture]
    public class GenerateGenericKeyAsyncTests
    {
        #region Private variables

        private Mock<IHashKeyGenerator> _hashKeyGeneratorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _hashKeyGeneratorMock = new Mock<IHashKeyGenerator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsNull_ThrowsArgumentNullException()
        {
            IKeyGenerator sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GenerateGenericKeyAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("keyElementCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationException()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationExceptionWhereMessageContainsKeyElementCollection()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("keyElementCollection"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueShouldContainSomeItems()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldContainSomeItems));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsEqualToStringEnumerable()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(IEnumerable<string>)));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToKeyElementCollection()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingField, Is.EqualTo("keyElementCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateGenericKeyAsync_WhenKeyElementCollectionIsEmpty_ThrowsIntranetValidationExceptionWhereInnerExceptionIsNull()
        {
            IKeyGenerator sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.GenerateGenericKeyAsync(Array.Empty<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateGenericKeyAsync_WhenKeyElementCollectionHasKeyElements_AssertComputeHashAsyncWasCalledOnHashKeyGenerator()
        {
            IKeyGenerator sut = CreateSut();

            string[] keyElements = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            await sut.GenerateGenericKeyAsync(keyElements);

            _hashKeyGeneratorMock.Verify(m => m.ComputeHashAsync(It.Is<IEnumerable<byte>>(value => value != null && Convert.ToBase64String(value.ToArray()) == Convert.ToBase64String(CalculateByteArrayForKeyElements(keyElements)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateGenericKeyAsync_WhenKeyElementCollectionHasKeyElements_ReturnsNotNull()
        {
            IKeyGenerator sut = CreateSut();

            string result = await sut.GenerateGenericKeyAsync(_fixture.CreateMany<string>(_random.Next(1, 5)).ToArray());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateGenericKeyAsync_WhenKeyElementCollectionHasKeyElements_ReturnsComputedHashFromComputeHashAsyncOnHashKeyGenerator()
        {
            string computedHash = _fixture.Create<string>();
            IKeyGenerator sut = CreateSut(computedHash);

            string result = await sut.GenerateGenericKeyAsync(_fixture.CreateMany<string>(_random.Next(1, 5)).ToArray());

            Assert.That(result, Is.EqualTo(computedHash));
        }

        private IKeyGenerator CreateSut(string computedHash = null)
        {
            _hashKeyGeneratorMock.Setup(m => m.ComputeHashAsync(It.IsAny<byte[]>()))
                .Returns(Task.FromResult(computedHash ?? _fixture.Create<string>()));

            return new BusinessLogic.Common.Logic.KeyGenerator(_hashKeyGeneratorMock.Object, _claimResolverMock.Object);
        }

        private static byte[] CalculateByteArrayForKeyElements(params string[] keyElements)
        {
            NullGuard.NotNull(keyElements, nameof(keyElements));

            return Encoding.UTF8.GetBytes(string.Join('|', keyElements));
        }
    }
}