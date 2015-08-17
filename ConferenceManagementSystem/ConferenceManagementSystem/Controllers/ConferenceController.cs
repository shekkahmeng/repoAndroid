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
    public class ConferenceController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();

        // GET: /Conference/
        public ActionResult Index()
        {
            return View(db.Conferences.ToList());
        }

        public ActionResult Menu(int? id)
        {
            Conference conference = db.Conferences.Find(id);
            return View(conference);
        }

        public ActionResult ConferenceDetail(int? id)
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
            conference.PhotoString = "data:image/png;base64," + Convert.ToBase64String(conference.Logo);
            conference.Password = conference.encryptedPassword;
            conference.ConfirmedPassword = conference.encryptedPassword;
            return View(conference);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ConferenceDetail([Bind(Include = "ConferenceId,LogoByte,Username,encryptedPassword,Password,ConfirmedPassword,OrganizerName,Website,Date,ContactName,Contact,PaperPrefix,LinkDirectory,LoggedIn,Logo,Short_Name,ChairmanName,ChairmanEmail,ConferencePhone,SystemEmail,SecretariatAddress,ConferenceTime,ConferenceVenue")] Conference conference)
        {
            conference.encryptedPassword = conference.Password;
            if (ModelState.IsValid)
            {
                if (conference.LogoByte != null)
                {
                    if (conference.LogoByte.ContentLength > 0)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(conference.LogoByte.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(conference.LogoByte.ContentLength);
                        }
                        conference.Logo = imageData;
                    }
                }
                db.Entry(conference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu", new { id = conference.ConferenceId });
            }
            return View(conference);
        }

        public ActionResult Fee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int organizerId = id.GetValueOrDefault();   //the int?id is not integer type which cannot store inside OrganizerId column

            var fee = db.Fees.FirstOrDefault(u => u.ConferenceId == id);
            if (fee == null)  //if there is no record in the database, create the data
            {
                List<Fee> record = new List<Fee> { new Fee { Category = "Local Participant", EarlyBird = 0, Normal = 0, ConferenceId = organizerId }, 
                                                   new Fee { Category = "Local Student", EarlyBird = 0, Normal = 0, ConferenceId = organizerId },
                                                   new Fee { Category = "Local Student (Conference & Workshop)", EarlyBird = 0, Normal = 0, ConferenceId = organizerId },
                                                   new Fee { Category = "International Participant", EarlyBird = 0, Normal = 0, ConferenceId = organizerId },
                                                   new Fee { Category = "International Student", EarlyBird = 0, Normal = 0, ConferenceId = organizerId },
                                                   new Fee { Category = "International Student (Conference & Workshop)", EarlyBird = 0, Normal = 0, ConferenceId = organizerId },};
                foreach (var i in record)
                {
                    db.Fees.Add(i);
                    db.SaveChanges();
                }
            }

            var fees = db.Fees.Where(u => u.ConferenceId == id);

            return View(fees.ToList());
        }

        // GET: /Conference/Details/5
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

        // GET: /Conference/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Conference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConferenceId,Username,Password,ConfirmedPassword,OrganizerName,Website,Date,ContactName,Contact,PaperPrefix,LinkDirectory,LoggedIn,SystemEmail")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                conference.encryptedPassword = Utilities.Function.Encrypt(conference.Password);
                db.Conferences.Add(conference);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conference);
        }

        // GET: /Conference/Edit/5
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
            conference.Password = Utilities.Function.Decrypt(conference.encryptedPassword);
            conference.ConfirmedPassword = Utilities.Function.Decrypt(conference.encryptedPassword);
            return View(conference);
        }

        // POST: /Conference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConferenceId,Username,encryptedPassword,Password,ConfirmedPassword,OrganizerName,Website,Date,ContactName,Contact,PaperPrefix,LinkDirectory,LoggedIn,Logo,Short_Name,ChairmanName,ChairmanEmail,ConferencePhone,SystemEmail,SecretariatAddress,ConferenceTime,ConferenceVenue")] Conference conference)
        {
            conference.encryptedPassword = Utilities.Function.Encrypt(conference.Password);
            if (ModelState.IsValid)
            {
                db.Entry(conference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conference);
        }

        // GET: /Conference/Delete/5
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

        // POST: /Conference/Delete/5
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
