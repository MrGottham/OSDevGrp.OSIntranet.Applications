using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.CsvContentBuilder
{
    [TestFixture]
    public class WithContentAsyncTests
    {
        #region Private variables

        private Mock<IDomainObjectToCsvConverter<object>> _domainObjectToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _domainObjectToCsvConverterMock = new Mock<IDomainObjectToCsvConverter<object>>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, IEnumerable<object>>(null, new object[0]));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenDataIsNull_ThrowsArgumentNullException()
        {
            IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, IEnumerable<object>>(CreateExportQuery(), null));

            Assert.That(result.ParamName, Is.EqualTo("data"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenDataCantBeCastToEnumerableOfDomainObject_ThrowsArgumentException()
        {
            IExportDataContentBuilder sut = CreateSut();

            ArgumentException result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<object>()));

            Assert.That(result.Message, Is.EqualTo($"Value cannot be cast to IEnumerable<{nameof(Object)}>. (Parameter 'data')"));
            Assert.That(result.ParamName, Is.EqualTo("data"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsEmptyDomainObjectCollection_AssertConvertAsyncWasNotCalledOnDomainObjectToCsvConverter()
        {
            IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), new object[0]);

            _domainObjectToCsvConverterMock.Verify(m => m.ConvertAsync(It.IsAny<object>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsEmptyDomainObjectCollection_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), new object[0]);

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsNonEmptyDomainObjectCollectionContainingNullValues_AssertConvertAsyncWasNotCalledOnDomainObjectToCsvConverter()
        {
            IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), new object[] {null, null, null});

            _domainObjectToCsvConverterMock.Verify(m => m.ConvertAsync(It.IsAny<object>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsNonEmptyDomainObjectCollectionContainingNullValues_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), new object[] {null, null, null});

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsNonEmptyDomainObjectCollectionContainingValues_AssertConvertAsyncWasCalledOnDomainObjectToCsvConverter()
        {
            IExportDataContentBuilder sut = CreateSut();

            object[] domainObjectCollection = _fixture.CreateMany<object>(_random.Next(5, 10)).ToArray();
            await sut.WithContentAsync(CreateExportQuery(), domainObjectCollection);

            _domainObjectToCsvConverterMock.Verify(m => m.ConvertAsync(It.IsNotNull<object>()), Times.Exactly(domainObjectCollection.Length));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsNonEmptyDomainObjectCollectionContainingValues_AssertConvertAsyncWasCalledOnDomainObjectToCsvConverterForEachDomainObject()
        {
            IExportDataContentBuilder sut = CreateSut();

            object[] domainObjectCollection = _fixture.CreateMany<object>(_random.Next(5, 10)).ToArray();
            await sut.WithContentAsync(CreateExportQuery(), domainObjectCollection);

            foreach (object domainObject in domainObjectCollection)
            {
                _domainObjectToCsvConverterMock.Verify(m => m.ConvertAsync(It.Is<object>(value => value != null && value == domainObject)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenDataIsNonEmptyDomainObjectCollectionContainingValues_AssertDataForDomainObjectCollectionHasBeenWritten()
        {
            IExportDataContentBuilder sut = CreateSut();

            object[] domainObjectCollection = _fixture.CreateMany<object>(_random.Next(5, 10)).ToArray();
            await sut.WithContentAsync(CreateExportQuery(), domainObjectCollection);

            byte[] result = await sut.BuildAsync();

            Assert.That(new Regex(Environment.NewLine, RegexOptions.Compiled).Matches(Encoding.UTF8.GetString(result)).Count, Is.EqualTo(domainObjectCollection.Length));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsNullForOneDomainObject_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(false);

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsEmptyCollectionForOneDomainObject_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(columnValueCollection: new string[0]);

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsNonEmptyCollectionContainingNullValuesForOneDomainObject_AssertColumnValuesHasBeenWritten()
        {
            IExportDataContentBuilder sut = CreateSut(columnValueCollection: new string[] {null, null, null});

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo($"\"\";\"\";\"\"{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsNonEmptyCollectionContainingEmptyValuesForOneDomainObject_AssertColumnValuesHasBeenWritten()
        {
            IExportDataContentBuilder sut = CreateSut(columnValueCollection: new[] {string.Empty, string.Empty, string.Empty});

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo($"\"\";\"\";\"\"{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsNonEmptyCollectionContainingWhiteSpaceValuesForOneDomainObject_AssertColumnValuesHasBeenWritten()
        {
            IExportDataContentBuilder sut = CreateSut(columnValueCollection: new[] {" ", "  ", "   "});

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo($"\"\";\"\";\"\"{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenConvertAsyncReturnsNonEmptyCollectionContainingValuesForOneDomainObject_AssertColumnValuesHasBeenWritten()
        {
            string[] columnValueCollection = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IExportDataContentBuilder sut = CreateSut(columnValueCollection: columnValueCollection);

            await sut.WithContentAsync(CreateExportQuery(), new[] {_fixture.Create<object>()});

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(string.Join(';', columnValueCollection.Select(columnValue => $"\"{columnValue}\"")) + Environment.NewLine));
        }

        private IExportDataContentBuilder CreateSut(bool hasColumnValueCollection = true, IEnumerable<string> columnValueCollection = null)
        {
            _domainObjectToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(hasColumnValueCollection ? columnValueCollection ?? _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray() : null));

            return new CsvContentBuilder<object, IDomainObjectToCsvConverter<object>>(_domainObjectToCsvConverterMock.Object, false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }
    }
}