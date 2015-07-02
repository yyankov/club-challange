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
            return RedirectToAction("Index", "Challenges");
        }

        public ActionResult Approve(int id)
        {
            SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            singleChallenge.Result = "Confirmed";
            singleChallenge.Confirmed=true;
            var winner = db.AspNetUsers.SingleOrDefault(e => e.Id == singleChallenge.WinnerId);
            winner.Score++;
            db.Entry(singleChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WaitingApproval","Challenges");
        }

        public ActionResult Disapprove(int id)
        {
            SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            singleChallenge.Result = "Confirmed";
            singleChallenge.Confirmed = true;
            if (singleChallenge.WinnerId == singleChallenge.User1Id)
            {
                singleChallenge.WinnerId = singleChallenge.User2Id;
                var winner = db.AspNetUsers.SingleOrDefault(e => e.Id == singleChallenge.WinnerId);
                winner.Score++;
            }
            else
            {
                singleChallenge.WinnerId = singleChallenge.User1Id;
                var winner = db.AspNetUsers.SingleOrDefault(e => e.Id == singleChallenge.WinnerId);
                winner.Score++;
            }
            db.Entry(singleChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WaitingApproval", "Challenges");
        }

        public ActionResult Accept(int id)
        {
            SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            singleChallenge.User2Accepted = true;
            singleChallenge.Result = "Accepted";
            db.Entry(singleChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Challenges");
        }


        // GET: /UsersClub/
        [HttpGet]
        public ActionResult ClaimVictory(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            var singleChallengeVM = new SingleChallengesVictoryModel();
            singleChallengeVM.SinglesChallengeId = singleChallenge.SinglesChallengeId;
            singleChallengeVM.User1Name = singleChallenge.AspNetUser.UserName;
            singleChallengeVM.User2Name = singleChallenge.AspNetUser1.UserName;
            var gameScores = new List<GameScore>();
            singleChallengeVM.GameScores = gameScores;
            singleChallengeVM.GameScores.Add(new GameScore());
            singleChallengeVM.GameScores.Add(new GameScore());
            singleChallengeVM.GameScores.Add(new GameScore());
            return PartialView("_ClaimVictory", singleChallengeVM);
        }

        public ActionResult ClaimVictory(SingleChallengesVictoryModel id)
        {
            //string currentUserId = User.Identity.GetUserId();
            //SingleChallenge singleChallenge = db.SingleChallenges.SingleOrDefault(e => e.SinglesChallengeId == id);
            //if (singleChallenge.AspNetUser.Id != currentUserId && singleChallenge.AspNetUser1.Id != currentUserId)
            //{
            //}
            //else
            //{
            //    singleChallenge.WinnerId = currentUserId;
            //    singleChallenge.Result = "Waiting approval";
            //    db.Entry(singleChallenge).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            return RedirectToAction("Index", "Challenges");
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
