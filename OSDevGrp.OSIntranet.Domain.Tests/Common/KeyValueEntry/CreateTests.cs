using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Tests.Common.KeyValueEntry
{
    [TestFixture]
    public class CreateTests
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
        public void Create_WhenKeyIsNullAndValueIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(null, _fixture.Create<TestValue>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsEmptyAndValueIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(string.Empty, _fixture.Create<TestValue>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsWhiteSpaceAndValueIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(" ", _fixture.Create<TestValue>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsNotNull()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), _fixture.Create<TestValue>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsKeyValueEntry()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), _fixture.Create<TestValue>());

            Assert.That(result, Is.TypeOf<Domain.Common.KeyValueEntry>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsKeyValueEntryWhereKeyIsEqualToKeyArgument()
        {
            string key = _fixture.Create<string>();
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(key, _fixture.Create<TestValue>());

            Assert.That(result.Key, Is.EqualTo(key));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsKeyValueEntryWhereValueIsNotNull()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), _fixture.Create<TestValue>());

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsKeyValueEntryWhereValueIsNotEmpty()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), _fixture.Create<TestValue>());

            Assert.That(result.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndValue_ReturnsKeyValueEntryWhereValueIsSerializedByteArrayOfValue()
        {
            int identifier = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            DateTime birthday = DateTime.Today.AddYears(_random.Next(10, 75) * -1).AddDays(_random.Next(-120, 120));
            decimal balance = _fixture.Create<decimal>();
            TestValue testValue = new TestValue
            {
                Identifier = identifier,
                Name = name,
                Birthday = birthday,
                Balance = balance
            };
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), testValue);

            TestValue otherTestValue = result.ToObject<TestValue>();
            Assert.That(otherTestValue, Is.Not.Null);
            Assert.That(otherTestValue.Identifier, Is.EqualTo(identifier));
            Assert.That(otherTestValue.Name, Is.EqualTo(name));
            Assert.That(otherTestValue.Birthday, Is.EqualTo(birthday));
            Assert.That(otherTestValue.Balance, Is.EqualTo(balance));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsNotNullEmptyOrWhiteSpaceAndValueIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), (TestValue) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("value"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsNullAndBase64StringIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(null, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray())));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsEmptyAndBase64StringIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(string.Empty, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray())));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsWhiteSpaceAndBase64StringIsNotNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(" ", Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray())));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsNotNullEmptyOrWhiteSpaceAndBase64StringIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsNotNullEmptyOrWhiteSpaceAndBase64StringIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenKeyIsNotNullEmptyOrWhiteSpaceAndBase64StringIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsNotNull()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsKeyValueEntry()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            Assert.That(result, Is.TypeOf<Domain.Common.KeyValueEntry>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsKeyValueEntryWhereKeyIsEqualToKeyArgument()
        {
            string key = _fixture.Create<string>();
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(key, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            Assert.That(result.Key, Is.EqualTo(key));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsKeyValueEntryWhereValueIsNotNull()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsKeyValueEntryWhereValueIsNotEmpty()
        {
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            Assert.That(result.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithKeyAndBase64String_ReturnsKeyValueEntryWhereValueIsEqualToByteArrayOfBase64String()
        {
            byte[] bytes = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            IKeyValueEntry result = Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), Convert.ToBase64String(bytes));

            Assert.That(result.Value, Is.EqualTo(bytes));
        }
    }
}