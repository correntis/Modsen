using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IAuthorsService
    {
        Task<Guid> AddAsync(Author author);
        Task<Guid> DeleteAsync(Guid id);
        Task<Author> GetAsync(Guid id);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<IEnumerable<Author>> GetPageAsync(int pageIndex, int pageSize);
        Task<Guid> UpdateAsync(Author author);
    }
}