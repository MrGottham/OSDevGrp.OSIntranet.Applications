using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Core;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class Accounting : AuditableBase, IAccounting
    {
        #region Private variables

        private ILetterHead _letterHead;

        #endregion

        #region Constructors

        public Accounting(int number, string name, ILetterHead letterHead)
            : this(number, name, letterHead, BalanceBelowZeroType.Creditors, 30)
        {
        }

        public Accounting(int number, string name, ILetterHead letterHead, BalanceBelowZeroType balanceBelowZero, int backDating)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNull(letterHead, nameof(letterHead));

            Number = number;
            Name = name;
            LetterHead = letterHead;
            BalanceBelowZero = balanceBelowZero;
            BackDating = backDating;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

        public ILetterHead LetterHead
        {
            get => _letterHead;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _letterHead = value;
            }
        }

        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        public int BackDating { get; set; }

        public DateTime StatusDate { get; private set; }

        public bool Deletable { get; private set; }

        public bool DefaultForPrincipal { get; private set; }

        #endregion

        #region Methods

        public Task<IAccounting> CalculateAsync(DateTime statusDate)
        {
            return Task.Run(() => 
            {
                StatusDate = statusDate.Date;

                return (IAccounting) this;
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

        public void ApplyDefaultForPrincipal(int? defaultAccountingNumber)
        {
            DefaultForPrincipal = defaultAccountingNumber.HasValue && defaultAccountingNumber.Value == Number;
        }

        #endregion
    }
}