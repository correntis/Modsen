using AutoMapper;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace Library.API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<RegisterRequest, User>();

            CreateMap<UserRole, UserRoleEntity>().ReverseMap();
        }
    }
}
