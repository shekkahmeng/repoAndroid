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
using ConferenceManagementSystem.Filters;

namespace ConferenceManagementSystem.Controllers
{
    [Authentication]
    public class ContentController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Content/
        public ActionResult Index()
        {
            var contents = db.Contents.Include(c => c.conference);
            return View(contents.ToList());
        }

        // GET: /Content/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: /Content/Create
        public ActionResult Create()
        {
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username");
            return View();
        }

        // POST: /Content/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentId,NewInfo,WelcomeTitle,WelcomeText,Organizer,Accomodation,Committee,CallForPapers,Programmes,ConferenceId")] Content content)
        {
            if (ModelState.IsValid)
            {
                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", content.ConferenceId);
            return View(content);
        }

        // GET: /Content/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int conferenceId = id.GetValueOrDefault();
            Content content = db.Contents.FirstOrDefault(u => u.ConferenceId == conferenceId);
            if (content == null)
            {
                db.Contents.Add(new Content() { ConferenceId = conferenceId });
                db.SaveChanges();
            }
            content = db.Contents.FirstOrDefault(u => u.ConferenceId == conferenceId);
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", content.ConferenceId);
            return View(content);
        }

        // POST: /Content/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentId,NewInfo,WelcomeTitle,WelcomeText,Organizer,Accomodation,Committee,CallForPapers,Programmes,ConferenceId")] Content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu", "Conference", new { id = content.ConferenceId });
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", content.ConferenceId);
            return View(content);
        }

        // GET: /Content/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: /Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
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
