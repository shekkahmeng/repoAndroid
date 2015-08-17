using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConferenceManagementSystem.Models;
using ConferenceManagementSystem.DataAccessLayer;

namespace ConferenceManagementSystem.Controllers
{
    public class NotificationEmailController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /NotificationEmail/
        public ActionResult Index()
        {
            var notificationemails = db.NotificationEmails.Include(n => n.conference);
            return View(notificationemails.ToList());
        }

        // GET: /NotificationEmail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationemail = db.NotificationEmails.Find(id);
            if (notificationemail == null)
            {
                return HttpNotFound();
            }
            return View(notificationemail);
        }

        // GET: /NotificationEmail/Create
        public ActionResult Create()
        {
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username");
            return View();
        }

        // POST: /NotificationEmail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmailId,PresenterRegistration,ParticipantRegistration,ParticipantConfirmation,AbstractSubmission,AbstractAcceptance,AbstractRejection,FullPaperSubmission,PaperAcceptance,PaperRejection,CameraReadyPaper,ReviewerInvitation,PaperForReviewing,FinishReview,UserInvitation,ConferenceId")] NotificationEmail notificationemail)
        {
            if (ModelState.IsValid)
            {
                db.NotificationEmails.Add(notificationemail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", notificationemail.ConferenceId);
            return View(notificationemail);
        }

        // GET: /NotificationEmail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int conferenceId = id.GetValueOrDefault();
            NotificationEmail notificationemail = db.NotificationEmails.FirstOrDefault(u => u.ConferenceId == conferenceId);
            if (notificationemail == null)
            {
                db.NotificationEmails.Add(new NotificationEmail() { ConferenceId = conferenceId });
                db.SaveChanges();
            }
            notificationemail = db.NotificationEmails.FirstOrDefault(u => u.ConferenceId == conferenceId);
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", notificationemail.ConferenceId);
            return View(notificationemail);
        }

        // POST: /NotificationEmail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmailId,PresenterRegistration,ParticipantRegistration,ParticipantConfirmation,AbstractSubmission,AbstractAcceptance,AbstractRejection,FullPaperSubmission,PaperAcceptance,PaperRejection,CameraReadyPaper,ReviewerInvitation,PaperForReviewing,FinishReview,UserInvitation,ConferenceId")] NotificationEmail notificationemail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notificationemail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu", "Conference", new { id = notificationemail.ConferenceId });
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", notificationemail.ConferenceId);
            return View(notificationemail);
        }

        // GET: /NotificationEmail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationemail = db.NotificationEmails.Find(id);
            if (notificationemail == null)
            {
                return HttpNotFound();
            }
            return View(notificationemail);
        }

        // POST: /NotificationEmail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotificationEmail notificationemail = db.NotificationEmails.Find(id);
            db.NotificationEmails.Remove(notificationemail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
