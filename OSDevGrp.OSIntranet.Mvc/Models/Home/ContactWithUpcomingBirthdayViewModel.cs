using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class ContactWithUpcomingBirthdayViewModel : ContactInfoViewModel
    {
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d. MMMM yyyy}")]
        public DateTime UpcomingBirthday { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dddd}")]
        public DateTime WeekDayForUpcomingBirthday => UpcomingBirthday;

        public ushort AgeOnUpcomingBirthday { get; set; }
    }
}