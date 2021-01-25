﻿using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BalanceInfoViewModel : InfoViewModelBase
    {
        [Display(Name = "Saldo", ShortName = "Saldo", Description = "Saldo")]
        public decimal Balance { get; set; }
    }
}