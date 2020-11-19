using AutoMapper;
using BlogApplication.Models;
using BlogApplication.ViewModels.Roles;
using BlogApplication.ViewModels.User;

namespace BlogApplication.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetailsViewModel>();

            CreateMap<User, CreateUserViewModel>();

            CreateMap<User, EditUserViewModel>();

            CreateMap<User, ChangePasswordViewModel>();

            CreateMap<User, ChangeRoleViewModel>()
                .ForMember(dest => dest.UserEmail, option => option.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.Id));

            CreateMap<SignUpViewModel, User>()
                .ForMember(dest => dest.UserName, option => option.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email));


        }

    }
}
