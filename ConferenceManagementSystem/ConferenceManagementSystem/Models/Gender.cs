using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Gender")]
    public class Gender
    {
        public int GenderId { get; set; }

        [Display(Name = "Gender")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}