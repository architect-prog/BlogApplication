using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels.Post
{
    public class PostDetailsViewModel
    {
        public string Title { get; set; }       
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
