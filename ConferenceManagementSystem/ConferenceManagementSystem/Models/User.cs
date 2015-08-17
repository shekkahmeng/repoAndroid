using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please fill-in Email.")]
        [EmailAddress(ErrorMessage = "This email address is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill-in Username.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d+)[a-zA-Z0-9]{6,15}$", ErrorMessage = "Username must be 6-15 characters, and include letters and numbers.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please fill in Password.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please fill in Confirm Password.")]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password", ErrorMessage = "The password fields did not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmedPassword { get; set; }

        public string encryptedPassword { get; set; }

        [ForeignKey("Title")]
        [Required(ErrorMessage = "Please Select One.")]
        [Display(Name = "Title")]
        public int TitleId { get; set; }

        [Required(ErrorMessage = "Please fill-in FullName.")]
        public string FullName { get; set; }

        [ForeignKey("Gender")]
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please Select One.")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Please fill-in Instituition.")]
        public string Instituition { get; set; }

        public string Faculty { get; set; }

        public string Department { get; set; }

        public string ResearchField { get; set; }

        [Required(ErrorMessage = "Please fill-in Address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please fill-in State.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please fill-in PostalCode.")]
        public int PostalCode { get; set; }

        [ForeignKey("Country")]
        [Required(ErrorMessage = "Please Select One.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegDate { get; set; }

        public virtual Title Title { get; set; }
        public virtual Country Country { get; set; }
        public virtual Gender Gender { get; set; }
        public ICollection<Attendee> Attendees { get; set; }
        public ICollection<Paper> Paper { get; set; }

        public bool LoggedIn { get; set; }
    }
}