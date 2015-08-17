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
using BotDetect.Web.UI.Mvc;
using ConferenceManagementSystem.Utilities;

namespace ConferenceManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Register/
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Country).Include(u => u.Gender).Include(u => u.Title);
            return View(users.ToList());
        }

        // GET: /Register/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /Register/Create
        public ActionResult Create()
        {

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name");
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name");
            ViewBag.TitleId = new SelectList(db.Titles, "TitleId", "Name");
            return View();
        }

        // POST: /Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "SampleCaptcha", "Incorrect CAPTCHA code!")] //validate captchaCode
        public ActionResult Create([Bind(Include = "UserId,Email,Username,Password,ConfirmedPassword,TitleId,FullName,GenderId,Instituition,Faculty,Department,ResearchField,Address,State,PostalCode,CountryId,PhoneNumber,FaxNumber,RegDate")] User user)
        {
            if (ModelState.IsValid)
            {
                user.encryptedPassword = Function.Encrypt(user.Password);
                user.RegDate = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index","Main");
            }
            else
            {
                TempData["msg"] = "The captcha is incorrect";

            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name", user.CountryId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", user.GenderId);
            ViewBag.TitleId = new SelectList(db.Titles, "TitleId", "Name", user.TitleId);
            return View(user);
        }

        // GET: /Register/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name", user.CountryId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", user.GenderId);
            ViewBag.TitleId = new SelectList(db.Titles, "TitleId", "Name", user.TitleId);
            return View(user);
        }

        // POST: /Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,Username,encryptedPassword,TitleId,FullName,GenderId,Instituition,Faculty,Department,ResearchField,Address,State,PostalCode,CountryId,PhoneNumber,FaxNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name", user.CountryId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", user.GenderId);
            ViewBag.TitleId = new SelectList(db.Titles, "TitleId", "Name", user.TitleId);
            return View(user);
        }

        // GET: /Register/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
