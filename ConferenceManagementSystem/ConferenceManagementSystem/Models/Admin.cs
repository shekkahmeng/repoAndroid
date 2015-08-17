using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public bool LoggedIn { get; set; }
    }
}