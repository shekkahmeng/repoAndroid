using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceManagementSystem.Models
{
    [Table("Content")]
    public class Content
    {
        [Key]
        public int ContentId { get; set; }

        [UIHint("tinymce_full")]
        [AllowHtml]
        public string NewInfo { get; set; }

        [AllowHtml]
        public string WelcomeTitle { get; set; }

        [AllowHtml]
        public string WelcomeText { get; set; }

        [AllowHtml]
        public string Organizer { get; set; }

        [AllowHtml]
        public string Accomodation { get; set; }

        [AllowHtml]
        public string Committee { get; set; }

        [AllowHtml]
        public string CallForPapers { get; set; }

        [AllowHtml]
        public string Programmes { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference conference { get; set; }
    }
}