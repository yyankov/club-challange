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
    public class UsersClubController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /UsersClub/
        public ActionResult Index()
        {
            var currentUSer = db.AspNetUsers.Find(User.Identity.GetUserId());
            var aspnetusers = db.AspNetUsers.Include(a => a.Club).Where(e => e.ClubId == currentUSer.ClubId);
            return View(aspnetusers.ToList());
        }

        // GET: /UsersClub/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            return View(aspnetuser);
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
