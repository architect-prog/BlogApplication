using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Models;
using BlogApplication.Repository;
using BlogApplication.ViewModels.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostRepository _posts;
        private readonly UserManager<User> _userManager;
        public PostsController(PostRepository posts, UserManager<User> userManager)
        {
            _posts = posts;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                ViewBag.IsAdmin = roles.Contains("Admin");
                ViewBag.UserId = user.Id;
            }
        
            IEnumerable<Post> posts = _posts.GetAll();

            return View(posts);
        }

        public IActionResult Create() => View();        

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            User user = await _userManager.GetUserAsync(User);

            Post post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                UserId = user.Id,
                PublishedDate = DateTime.UtcNow
            };

            await _posts.Add(post);           
            return View();
        }

        public async Task<IActionResult> Edit(int postId)
        {
            Post post = await _posts.GetById(postId);
            
            if (post == null)
            {
                return NotFound();
            }

            EditPostViewModel model = new EditPostViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = await _posts.GetById(model.Id);
                if (post != null)
                {
                    post.Title = model.Title;
                    post.Content = model.Content;

                    await _posts.Update(post);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Delete(int postId)
        {
            await _posts.Delete(postId);            

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int postId)
        {
            Post post = await _posts.GetById(postId);

            PostDetailsViewModel model = new PostDetailsViewModel()
            {
                Title = post.Title,
                Content = post.Content,
                PublishedDate = post.PublishedDate
            };
            return View(model);
        }
    }
}
