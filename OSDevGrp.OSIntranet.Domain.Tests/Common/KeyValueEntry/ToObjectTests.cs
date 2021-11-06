using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Tests.Common.KeyValueEntry
{
    [TestFixture]
    public class ToObjectTests
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
        public void ToObject_WhenCalled_ReturnsNotNull()
        {
            IKeyValueEntry sut = CreateSut();

            TestValue result = sut.ToObject<TestValue>();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToObject_WhenCalled_ReturnsObjectFromValue()
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
            IKeyValueEntry sut = CreateSut(testValue);

            TestValue result = sut.ToObject<TestValue>();
            
            Assert.That(result.Identifier, Is.EqualTo(identifier));
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Birthday, Is.EqualTo(birthday));
            Assert.That(result.Balance, Is.EqualTo(balance));
        }

        private IKeyValueEntry CreateSut(TestValue testValue = null)
        {
            return Domain.Common.KeyValueEntry.Create(_fixture.Create<string>(), testValue ?? _fixture.Create<TestValue>());
        }
    }
}