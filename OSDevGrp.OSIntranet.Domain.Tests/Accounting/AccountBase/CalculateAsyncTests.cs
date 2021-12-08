using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
{
    [TestFixture]
    public class CalculateAsyncTests
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
        public async Task CalculateAsync_WhenCalled_AssertGetCalculationTasksWasCalledOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            Sut result = (Sut) await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.GetCalculationTasksWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertGetCalculationTasksWasCalledWithSameStatusDate()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await sut.CalculateAsync(statusDate);

            Assert.That(result.GetCalculationTasksWasCalledWithStatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertGetCalculationResultAsyncWasCalledOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            Sut result = (Sut) await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.GetCalculationResultAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertAlreadyCalculatedWasNotCalledOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            Sut result = (Sut)await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AlreadyCalculatedWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertAllCalculationTasksIsCompleted()
        {
            Task[] calculationTasks = CreateCalculationTasks();
            IAccountBase<IAccountBase> sut = CreateSut(calculationTasks);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(calculationTasks.All(calculationTask => calculationTask.IsCompleted), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountBase()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            IAccountBase result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountBaseWhereStatusDateEqualDateFromCall()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountBase result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertGetCalculationTasksWasCalledOnlyOnceOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await ((IAccountBase<IAccountBase>) await ((IAccountBase<IAccountBase>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.GetCalculationTasksWasCalledTimes, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertGetCalculationResultAsyncWasCalledOnlyOnceOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await ((IAccountBase<IAccountBase>) await ((IAccountBase<IAccountBase>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.GetCalculationResultAsyncWasCalledTimes, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertAlreadyCalculatedWasCalledTwiceOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await ((IAccountBase<IAccountBase>) await ((IAccountBase<IAccountBase>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.AlreadyCalculatedWasCalledTimes, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameAccountBase()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountBase result = await ((IAccountBase<IAccountBase>) await ((IAccountBase<IAccountBase>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IAccountBase<IAccountBase> CreateSut(Task[] calculationTasks = null)
        {
            return new Sut(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPostingLineCollectionMock().Object, calculationTasks ?? CreateCalculationTasks());
        }

        private Task[] CreateCalculationTasks()
        {
            return new[]
            {
                Task.Run(() => { Task.Delay(_random.Next(10, 250)); }),
                Task.Run(() => { Task.Delay(_random.Next(10, 250)); }),
                Task.Run(() => { Task.Delay(_random.Next(10, 250)); })
            };
        }

        private class Sut : Domain.Accounting.AccountBase<IAccountBase>
        {
            #region Private variables

            private readonly Task[] _calculationTasks;

            #endregion

            #region Constructor

            public Sut(IAccounting accounting, string accountNumber, string accountName, IPostingLineCollection postingLineCollection, Task[] calculationTasks)
                : base(accounting, accountNumber, accountName, postingLineCollection)
            {
                NullGuard.NotNull(calculationTasks, nameof(calculationTasks));

                _calculationTasks = calculationTasks;
            }

            #endregion

            #region Properties

            public bool GetCalculationTasksWasCalled => GetCalculationTasksWasCalledTimes > 0;

            public int GetCalculationTasksWasCalledTimes { get; private set; }

            public DateTime? GetCalculationTasksWasCalledWithStatusDate { get; private set; }

            public bool GetCalculationResultAsyncWasCalled => GetCalculationResultAsyncWasCalledTimes > 0;

            public int GetCalculationResultAsyncWasCalledTimes { get; private set; }

            public bool AlreadyCalculatedWasCalled => AlreadyCalculatedWasCalledTimes > 0;

            public int AlreadyCalculatedWasCalledTimes { get; private set; }

            #endregion

            #region Methods

            protected override Task[] GetCalculationTasks(DateTime statusDate)
            {
                GetCalculationTasksWasCalledTimes++;
                GetCalculationTasksWasCalledWithStatusDate = statusDate;

                return _calculationTasks;
            }

            protected override Task<IAccountBase> GetCalculationResultAsync()
            {
                GetCalculationResultAsyncWasCalledTimes++;

                return Task.FromResult<IAccountBase>(this);
            }

            protected override IAccountBase AlreadyCalculated()
            {
                AlreadyCalculatedWasCalledTimes++;

                return this;
            }

            #endregion
        }
    }
}