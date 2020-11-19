using AutoMapper;
using BlogApplication.Models;
using BlogApplication.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            //CreateMap<User, UserDetailsViewModel>().ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.));
        }

    }
}
