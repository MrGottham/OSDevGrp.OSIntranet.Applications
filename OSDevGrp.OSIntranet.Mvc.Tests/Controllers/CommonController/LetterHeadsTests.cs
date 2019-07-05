using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class LetterHeadsTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeads_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.LetterHeads();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeads_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LetterHeads();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeads_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToLetterHeads()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.LetterHeads();

            Assert.That(result.ViewName, Is.EqualTo("LetterHeads"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LetterHeads_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfLetterHeadViewModel()
        {
            IEnumerable<ILetterHead> letterHeadMockCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(letterHeadMockCollection);

            ViewResult result = (ViewResult) await sut.LetterHeads();

            Assert.That(result.Model, Is.TypeOf<List<LetterHeadViewModel>>());

            List<LetterHeadViewModel> letterHeadViewModelCollection = ((List<LetterHeadViewModel>) result.Model);

            Assert.That(letterHeadViewModelCollection, Is.Not.Null);
            Assert.That(letterHeadViewModelCollection, Is.Not.Empty);
            Assert.That(letterHeadViewModelCollection.Count, Is.EqualTo(letterHeadViewModelCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<ILetterHead> letterHeadCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => letterHeadCollection ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}