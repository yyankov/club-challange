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
    public class TeamChallengesController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /SingleChallenges/
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            AspNetUser currentUser = db.AspNetUsers.Find(currentUserId);
            int currentUserClubId = currentUser.Club.ClubId;
            var teamChallenges = db.TeamChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2).Include(s => s.AspNetUser3).Include(s => s.AspNetUser4);
            teamChallenges = teamChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId);
            return View(teamChallenges.ToList());
        }


        public ActionResult Reject(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            TeamChallenge teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            teamChallenge.Confirmed = true;
            teamChallenge.Result = "Rejected";
            if (currentUserId == teamChallenge.User2Id)
            {
                teamChallenge.Winner1Id = teamChallenge.User3Id;
                teamChallenge.AspNetUser1.TeamRefusals++;
                if (teamChallenge.AspNetUser1.TeamRefusals == 5)
                {
                    teamChallenge.AspNetUser1.TeamScore = 0;
                    teamChallenge.AspNetUser1.TeamRefusals = 0;
                }

                teamChallenge.AspNetUser2.TeamScore++;
                teamChallenge.AspNetUser3.TeamScore++;
            }
            else if (currentUserId == teamChallenge.User3Id)
            {
                teamChallenge.Winner1Id = teamChallenge.User1Id;
                teamChallenge.AspNetUser2.TeamRefusals++;
                if (teamChallenge.AspNetUser2.TeamRefusals == 5)
                {
                    teamChallenge.AspNetUser2.TeamScore = 0;
                    teamChallenge.AspNetUser2.TeamRefusals = 0;
                }
                teamChallenge.AspNetUser.TeamScore++;
                teamChallenge.AspNetUser1.TeamScore++;
            }
            else if (currentUserId == teamChallenge.User4Id)
            {
                teamChallenge.Winner1Id = teamChallenge.User1Id;
                teamChallenge.AspNetUser3.TeamRefusals++;
                if (teamChallenge.AspNetUser3.TeamRefusals == 5)
                {
                    teamChallenge.AspNetUser3.TeamScore = 0;
                    teamChallenge.AspNetUser3.TeamRefusals = 0;
                }
                teamChallenge.AspNetUser.TeamScore++;
                teamChallenge.AspNetUser1.TeamScore++;
            }
            db.Entry(teamChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Challenges");
        }

        public ActionResult Accept(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            TeamChallenge teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            if (currentUserId == teamChallenge.User2Id)
            {
                teamChallenge.User2Accepted = true;
            }
            else if (currentUserId == teamChallenge.User3Id)
            {
                teamChallenge.User3Accepted = true;
            }
            else if (currentUserId == teamChallenge.User4Id)
            {
                teamChallenge.User4Accepted = true;
            }

            if (teamChallenge.User1Accepted && teamChallenge.User2Accepted && teamChallenge.User3Accepted && teamChallenge.User4Accepted)
            {

                teamChallenge.Result = "Accepted";
            }
            db.Entry(teamChallenge).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Challenges");
        }


        [HttpGet]
        public ActionResult ClaimVictory(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            var teamChallengeVM = new TeamChallengesVictoryModel();
            teamChallengeVM.SinglesChallengeId = teamChallenge.TeamChallengeId;
            teamChallengeVM.User1Name = teamChallenge.AspNetUser.UserName;
            teamChallengeVM.User2Name = teamChallenge.AspNetUser1.UserName;
            teamChallengeVM.User3Name = teamChallenge.AspNetUser2.UserName;
            teamChallengeVM.User4Name = teamChallenge.AspNetUser3.UserName;
            var gameScores = new List<GameScore>();
            teamChallengeVM.GameScores = gameScores;
            teamChallengeVM.GameScores.Add(new GameScore());
            teamChallengeVM.GameScores.Add(new GameScore());
            teamChallengeVM.GameScores.Add(new GameScore());
            return PartialView("_ClaimVictory", teamChallengeVM);
        }

        public ActionResult ClaimVictory(int id)
        {
            //var currentUserId = User.Identity.GetUserId();
            //var teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            //if (teamChallenge.User1Id != currentUserId && teamChallenge.User2Id != currentUserId && teamChallenge.User3Id != currentUserId && teamChallenge.User4Id != currentUserId)
            //{
            //}
            //else
            //{
            //    if (teamChallenge.User1Id == currentUserId || teamChallenge.User2Id == currentUserId)
            //    {
            //        teamChallenge.Winner1Id = teamChallenge.User1Id;
            //    }
            //    else
            //    {
            //        teamChallenge.Winner1Id = teamChallenge.User3Id;

            //    }
            //    teamChallenge.Result = "Waiting approval";
            //    db.Entry(teamChallenge).State = EntityState.Modified;
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

        public ActionResult Approve(int id)
        {
            var teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            teamChallenge.Result = "Confirmed";
            teamChallenge.Confirmed = true;

            if (teamChallenge.Winner1Id == teamChallenge.User1Id)
            {
                teamChallenge.AspNetUser.TeamScore++;
                teamChallenge.AspNetUser1.TeamScore++;
            }
            else if (teamChallenge.Winner1Id == teamChallenge.User3Id)
            {
                teamChallenge.AspNetUser2.TeamScore++;
                teamChallenge.AspNetUser3.TeamScore++;
            }
            db.Entry(teamChallenge).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("WaitingApproval", "Challenges");
        }

        public ActionResult Disapprove(int id)
        {
            var teamChallenge = db.TeamChallenges.SingleOrDefault(e => e.TeamChallengeId == id);
            teamChallenge.Result = "Confirmed";
            teamChallenge.Confirmed = true;
            if (teamChallenge.Winner1Id == teamChallenge.User1Id)
            {
                teamChallenge.AspNetUser2.TeamScore++;
                teamChallenge.AspNetUser3.TeamScore++;
            }
            else if (teamChallenge.Winner1Id == teamChallenge.User3Id)
            {
                teamChallenge.AspNetUser.TeamScore++;
                teamChallenge.AspNetUser1.TeamScore++;
            }
            db.Entry(teamChallenge).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("WaitingApproval", "Challenges");
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
