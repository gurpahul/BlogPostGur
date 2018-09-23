using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GurpahulBlogggg18.Helpers;
using GurpahulBlogggg18.Models;

namespace GurpahulBlogggg18.Controllers
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.Posts.FirstOrDefault(p => p.Slug == Slug);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }


        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL,Published")] BlogPosts blogPosts, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                
                var Slug = StringUtilites.URLFriendly(blogPosts.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPosts);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPosts);
                }
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPosts.MediaUrl = "/Uploads/" + fileName;
                }
                blogPosts.Slug = Slug;
                blogPosts.Created = DateTimeOffset.Now;
                db.Posts.Add(blogPosts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPosts);
        }


      


        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.Posts.Find(id);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,MediaURL,Published")] BlogPosts blogPosts, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var blog = db.Posts.Where(p => p.Id == blogPosts.Id).FirstOrDefault();
                blog.Body = blogPosts.Body;
                blog.Published = blogPosts.Published;
                //blog.Slug = blogPost.Slug;
                blog.Title = blogPosts.Title;
                blog.Updated = DateTime.Now;
                var Slug = StringUtilites.URLFriendly(blogPosts.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPosts);
                }
                if (db.Posts.Any(p => p.Slug == Slug && p.Id != blog.Id))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPosts);
                }
                blog.Slug = Slug;
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blog.MediaUrl = "/Uploads/" + fileName;
                }

                blogPosts.Created = DateTimeOffset.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(blogPosts);
        }



        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.Posts.Find(id);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPosts blogPosts = db.Posts.Find(id);
            db.Posts.Remove(blogPosts);
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
    }
}
