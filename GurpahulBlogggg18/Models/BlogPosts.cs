using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GurpahulBlogggg18.Models
{
    public class BlogPosts
    {
        public BlogPosts()
        {
            this.Comments = new HashSet<Comments>();
            this.Created = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public bool Published { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }

    }
}