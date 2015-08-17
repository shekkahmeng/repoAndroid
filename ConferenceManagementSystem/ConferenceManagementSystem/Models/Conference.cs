using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Conference")]
    public class Conference
    {
        [Key]
        public int ConferenceId { get; set; }

        [Required]
        public string Username { get; set; }

        public string encryptedPassword { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password fields did not match")]
        public string ConfirmedPassword { get; set; }

        [Required]
        [Display(Name = "Conference Name")]
        public string OrganizerName { get; set; }

        [Required]
        [Display(Name = "Conference Website")]
        public string Website { get; set; }

        [Display(Name = "Conference Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Required]
        [Display(Name = "Contact /Telephone No /Email")]
        public string Contact { get; set; }

        [Required]
        [Display(Name = "Paper Prefix")]
        public string PaperPrefix { get; set; }

        [Required]
        [Display(Name = "Link Directory")]
        public string LinkDirectory { get; set; }

        public bool LoggedIn { get; set; }

        public byte[] Logo { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoByte { get; set; }

        [NotMapped]
        public string PhotoString { get; set; }

        public string Short_Name { get; set; }

        public string ChairmanName { get; set; }

        public string ChairmanEmail { get; set; }

        public string ConferencePhone { get; set; }

        [Required]
        public string SystemEmail { get; set; }

        public string SecretariatAddress { get; set; }

        public string ConferenceTime { get; set; }

        public string ConferenceVenue { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<DateDealine> DateDealines { get; set; }
        public virtual ICollection<NotificationEmail> NotificationEmails { get; set; }
        public ICollection<Attendee> Attendees { get; set; }
        public virtual ICollection<Paper> Papers { get; set; }

        //[ForeignKey("Title")]
        //public int TitleId { get; set; }

        //public virtual Title Title { get; set; }
    }
}