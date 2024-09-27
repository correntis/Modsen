using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IBooksRepository
    {
        Task AddAsync(BookEntity book);
        void Delete(BookEntity book);
        Task<BookEntity> GetAsync(Guid id);
        Task<BookEntity> GetByAuthorAsync(Guid authorId);
        Task<BookEntity> GetByIsbnAsync(string isbn);
        Task<IEnumerable<BookEntity>> GetAllAsync();
        Task<IEnumerable<BookEntity>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter);
        Task<int> GetAmountAsync(BooksFilter filter);
    }
}