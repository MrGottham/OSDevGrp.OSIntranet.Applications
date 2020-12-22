using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Common;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.CommonController
{
    [TestFixture]
    public class LetterHeadsAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeadsAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.LetterHeadsAsync();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeadsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<LetterHeadModel>> result = await sut.LetterHeadsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeadsAsync_WhenCalled_AssertOkObjectResultContainsLetterHeads()
        {
            IList<ILetterHead> letterHeadMockCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(letterHeadMockCollection);

            OkObjectResult result = (OkObjectResult) (await sut.LetterHeadsAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            IList<LetterHeadModel> letterHeadModels = ((IEnumerable<LetterHeadModel>) result.Value).ToList();
            Assert.That(letterHeadModels, Is.Not.Null);
            Assert.That(letterHeadModels, Is.Not.Empty);
            Assert.That(letterHeadModels.Count, Is.EqualTo(letterHeadMockCollection.Count));
        }

        private Controller CreateSut(IEnumerable<ILetterHead> letterHeads = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(letterHeads ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));

            return new Controller(_queryBusMock.Object);
        }
    }
}