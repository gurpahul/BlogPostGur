using System;

using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using GurpahulBlogggg18.Models;

namespace GurpahulBlogggg18.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPosts);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogPostsId = new SelectList(db.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BlogPostsId,AuthorId,Body,Created,Updated,UpdateReason")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comments.AuthorId);
            ViewBag.BlogPostsId = new SelectList(db.Posts, "Id", "Title", comments.BlogPostsId);
            return View(comments);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
           
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Body,UpdateReason")] Comments comment)
        {
            if (ModelState.IsValid)
            {
                var commentDb = db.Comments.Where(p => p.Id == comment.Id).FirstOrDefault();
                commentDb.Updated = DateTime.Now;
                commentDb.Body = comment.Body;
                commentDb.UpdateReason = comment.UpdateReason;

                db.SaveChanges();
                return RedirectToAction("DetailsSlug", "BlogPosts", new { slug = commentDb.BlogPosts.Slug });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]

        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.Comments.Find(id);
            var slug = comments.BlogPosts.Slug;
            db.Comments.Remove(comments);
            db.SaveChanges();
            return RedirectToAction("DetailsSlug", "BlogPosts", new { slug });

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
