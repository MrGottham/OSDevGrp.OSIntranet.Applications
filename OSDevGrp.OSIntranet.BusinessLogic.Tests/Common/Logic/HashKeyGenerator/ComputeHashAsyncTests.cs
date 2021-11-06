using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Logic.HashKeyGenerator
{
    [TestFixture]
    public class ComputeHashAsyncTests
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
        public void ComputeHashAsync_WhenByteCollectionIsNull_ThrowsArgumentNullException()
        {
            IHashKeyGenerator sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ComputeHashAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("byteCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenByteCollectionIsNotNull_ReturnsNotNull()
        {
            IHashKeyGenerator sut = CreateSut();

            string result = await sut.ComputeHashAsync(_fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenByteCollectionIsNotNull_ReturnsNonEmptyString()
        {
            IHashKeyGenerator sut = CreateSut();

            string result = await sut.ComputeHashAsync(_fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenByteCollectionIsNotNull_ReturnsBase64String()
        {
            IHashKeyGenerator sut = CreateSut();

            string result = await sut.ComputeHashAsync(_fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray());

            Assert.That(result.IsBase64String(), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenByteCollectionIsNotNull_ReturnsStringWith88Characters()
        {
            IHashKeyGenerator sut = CreateSut();

            string result = await sut.ComputeHashAsync(_fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray());

            Assert.That(result.Length, Is.EqualTo(88));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenCalledWithSameByteCollectionOnDifferentInstances_ReturnsSameStringOnEachInstance()
        {
            IHashKeyGenerator sut = CreateSut();

            byte[] byteCollection = _fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray();

            string result = await sut.ComputeHashAsync(byteCollection);

            foreach (IHashKeyGenerator otherSut in new[] { CreateSut(), CreateSut(), CreateSut() })
            {
                Assert.That(await otherSut.ComputeHashAsync(byteCollection), Is.EqualTo(result));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ComputeHashAsync_WhenCalledWithDifferentByteCollectionsSameInstance_ReturnsDifferentStrings()
        {
            IHashKeyGenerator sut = CreateSut();

            string result = await sut.ComputeHashAsync(_fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray());

            foreach (byte[] byteCollection in new[] { _fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray(), _fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray(), _fixture.CreateMany<byte>(_random.Next(128, 256)).ToArray() })
            {
                Assert.That(await sut.ComputeHashAsync(byteCollection), Is.Not.EqualTo(result));
            }
        }

        private IHashKeyGenerator CreateSut()
        {
            return new BusinessLogic.Common.Logic.HashKeyGenerator();
        }
    }
}