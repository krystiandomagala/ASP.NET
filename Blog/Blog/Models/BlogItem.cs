using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class BlogItem
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimestamp { get; set; }
    }
}
