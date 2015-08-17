using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConferenceManagementSystem.Models
{
    [Table("Paper")]
    public class Paper
    {
        [Key]
        public int PaperId { get; set; }

        public int ConferenceId { get; set; }
        public virtual Conference conference { get; set; }

        public int UserId { get; set; }
        public virtual User user { get; set; }

        public string PaperTitle { get; set; }
        public string AuthorList { get; set; }
        public string Co_Author { get; set; }
        public string Affiliation { get; set; }
        public string Presenter { get; set; }
        public string Abstract { get; set; }
        public string PaperDescription { get; set; }
        public byte[] AbstractFile { get; set; }
        public string Keywords { get; set; }
        public int TopicId { get; set; }
        public DateTime AbstractSubmissionDate { get; set; }
        
        public int AbstractSubmissionNotification { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    
    }
}