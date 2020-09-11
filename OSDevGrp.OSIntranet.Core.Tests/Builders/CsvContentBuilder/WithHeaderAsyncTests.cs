using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class WithHeaderAsyncTests
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
        public void WithHeaderAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithHeaderAsync<IExportQuery>(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenQueryIsNotNull_AssertGetColumnNamesAsyncWasCalledOnDomainObjectToCsvConverter()
        {
            IExportDataContentBuilder sut = CreateSut();

            await sut.WithHeaderAsync(CreateExportQuery());

            _domainObjectToCsvConverterMock.Verify(m => m.GetColumnNamesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncReturnsNull_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(false);

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncReturnsEmptyCollection_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(columnNameCollection: new string[0]);

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncReturnsNonEmptyCollectionContainingNullValues_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(columnNameCollection: new string[] {null, null, null});

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncReturnsNonEmptyCollectionContainingEmptyValues_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(columnNameCollection: new[] {string.Empty, string.Empty, string.Empty});

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncNonEmptyCollectionContainingWhiteSpaceValues_AssertNoWrittenData()
        {
            IExportDataContentBuilder sut = CreateSut(columnNameCollection: new[] {" ", "  ", "   "});

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenGetColumnNamesAsyncReturnsNonEmptyCollectionContainingValues_AssertColumnNamesHasBeenWritten()
        {
            string[] columnNameCollection = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IExportDataContentBuilder sut = CreateSut(columnNameCollection: columnNameCollection);

            await sut.WithHeaderAsync(CreateExportQuery());

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(string.Join(';', columnNameCollection.Select(columnName => $"\"{columnName}\"")) + Environment.NewLine));
        }

        private IExportDataContentBuilder CreateSut(bool hasColumnNameCollection = true, IEnumerable<string> columnNameCollection = null)
        {
            _domainObjectToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(hasColumnNameCollection ? columnNameCollection ?? _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray() : null));
            
            return new CsvContentBuilder<object, IDomainObjectToCsvConverter<object>>(_domainObjectToCsvConverterMock.Object, false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }
    }
}