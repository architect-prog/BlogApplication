using AutoMapper;
using BlogApplication.Models;
using BlogApplication.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostViewModel, Post>();
               
            CreateMap<Post, EditPostViewModel>();

            CreateMap<Post, PostDetailsViewModel>();
        }
    }
}
