﻿using System;
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
    public class ClubController : Controller
    {
        private ClubChallengeEntities db = new ClubChallengeEntities();

        // GET: /Clubs/
        public ActionResult Index()
        {
            return View(db.Clubs.ToList());
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
            return View(club);
        }

        // GET: /Clubs/Create
        public ActionResult Create()
        {
            return View(new Club());
        }

        // POST: /Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ClubId,OwnerId,Name,Text,ImageData,ImageMimeType")] Club club)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Clubs.Add(club);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(club);
        //}

        [HttpPost]
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
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }
        // GET: /Clubs/Edit/5
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
            return View(club);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
