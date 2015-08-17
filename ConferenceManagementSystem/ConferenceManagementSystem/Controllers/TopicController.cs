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
    public class TopicController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Topic/
        public ActionResult Index()
        {
            var topics = db.Topics.Include(t => t.conference);
            return View(topics.ToList());
        }

        // GET: /Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: /Topic/Create
        public ActionResult Create()
        {
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username");
            return View();
        }

        // POST: /Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicId,Name,Order,ConferenceId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", topic.ConferenceId);
            return View(topic);
        }

        // GET: /Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", topic.ConferenceId);
            return View(topic);
        }

        // POST: /Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Name,Order,ConferenceId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", topic.ConferenceId);
            return View(topic);
        }

        // GET: /Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: /Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
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
