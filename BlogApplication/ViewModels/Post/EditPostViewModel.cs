using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels.Post
{
    public class EditPostViewModel
    {
        public int Id { get; set; }     
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFileCollection ImageCollection { get; set; }
    }
}
