using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogApplication.Models;
using BlogApplication.Repository;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _posts;
        private readonly ImageRepository _images;

        public HomeController(ILogger<HomeController> logger, PostRepository posts, ImageRepository images)
        {
            _logger = logger;
            _posts = posts;
            _images = images;
        }

        public IActionResult Index()
        {
            List<Post> posts = _posts.GetAll();
            Random random = new Random(DateTime.Now.Millisecond);

            List<Post> result = new List<Post>();
            for (int i = 0; i < 3; i++)
            {
                result.Add(posts[random.Next(0, posts.Count)]);
            }

            foreach (Post post in result)
            {
                post.Images = _images.GetAll().Where(x => x.PostId == post.Id).ToList();
            }

            return View(result);
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
