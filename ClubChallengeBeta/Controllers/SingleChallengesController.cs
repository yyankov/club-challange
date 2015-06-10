using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubChallengeBeta.App_Data;

namespace ClubChallengeBeta.Controllers
{
    public class SingleChallengesController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /SingleChallenges/
        public ActionResult Index()
        {
            var singlechallenges = db.SingleChallenges.Include(s => s.AspNetUser).Include(s => s.AspNetUser1).Include(s => s.AspNetUser2);
            return View(singlechallenges.ToList());
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
