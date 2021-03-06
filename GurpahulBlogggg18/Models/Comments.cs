﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GurpahulBlogggg18.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public int BlogPostsId { get; set; }
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateReason { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual BlogPosts BlogPosts { get; set; }
    }
}