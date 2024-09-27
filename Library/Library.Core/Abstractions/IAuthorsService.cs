using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IAuthorsService
    {
        Task<Guid> AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Guid id);
        
        Task<AuthorEntity> GetAsync(Guid id);
        Task<IEnumerable<AuthorEntity>> GetAllAsync();
        Task<IEnumerable<AuthorEntity>> GetPageAsync(int pageIndex, int pageSize);
    }
}