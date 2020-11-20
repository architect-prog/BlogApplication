using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels.Post
{
    public class CreatePostViewModel
    {
        public IFormFileCollection ImageCollection { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

    }
}
