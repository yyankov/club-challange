using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ClubChallengeBeta.App_Data;
using ClubChallengeBeta.Models;
using System.Web.Routing;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace ClubChallengeBeta.Controllers
{
    public class ClubController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        public  bool IsClubUser(string userid)
        {
            var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == userid);
            var clubUser = false;
            foreach (var role in currentUser.AspNetRoles)
            {
                if (role.Name == "Club")
                {
                    clubUser = true;
                }
            }
            return clubUser;
        }
        // GET: /Clubs/
        public ActionResult Index()
        {
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            var clubs = db.Clubs.ToList();
            foreach (var club in clubs)
            {
                if(club.AspNetUser == null)
                {
                    club.AspNetUser = db.AspNetUsers.SingleOrDefault(e => e.Id == club.OwnerId.Remove(club.OwnerId.Length - 3));
                }
            }
            //TODO: check what broke?! Guid comming from db with extra \t
            var clubSearch = new ClubSearchViewModel();
            clubSearch.Clubs = clubs.Select(e => new ClubViewModel(e, currentUser)).ToList();;

            clubSearch.ClubUser = IsClubUser(User.Identity.GetUserId());
            clubSearch.HasClub = currentUser.Club != null;
            ViewBag.ClubUser = IsClubUser(User.Identity.GetUserId());
            return View(clubSearch);
        }

        // GET: /Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());

            ViewBag.CanJoin = currentUser.Club == null;
            return View(new ClubViewModel(club, currentUser));
        }

        // GET: /Clubs/Create
        [Authorize(Roles="Club")]
        public ActionResult Create()
        {
            return View(new Club());
        }

        [HttpPost]
        [Authorize(Roles = "Club")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClubId,OwnerId,Name,Text,ImageData,ImageMimeType")] Club club, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    club.ImageMimeType = image.ContentType;
                    club.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(club.ImageData, 0, image.ContentLength);
                }
                else
                {
                    club.ImageData = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "Content/cicon.png");
                    club.ImageMimeType = "png";
                }
                club.OwnerId = User.Identity.GetUserId();
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        // GET: /Clubs/Edit/5
        [Authorize(Roles = "Club")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: /Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClubId,OwnerId,Name,Text,ImageData,ImageMimeType")] Club club, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    club.ImageMimeType = image.ContentType;
                    club.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(club.ImageData, 0, image.ContentLength);
                }
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        // GET: /Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(new ClubViewModel(club, db.AspNetUsers.Find(User.Identity.GetUserId())));
        }

        // POST: /Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult GetIcon(int id)
        {
            Club club = db.Clubs.Find(id);
            if (club != null)
            {
                return File(club.ImageData, club.ImageMimeType);
            }
            else { return null; }
        }



        public ActionResult Join(int id)
        {
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            if (currentUser.Club != null)
            {
            }
            else
            {
                Club club = db.Clubs.Find(id);
                currentUser.Club = club;
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }


        [HttpGet]
        public ActionResult Leave(int id)
        {
            try
            {
                var uid = User.Identity.GetUserId();
                var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == uid);

                //var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                currentUser.ClubId = null;
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                
            }
            return RedirectToAction("Details", new { id = id });
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
