using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("DateDealines")]
    public class DateDealine
    {
        public int DateDealineId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference conference { get; set; }
    }
}