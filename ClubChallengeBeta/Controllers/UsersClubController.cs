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

namespace ClubChallengeBeta.Controllers
{
    public class UsersClubController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /UsersClub/
        public ActionResult Index()
        {
            var currentUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            var aspNetUsers = db.AspNetUsers.Include(a => a.Club).Where(e => e.ClubId == currentUser.ClubId);
            var users = aspNetUsers.Select(e => new SimpleUserViewModel()
            {
                UserId = e.Id,
                UserName = e.UserName,
                Phone = e.PhoneNumber,
                Email = e.Email,
                IsOwner = e.Club.OwnerId == e.Id
            });
            ViewBag.isOwner = currentUser.Id == currentUser.Club.OwnerId;
            return View(users);
        }

        // GET: /UsersClub/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(new UserDetailsViewModel(aspNetUser, db.AspNetUsers.Find(User.Identity.GetUserId())));
        }


        public ActionResult Challenge(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == currentUserId) { }
            else
            {
                try
                {
                    var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == currentUserId);
                    var challengedUser = db.AspNetUsers.SingleOrDefault(e => e.Id == id);
                    var sChallenge = new SingleChallenge();
                    sChallenge.User1Id = currentUserId;
                    sChallenge.User2Id = id;
                    sChallenge.User1Accepted = true;
                    sChallenge.User2Accepted = false;
                    sChallenge.DateCreated = DateTime.Now;
                    db.SingleChallenges.Add(sChallenge);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Index", "Challenges");
        }

        // GET: /UsersClub/
        [HttpGet]
        public ActionResult MultiChallenge(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == currentUserId);
            var users = db.AspNetUsers
                .Where(e => e.Id != currentUserId && e.ClubId == currentUser.ClubId)
                .Select(e => new SelectListItem
            {
                Text = e.UserName,
                Value = e.Id
            });
            ViewBag.users = users;
            var challenge = new MultiChallengeViewModel();
            challenge.PartnerId = id;
            return PartialView("_MultiChallenge", challenge);
        }


        [HttpPost]
        public ActionResult MultiChallenge(MultiChallengeViewModel mc)
        {
            var currentUserId = User.Identity.GetUserId();
            if (mc.PartnerId == currentUserId || mc.Opponent1Id == currentUserId || mc.Opponent2Id == currentUserId) { }
            else
            {
                try
                {
                    var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == currentUserId);
                    var tChallenge = new TeamChallenge();
                    tChallenge.User1Id = currentUserId;
                    tChallenge.User2Id = mc.PartnerId;
                    tChallenge.User3Id = mc.Opponent1Id;
                    tChallenge.User4Id = mc.Opponent2Id;
                    tChallenge.User1Accepted = true;
                    tChallenge.User2Accepted = false;
                    tChallenge.User3Accepted = false;
                    tChallenge.User4Accepted = false;
                    tChallenge.DateCreated = DateTime.Now;
                    db.TeamChallenges.Add(tChallenge);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Index", "Challenges");
        }



        public ActionResult Kick(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == currentUserId) { }
            else
            {
                try
                {
                    var kickedUser = db.AspNetUsers.SingleOrDefault(e => e.Id == id);
                    kickedUser.ClubId = null;
                    db.Entry(kickedUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {

                }
            }
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
