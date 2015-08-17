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
using System.Net.Mail;
using System.Text;

namespace ConferenceManagementSystem.Controllers
{
    public class AttendeeController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Attendee/
        public ActionResult Index()
        {
            var attendees = db.Attendees.Include(a => a.conference).Include(a => a.fee).Include(a => a.user).Include(a => a.usertype);
            return View(attendees.ToList());
        }

        // GET: /Attendee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // GET: /Attendee/Create
        public ActionResult Create(int? id)
        {
            int conferenceId = id.GetValueOrDefault();
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username");
            ViewBag.FeeId = new SelectList(db.Fees, "FeeId", "Category");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name");
            TempData["ConferenceId"] = conferenceId;
            TempData["UserId"] = (int)Session["sessionLoggedInUserId"];
            return View();
        }

        // POST: /Attendee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AttendeeId,ConferenceId,FeeId,UserId,UserTypeId")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Attendees.Add(attendee);
                db.SaveChanges();

                var conference = db.Conferences.FirstOrDefault(u => u.ConferenceId == attendee.ConferenceId);
                var user = db.Users.FirstOrDefault(u => u.UserId == attendee.UserId);
                var emailMessage = db.NotificationEmails.FirstOrDefault(u => u.ConferenceId == attendee.ConferenceId);
                var builder = new StringBuilder();

                if (attendee.UserTypeId == 1)
                {
                    builder.AppendLine(emailMessage.ParticipantRegistration.ToString());
                }
                else
                {
                    builder.AppendLine(emailMessage.PresenterRegistration.ToString());
                }

                var message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(conference.SystemEmail);
                message.To.Add(new MailAddress(user.Email));
                //message.CC.Add(new MailAddress("swcmspv2@gmail.com"));
                message.Subject = "Conference";
                message.Body = builder.ToString();
                SmtpClient client = new SmtpClient();
                client.Send(message);
                return RedirectToAction("Home", "Main", new { id = attendee.ConferenceId });
            }

            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", attendee.ConferenceId);
            ViewBag.FeeId = new SelectList(db.Fees, "FeeId", "Category", attendee.FeeId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", attendee.UserId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", attendee.UserTypeId);
            return View(attendee);
        }

        // GET: /Attendee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int conferenceId = id.GetValueOrDefault();
            Attendee attendee = db.Attendees.FirstOrDefault(u => u.ConferenceId == conferenceId);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", attendee.ConferenceId);
            ViewBag.FeeId = new SelectList(db.Fees, "FeeId", "Category", attendee.FeeId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", attendee.UserId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", attendee.UserTypeId);
            return View(attendee);
        }

        // POST: /Attendee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AttendeeId,ConferenceId,FeeId,UserId,UserTypeId")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", attendee.ConferenceId);
            ViewBag.FeeId = new SelectList(db.Fees, "FeeId", "Category", attendee.FeeId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", attendee.UserId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", attendee.UserTypeId);
            return View(attendee);
        }

        // GET: /Attendee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: /Attendee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendee attendee = db.Attendees.Find(id);
            db.Attendees.Remove(attendee);
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
