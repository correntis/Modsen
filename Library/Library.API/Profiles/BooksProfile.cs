using AutoMapper;
using Library.API.Contracts;
using Library.Core.Models;
using Library.Core.Entities;

namespace Library.API.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, BookEntity>().ReverseMap();
            CreateMap<Book, BookContract>().ReverseMap();
            AllowNullCollections = false;
        }
    }
}
