using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class Post
    {
        public int Id { get; set; }       
        public string Title { get; set; }       
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Image> Images { get; set; }
    }
}
