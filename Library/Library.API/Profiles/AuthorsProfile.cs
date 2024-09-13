using AutoMapper;
using Library.API.Contracts;
using Library.Core.Models;
using Library.DataAccess.Entities;

namespace Library.API.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, AuthorEntity>().ReverseMap();
            CreateMap<Author, AuthorContract>().ReverseMap();

            AllowNullCollections = false;
        }
    }
}
