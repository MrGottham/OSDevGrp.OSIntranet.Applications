﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PaymentTerm : AuditableBase, IPaymentTerm
    {
        #region Constructor

        public PaymentTerm(int number, string name)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            Number = number;
            Name = name;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

        public bool IsProtected { get; private set; }

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

        public void ApplyProtection()
        {
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

        #endregion
    }
}