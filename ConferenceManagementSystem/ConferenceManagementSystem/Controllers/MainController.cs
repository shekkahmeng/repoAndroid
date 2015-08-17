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
using System.Text;

namespace ConferenceManagementSystem.Controllers
{
    public class MainController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Main/
        public ActionResult Index()
        {
            return View(db.Conferences.ToList());
        }

        public ActionResult Home(int? id)
        {
            int conferenceId = id.GetValueOrDefault();
            Content content = db.Contents.FirstOrDefault(u => u.ConferenceId == conferenceId);

            var builder = new StringBuilder();
            if (content.WelcomeText != null)
            {
                builder.AppendLine(content.WelcomeText.ToString());
                ViewBag.welcomeText = builder.ToString();
            }
            else
            {
                ViewBag.welcomeText = null;
            }
            TempData["Data"] = Session["sessionLoggedInUserId"];
            TempData["ConferenceId"] = conferenceId;
            return View();
        }

        // GET: /Main/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // GET: /Main/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Main/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConferenceId,Username,encryptedPassword,OrganizerName,Website,Date,ContactName,Contact,PaperPrefix,LinkDirectory,LoggedIn,Logo,Short_Name,ChairmanName,ChairmanEmail,ConferencePhone,SystemEmail,SecretariatAddress,ConferenceTime,ConferenceVenue")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                db.Conferences.Add(conference);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conference);
        }

        // GET: /Main/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: /Main/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConferenceId,Username,encryptedPassword,OrganizerName,Website,Date,ContactName,Contact,PaperPrefix,LinkDirectory,LoggedIn,Logo,Short_Name,ChairmanName,ChairmanEmail,ConferencePhone,SystemEmail,SecretariatAddress,ConferenceTime,ConferenceVenue")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conference).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(conference);
        }

        // GET: /Main/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: /Main/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conference conference = db.Conferences.Find(id);
            db.Conferences.Remove(conference);
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
