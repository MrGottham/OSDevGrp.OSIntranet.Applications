using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class InfoViewModelBase : AuditableViewModelBase
    {
        [Display(Name = "År", ShortName = "År", Description = "År")]
        [Required(ErrorMessage = "Der skal angives et årstal.")]
        [Range(1950, 2199, ErrorMessage = "Årstallet skal være mellem {1} og {2}.")]
        public short Year { get; set; }

        [Display(Name = "Måned", ShortName = "Måned", Description = "Måned")]
        [Required(ErrorMessage = "Der skal angives en måned.")]
        [Range(1, 12, ErrorMessage = "Måneden skal være mellem {1} og {2}.")]
        public short Month { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:MMMM}")]
        public DateTime MonthAsText => new(Year, Month, 1);

        public bool IsCurrentMonth => Year == Today.Year && Month == Today.Month;

        public bool Editable => IsProtected == false && (Year > Today.Year || Year == Today.Year && Month >= Today.Month);

        public bool IsProtected { get; set; }

        public bool Deletable { get; set; }

        private DateTime Today => DateTime.Today;
    }
}