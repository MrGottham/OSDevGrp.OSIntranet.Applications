using System;
using System.Text.Json;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.PushKeyValueEntryCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            IPushKeyValueEntryCommand sut = CreateSut();

            IKeyValueEntry result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsKeyValueEntry()
        {
            IPushKeyValueEntryCommand sut = CreateSut();

            IKeyValueEntry result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<KeyValueEntry>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsKeyValueEntryWhereKeyIsEqualToKeyOnPushKeyValueEntryCommand()
        {
            string key = _fixture.Create<string>();
            IPushKeyValueEntryCommand sut = CreateSut(key);

            IKeyValueEntry result = sut.ToDomain();

            Assert.That(result.Key, Is.EqualTo(key));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsKeyValueEntryWhereValueIsEqualToByteArrayOfValueOnPushKeyValueEntryCommand()
        {
            TestValue value = BuildTestValue();
            IPushKeyValueEntryCommand sut = CreateSut(value: value);

            IKeyValueEntry result = sut.ToDomain();

            Assert.That(Convert.ToBase64String(result.Value), Is.EqualTo(value.ToBase64()));
        }

        private IPushKeyValueEntryCommand CreateSut(string key = null, TestValue value = null)
        {
            return _fixture.Build<BusinessLogic.Common.Commands.PushKeyValueEntryCommand>()
                .With(m => m.Key, key ?? _fixture.Create<string>())
                .With(m => m.Value, value ?? BuildTestValue())
                .Create();
        }

        private TestValue BuildTestValue()
        {
            return new TestValue
            {
                Age = _fixture.Create<int>()
            };
        }

        private class TestValue
        {
            #region Properties

            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public int Age { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local

            #endregion

            #region Methods

            public string ToBase64()
            {
                return Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(this, GetType(), new JsonSerializerOptions { WriteIndented = false }));
            }

            #endregion
        }
    }
}