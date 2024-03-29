﻿using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class PostingLinePresentationViewModel : AuditableViewModelBase
    {
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime PostingDate { get; set; }

        public string Details { get; set; }

        public decimal PostingValue { get; set; }
    }
}