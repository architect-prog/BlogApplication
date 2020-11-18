using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Models;
using BlogApplication.Repository;
using BlogApplication.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserContext _posts;
        public PostsController(UserContext posts)
        {
            _posts = posts;
        }
        public IActionResult Index()
        {
            IEnumerable<Post> posts = _posts.Posts.ToList();
            return View(posts);
        }

        public IActionResult Create() => View();        

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {

            Post post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                PublishedDate = DateTime.UtcNow                
            };

            await _posts.Posts.AddAsync(post);
            await _posts.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Edit(int postId)
        {
            Post post = await _posts.Posts.FindAsync(postId);

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
                Post post = await _posts.Posts.FindAsync(model.Id);
                if (post != null)
                {
                    post.Title = model.Title;
                    post.Content = model.Content;
                    _posts.Posts.Update(post);

                    await _posts.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Delete(int postId)
        {
            Post post = await _posts.Posts.FindAsync(postId);
            if (post != null)
            {
                _posts.Posts.Remove(post);
                await _posts.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int postId)
        {
            Post post = await _posts.Posts.FindAsync(postId);
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
