using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId { get; set; }

        //[ForeignKey("Conference")]
        public int ConferenceId { get; set; }

        //[ForeignKey("Fee")]
        [Display(Name = "Registration Fee Category")]
        public int FeeId { get; set; }

        //[ForeignKey("User")]
        public int UserId { get; set; }

        [Display(Name="Register Me As")]
        //[ForeignKey("UserType")]
        public int UserTypeId { get; set; }

        public virtual Conference conference { get; set; }
        public virtual Fee fee { get; set; }
        public virtual User user { get; set; }
        public virtual UserType usertype { get; set; }
    }
}