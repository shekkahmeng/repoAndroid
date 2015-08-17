using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceManagementSystem.Models
{
    [Table("NotificationEmail")]
    public class NotificationEmail
    {
        [Key]
        public int EmailId { get; set; }

        [AllowHtml]
        public string PresenterRegistration { get; set; }

        [AllowHtml]
        public string ParticipantRegistration { get; set; }

        [AllowHtml]
        public string ParticipantConfirmation { get; set; }

        [AllowHtml]
        public string AbstractSubmission { get; set; }

        [AllowHtml]
        public string AbstractAcceptance { get; set; }

        [AllowHtml]
        public string AbstractRejection { get; set; }

        [AllowHtml]
        public string FullPaperSubmission { get; set; }

        [AllowHtml]
        public string PaperAcceptance { get; set; }

        [AllowHtml]
        public string PaperRejection { get; set; }

        [AllowHtml]
        public string CameraReadyPaper { get; set; }

        [AllowHtml]
        public string ReviewerInvitation { get; set; }

        [AllowHtml]
        public string PaperForReviewing { get; set; }

        [AllowHtml]
        public string FinishReview { get; set; }

        [AllowHtml]
        public string UserInvitation { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference conference { get; set; }
    }
}