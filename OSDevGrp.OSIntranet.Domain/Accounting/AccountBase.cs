using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountBase<T> : AuditableBase, IAccountBase<T> where T : IAccountBase
    {
        #region Private variables

        private string _description;
        private string _note;

        #endregion

        #region Constructor

        protected AccountBase(IAccounting accounting, string accountNumber, string accountName)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNullOrWhiteSpace(accountName, nameof(accountName));

            Accounting = accounting;
            AccountNumber = accountNumber.Trim().ToUpper();
            AccountName = accountName.Trim();
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

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

        public Task<T> CalculateAsync(DateTime statusDate)
        {
            return Task.Run(() => 
            {
                StatusDate = statusDate.Date;

                return Calculate(StatusDate);
            });
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

        protected abstract T Calculate(DateTime statusDate);

        #endregion
    }
}