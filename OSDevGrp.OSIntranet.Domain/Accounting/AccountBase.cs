using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountBase<T> : AuditableBase, IAccountBase<T> where T : IAccountBase
    {
        #region Private variables

        private string _description;
        private string _note;
        private bool _isCalculating;

        #endregion

        #region Constructor

        protected AccountBase(IAccounting accounting, string accountNumber, string accountName, IPostingLineCollection postingLineCollection)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNullOrWhiteSpace(accountName, nameof(accountName))
                .NotNull(postingLineCollection, nameof(postingLineCollection));

            Accounting = accounting;
            AccountNumber = accountNumber.Trim().ToUpper();
            AccountName = accountName.Trim();
            PostingLineCollection = postingLineCollection;
        }

        #endregion

        #region Properties

        public IAccounting Accounting { get; }

        public string AccountNumber { get; }

        public string AccountName { get; }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string Note
        {
            get => _note;
            set => _note = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public DateTime StatusDate { get; private set; }

        public bool IsProtected { get; private set; }

        public bool Deletable { get; private set; }

        public IPostingLineCollection PostingLineCollection { get; protected set; }

        #endregion

        #region Methods

        public async Task<T> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return AlreadyCalculated();
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                await Task.WhenAll(GetCalculationTasks(StatusDate));

                return await GetCalculationResultAsync();
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public virtual void ApplyProtection()
        {
            PostingLineCollection?.ApplyProtection();

            DisallowDeletion();

            IsProtected = true;
        }

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public override int GetHashCode()
        {
            return $"{AccountNumber}@{Accounting.Number}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null && GetHashCode() == obj.GetHashCode();
        }

        protected abstract Task[] GetCalculationTasks(DateTime statusDate);

        protected abstract Task<T> GetCalculationResultAsync();

        protected abstract T AlreadyCalculated();

        protected async Task CalculatePostingLineCollectionAsync(DateTime statusDate)
        {
            PostingLineCollection = await PostingLineCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}