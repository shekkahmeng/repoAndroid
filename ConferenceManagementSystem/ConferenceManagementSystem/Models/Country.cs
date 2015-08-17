using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}