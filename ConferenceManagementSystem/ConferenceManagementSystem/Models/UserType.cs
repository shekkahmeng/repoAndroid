using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("UserType")]
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        public string Name { get; set; }

        public ICollection<Attendee> Attendees { get; set; }
    }
}