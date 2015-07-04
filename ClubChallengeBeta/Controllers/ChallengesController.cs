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
    public class ChallengesController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();
        //
        // GET: /Challenges/
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == id);
            ViewBag.ClubName = currentUser.Club.Name;
            var teamChallenges = db.TeamChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2).Include(s => s.AspNetUser3).Include(s => s.AspNetUser4);
            teamChallenges = teamChallenges.Where(e => e.AspNetUser.Id == id || e.AspNetUser1.Id == id || e.AspNetUser2.Id == id || e.AspNetUser3.Id == id);

            var teamChallengesView = teamChallenges.ToList().Select(e => new TeamChallengesViewModel(e, currentUser));



            var singleChallenges = db.SingleChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            singleChallenges = singleChallenges.Where(e => e.AspNetUser.Id == id || e.AspNetUser1.Id == id);
            var singleChallengesView = singleChallenges.ToList().Select(e => new SingleChallengesViewModel(e, currentUser));
            var result = new MyChallengesViewModel();
            result.TeamChallenges = teamChallengesView;
            result.SingleChallenges = singleChallengesView;
            return View(result);
        }


        // GET: /SingleChallenges/
        public ActionResult WaitingApproval()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.AspNetUsers.Find(currentUserId);
            var currentUserClubId = currentUser.Club.ClubId;
            var singleChallenges = db.SingleChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            singleChallenges = singleChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId && s.Result == "Waiting approval");
            var singleChallengesView = singleChallenges.ToList().Select(e => new SingleChallengesViewModel(e, currentUser));
            var teamChallenges = db.TeamChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            teamChallenges = teamChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId && s.Result == "Waiting approval");
            var teamChallengesView = teamChallenges.ToList().Select(e => new TeamChallengesViewModel(e, currentUser));
            var result = new MyChallengesViewModel();
            result.TeamChallenges = teamChallengesView;
            result.SingleChallenges = singleChallengesView;
            return View(result);
        }
	}
}