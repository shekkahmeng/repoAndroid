using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Title")]
    public class Title
    {
        [Key]
        public int TitleId { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}