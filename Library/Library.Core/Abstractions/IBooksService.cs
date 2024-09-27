using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IBooksService
    {
        Task<Guid> AddAsync(Book book);
        Task AddAuthorAsync(Guid bookId, Guid authorId);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
        Task DeleteAuthorAsync(Guid bookId, Guid authorId);

        Task<BookEntity> GetAsync(Guid id);
        Task<BookEntity> GetByAuthorAsync(Guid authorId);
        Task<BookEntity> GetByIsbnAsync(string isbn);

        Task<IEnumerable<BookEntity>> GetAllAsync();
        Task<IEnumerable<BookEntity>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter);
        Task<int> GetAmountAsync(BooksFilter filter);
    }
}