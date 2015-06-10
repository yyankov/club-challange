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

namespace ClubChallengeBeta.Controllers
{
    public class SingleChallengesController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /SingleChallenges/
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            AspNetUser currentUser = db.AspNetUsers.Find(currentUserId);
            int currentUserClubId = currentUser.Club.ClubId;
            var singleChallenges = db.SingleChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            singleChallenges = singleChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId);
            return View(singleChallenges.ToList());
        }

        // GET: /SingleChallenges/
        public ActionResult MyChallenges()
        {
            string id = User.Identity.GetUserId();
            var singlechallenges = db.SingleChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            singlechallenges = singlechallenges.Where(e => e.AspNetUser.Id == id || e.AspNetUser1.Id == id);
            return View(singlechallenges.ToList());
        }
        public ActionResult Reject(int id)
        {
            SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            singleChallenge.Confirmed=true;
            singleChallenge.WinnerId = singleChallenge.AspNetUser.Id;
            singleChallenge.Result = "Rejected";
            singleChallenge.AspNetUser.Score++;
            singleChallenge.AspNetUser1.Refusals++;
            if (singleChallenge.AspNetUser1.Refusals == 5)
            {
                singleChallenge.AspNetUser1.Score = 0;
                singleChallenge.AspNetUser1.Refusals = 0;
            }
            db.Entry(singleChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyChallenges");
        }
        public ActionResult Accept(int id)
        {
            SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            singleChallenge.User2Accepted = true;
            singleChallenge.Result = "Accepted";
            db.Entry(singleChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyChallenges");
        }
        // GET: /SingleChallenges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SingleChallenge singlechallenge = db.SingleChallenges.Find(id);
            if (singlechallenge == null)
            {
                return HttpNotFound();
            }
            return View(singlechallenge);
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
