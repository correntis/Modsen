using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IBooksRepository
    {
        Task<Guid> AddAsync(Book book);
        Task<Guid> AddAuthorAsync(Guid bookId, Guid authorId);
        Task<Guid> DeleteAsync(Guid id);
        Task<Guid> DeleteAuthorAsync(Guid bookId, Guid authorId);
        Task<Book> GetAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIsbnAsync(string isbn);
        Task<Book> GetByAuthorAsync(Guid authorId);
        Task<IEnumerable<Book>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter);
        Task<int> GetAmountAsync(BooksFilter filter);
        Task<Guid> UpdateAsync(Book book);
    }
}