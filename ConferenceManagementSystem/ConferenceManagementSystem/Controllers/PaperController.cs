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
using System.IO;

namespace ConferenceManagementSystem.Controllers
{
    public class PaperController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Paper/
        public ActionResult Index()
        {
            var papers = db.Papers.Include(p => p.conference).Include(p => p.user);
            return View(papers.ToList());
        }

        // GET: /Paper/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // GET: /Paper/Create
        public ActionResult Create(int? id)
        {
            int conferenceId = id.GetValueOrDefault();
            Paper exist = db.Papers.FirstOrDefault(u => u.ConferenceId == conferenceId);
            if (exist == null)
            {
                var paper = new Paper();
                paper.ConferenceId = conferenceId;
                paper.UserId = (int)Session["sessionLoggedInUserId"];
                paper.AbstractSubmissionDate = DateTime.Now;
                db.Papers.Add(paper);
                db.SaveChanges();
                return View(paper);
            }
            return View(exist);
        }

        // POST: /Paper/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PaperId,ConferenceId,UserId,PaperTitle,AuthorList,Co_Author,Affiliation,Presenter,Abstract,PaperDescription,AbstractFile,Keywords,TopicId,AbstractSubmissionDate,AbstractSubmissionNotification")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                string filePath = FileUrl(paper.Abstract);
                paper.Abstract = filePath;
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", paper.ConferenceId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", paper.UserId);
            return View(paper);
        }

        // GET: /Paper/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int conferenceId = id.GetValueOrDefault();
            Paper paper = db.Papers.FirstOrDefault(u => u.ConferenceId == conferenceId);
            if (paper == null)
            {
                ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username");
                ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
                return View();
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", paper.ConferenceId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", paper.UserId);
            return View(paper);
        }

        // POST: /Paper/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PaperId,ConferenceId,UserId,PaperTitle,AuthorList,Co_Author,Affiliation,Presenter,Abstract,PaperDescription,AbstractFile,Keywords,TopicId,AbstractSubmissionDate,AbstractSubmissionNotification")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                string imgPath = FileUrl(paper.Abstract);
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConferenceId = new SelectList(db.Conferences, "ConferenceId", "Username", paper.ConferenceId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", paper.UserId);
            return View(paper);
        }

        private string FileUrl(string filePath)
        {
            string wordFilePath = filePath;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Paper/WordFile"), fileName);

                    wordFilePath = HttpUtility.UrlPathEncode("/Paper/WordFile/" + fileName);
                    file.SaveAs(path);
                }
            }

            return wordFilePath;
        }

        // GET: /Paper/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: /Paper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paper paper = db.Papers.Find(id);
            db.Papers.Remove(paper);
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
