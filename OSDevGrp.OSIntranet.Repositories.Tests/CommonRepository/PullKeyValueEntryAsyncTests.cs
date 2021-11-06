using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class PullKeyValueEntryAsyncTests : CommonRepositoryTestBase
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
        public void PullKeyValueEntryAsync_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PullKeyValueEntryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void PullKeyValueEntryAsync_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PullKeyValueEntryAsync(string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void PullKeyValueEntryAsync_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PullKeyValueEntryAsync(" "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task PullKeyValueEntryAsync_WhenKeyHasNotBeenPushed_ReturnsNull()
        {
            ICommonRepository sut = CreateSut();

            string key = BuildKey();
            IKeyValueEntry result = await sut.PullKeyValueEntryAsync(key);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task PullKeyValueEntryAsync_WhenKeyHasBeenPushed_ReturnsKeyValueEntry()
        {
            ICommonRepository sut = CreateSut();

            string key = BuildKey();
            byte[] value = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();

            IKeyValueEntry pushedKeyValueEntry = await sut.PushKeyValueEntryAsync(new KeyValueEntry(key, value));
            try
            {
                IKeyValueEntry result = await sut.PullKeyValueEntryAsync(key);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Key, Is.EqualTo(key));
                Assert.That(result.Value, Is.EqualTo(value));
            }
            finally
            {
                await sut.DeleteKeyValueEntryAsync(pushedKeyValueEntry.Key);
            }
        }

        private string BuildKey()
        {
            string key = $"{_fixture.Create<string>()}|{GetType().Name}|{_fixture.Create<string>()}";

            using MD5 md5Hash = MD5.Create();
            byte[] keyData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(key));

            StringBuilder keyBuilder = new StringBuilder();
            foreach (byte b in keyData)
            {
                keyBuilder.Append(b.ToString("x2"));
            }

            return keyBuilder.ToString();
        }
    }
}