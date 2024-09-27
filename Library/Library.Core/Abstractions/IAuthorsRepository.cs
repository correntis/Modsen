using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IAuthorsRepository
    {
        Task AddAsync(AuthorEntity author);
        void Delete(AuthorEntity author);
        
        Task<AuthorEntity> GetAsync(Guid id);
        Task<IEnumerable<AuthorEntity>> GetAllAsync();
        Task<IEnumerable<AuthorEntity>> GetPageAsync(int pageIndex, int pageSize);
    }
}