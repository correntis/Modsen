using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IBooksService
    {
        Task<Guid> AddAsync(Book book);
        Task<Guid> AddAuthorAsync(Guid bookId, Guid authorId);
        Task<Guid> DeleteAsync(Guid id);
        Task<Guid> DeleteAuthorAsync(Guid bookId, Guid authorId);
        Task<Book> GetAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIsbnAsync(string isbn);
        Task<Book> GetByAuthorAsync(Guid authorId);
        Task<IEnumerable<Book>> GetPageAsync(int pageIndex, int pageSize);
        Task<Guid> UpdateAsync(Book book);
    }
}