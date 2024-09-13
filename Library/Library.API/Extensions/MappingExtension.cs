using AutoMapper.Configuration;
using AutoMapper;
using Library.API.Profiles;

namespace Library.API.Extensions
{
    public static class MappingExtension
    {
        public static void AddLibraryMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(BooksProfile),
                typeof(AuthorsProfile)
            );
        }
    }
}
