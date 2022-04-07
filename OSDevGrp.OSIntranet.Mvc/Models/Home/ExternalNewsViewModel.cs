using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class ExternalNewsViewModel
    {
        public string Identifier { get; set; }

        public DateTime Timestamp { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dddd}")]
        public DateTime Weekday => Timestamp;

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d. MMMM yyyy}")]
        public DateTime Date => Timestamp;

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime Time => Timestamp;

        public string Header { get; set; }

        public string Provider { get; set; }

        public string SourceUrl { get; set; } 
    }
}