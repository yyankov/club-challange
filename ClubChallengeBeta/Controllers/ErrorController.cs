using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClubChallengeBeta.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NoPermissions()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
	}
}