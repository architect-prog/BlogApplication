using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApplication.Models;
using BlogApplication.Repository;
using BlogApplication.ViewModels.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostRepository _posts;
        private readonly ImageRepository _images;
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public PostsController(PostRepository posts, UserManager<User> userManager, IMapper mapper, ImageRepository images)
        {
            _posts = posts;
            _userManager = userManager;
            _mapper = mapper;
            _images = images;
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

            foreach (Post post in posts)
            {
                post.Images = _images.GetAll().Where(x => x.PostId == post.Id).ToList();
            }

            return View(posts);
        }

        public async Task<IActionResult> UserPosts()
        {
            User user = await _userManager.GetUserAsync(User);

            IEnumerable<Post> userPosts = _posts.GetAll().Where(x => x.UserId == user.Id);

            return View(userPosts);
        }

        public IActionResult Create() => View();        

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            User user = await _userManager.GetUserAsync(User);

            Post post = _mapper.Map<Post>(model);
            post.UserId = user.Id;
            post.PublishedDate = DateTime.UtcNow;

            post = await _posts.Add(post);

            if (model.ImageCollection != null)
            {
                foreach (IFormFile image in model.ImageCollection)
                {
                    using (BinaryReader reader = new BinaryReader(image.OpenReadStream()))
                    {
                        byte[] imageBytes = reader.ReadBytes((int)image.Length);

                        await _images.Add(new Image()
                        {
                            ImageBytes = imageBytes,
                            PostId = post.Id
                        });
                    }
                }
            }       

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int postId)
        {
            Post post = await _posts.GetById(postId);

            if (post == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (post.UserId == user.Id || roles.Contains("Admin"))
            {
                EditPostViewModel model = _mapper.Map<EditPostViewModel>(post);
                return View(model);
            }
            else
            {
                return NotFound();
            }          
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = await _posts.GetById(model.Id);                
                post.Title = model.Title;
                post.Content = model.Content;

                List<Image> images = _images.GetAll().Where(x => x.PostId == post.Id).ToList();
                foreach (Image image in images)
                {
                    await _images.Delete(image.Id);
                }

                if (model.ImageCollection != null)
                {
                    foreach (IFormFile image in model.ImageCollection)
                    {
                        using (BinaryReader reader = new BinaryReader(image.OpenReadStream()))
                        {
                            byte[] imageBytes = reader.ReadBytes((int)image.Length);

                            await _images.Add(new Image()
                            {
                                ImageBytes = imageBytes,
                                PostId = post.Id
                            });
                        }
                    }
                }

                await _posts.Update(post);
                return RedirectToAction(nameof(Index));                
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Delete(int postId)
        {
            Post post = await _posts.GetById(postId);
            User user = await _userManager.GetUserAsync(User);          
            IList<string> roles = await _userManager.GetRolesAsync(user);
           
            if (post.UserId == user.Id || roles.Contains("Admin"))
            { 
                await _posts.Delete(postId);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Details(int postId)
        {
            Post post = await _posts.GetById(postId);
            List<Image> images = _images.GetAll().Where(x => x.PostId == post.Id).ToList();
            post.Images = images;

            PostDetailsViewModel model = _mapper.Map<PostDetailsViewModel>(post); 

            return View(model);
        }
    }
}
