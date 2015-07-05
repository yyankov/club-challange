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

    [Authorize]
    public class ClubController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        public bool IsClubUser(string userid)
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

        public ActionResult Index()
        {
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            var clubs = db.Clubs.ToList();
            foreach (var club in clubs)
            {
                if (club.AspNetUser == null)
                {
                    club.AspNetUser = db.AspNetUsers.SingleOrDefault(e => e.Id == club.OwnerId.Remove(club.OwnerId.Length - 3));
                }
            }
            //TODO: check what broke?! Guid comming from db with extra \t
            var clubSearch = new ClubSearchViewModel();
            clubSearch.Clubs = clubs.Select(e => new ClubViewModel(e, currentUser)).ToList(); ;

            clubSearch.ClubUser = IsClubUser(User.Identity.GetUserId());
            clubSearch.HasClub = currentUser.Club != null;
            ViewBag.ClubUser = IsClubUser(User.Identity.GetUserId());
            return View(clubSearch);
        }


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
            var result = new ClubViewModel(club, currentUser);
            result.Users = club.AspNetUsers.Select(e => new UserViewModel(e)).OrderByDescending(e => e.Trophies + e.TeamTrophies).ToList();
            result.SinglesLeaders = club.AspNetUsers.Select(e => new UserViewModel(e)).OrderByDescending(e => e.Score).ToList();
            result.TeamLeaders = club.AspNetUsers.Select(e => new UserViewModel(e)).OrderByDescending(e => e.TeamScore).ToList();
            return View(result);
        }


        [Authorize(Roles = "Club")]
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
                    club.ImageMimeType = "image/png";
                }
                var userId = User.Identity.GetUserId();
                club.OwnerId = userId;
                var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == userId);
                db.Clubs.Add(club);
                currentUser.Club = club;
                db.Clubs.Add(club);
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }


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

        [HttpPost]
        [Authorize(Roles = "Club")]
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


        [Authorize(Roles = "Club")]
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

        [Authorize(Roles = "Club")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Club club = db.Clubs.Find(id);
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            currentUser.ClubId = null;
            db.Entry(currentUser).State = EntityState.Modified;
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Gallery(int id)
        {
            var club = db.Clubs.Find(id);
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            ViewBag.ClubName = club.Name;
            ViewBag.CanAddImage = club.OwnerId == currentUser.Id;
            ViewBag.Images = club.ClubImages.Select(e => new ClubImageViewModel()
            {
                ClubImageId = e.ClubImageId
            });
            var clubImage = new ClubImage();
            clubImage.ClubId = id;
            return View(clubImage);
        }

        [HttpPost]
        [Authorize(Roles = "Club")]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage(ClubImage clubImage, HttpPostedFileBase image)
        {
            if (image != null)
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());

                clubImage.ImageMimeType = image.ContentType;
                clubImage.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(clubImage.ImageData, 0, image.ContentLength);
                db.ClubImages.Add(clubImage);
                db.SaveChanges();
            }
            return RedirectToAction("Gallery", new { id = clubImage.ClubId });
        }

        public FileContentResult GetClubImage(int id)
        {
            var clubImage = db.ClubImages.Find(id);
            if (clubImage != null)
            {
                return File(clubImage.ImageData, clubImage.ImageMimeType);
            }
            else { return null; }
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

        [Authorize(Roles = "Club")]
        public ActionResult ResetSinglesPoints(string id)
        {

            var user = db.AspNetUsers.SingleOrDefault(e => e.Id == id);
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());

                if (user.ClubId != currentUser.ClubId)
                { }
                else
                {
                    user.Score = 0;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Details", new { id = user.ClubId });
        }

        [Authorize(Roles = "Club")]
        public ActionResult ResetSinglesBoard(int id)
        {
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (id != currentUser.ClubId)
                { }
                else
                {
                    var club = db.Clubs.SingleOrDefault(e => e.ClubId == id);
                    foreach (var user in club.AspNetUsers)
                    {
                        user.Score = 0;
                    }
                    db.Entry(club).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Club")]
        public ActionResult GiveSinglesReward(int id)
        {
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (id != currentUser.ClubId)
                { }
                else
                {
                    var club = db.Clubs.SingleOrDefault(e => e.ClubId == id);
                    club.AspNetUsers.OrderByDescending(e => e.Score).FirstOrDefault().Trophies++;
                    foreach (var user in club.AspNetUsers)
                    {
                        user.Score = 0;
                    }
                    db.Entry(club).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Club")]
        public ActionResult ResetTeamPoints(string id)
        {

            var user = db.AspNetUsers.SingleOrDefault(e => e.Id == id);
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());

                if (user.ClubId != currentUser.ClubId)
                { }
                else
                {
                    user.TeamScore = 0;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Details", new { id = user.ClubId });
        }

        [Authorize(Roles = "Club")]
        public ActionResult ResetTeamBoard(int id)
        {
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (id != currentUser.ClubId)
                { }
                else
                {
                    var club = db.Clubs.SingleOrDefault(e => e.ClubId == id);
                    foreach (var user in club.AspNetUsers)
                    {
                        user.TeamScore = 0;
                    }
                    db.Entry(club).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Club")]
        public ActionResult GiveTeamReward(int id)
        {
            try
            {
                var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (id != currentUser.ClubId)
                { }
                else
                {
                    var club = db.Clubs.SingleOrDefault(e => e.ClubId == id);
                    club.AspNetUsers.OrderByDescending(e => e.Score).FirstOrDefault().TeamTrophies++;
                    foreach (var user in club.AspNetUsers)
                    {
                        user.TeamScore = 0;
                    }
                    db.Entry(club).State = EntityState.Modified;
                    db.SaveChanges();
                }
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
