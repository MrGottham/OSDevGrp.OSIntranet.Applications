﻿using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.ExportDataContentBuilderBase
{
    [TestFixture]
    public class WithHeaderAsyncTests
    {
        [Test]
        [Category("UnitTest")]
        public void WithHeaderAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithHeaderAsync<IExportQuery>(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithHeaderAsync_WhenQueryIsNotNull_ThrowsNoException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            await sut.WithHeaderAsync(CreateExportQuery());
        }

        private IExportDataContentBuilder CreateSut()
        {
            return new MyExportDataContentBuilder();
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private class MyExportDataContentBuilder : Core.Builders.ExportDataContentBuilderBase
        {
        }
    }
}