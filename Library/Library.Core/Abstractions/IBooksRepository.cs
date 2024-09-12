using Library.Core.Models;

namespace Library.Core.Abstraction
{
    public interface IBooksRepository
    {
        Task<Guid> Create(Book book);
        Task<Guid> Delete(Guid id);
        Task<Book> Get(Guid id);
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetByISBN(string isbn);
        Task<IEnumerable<Book>> GetPage(int pageIndex, int pageSize);
        Task<Guid> Issue(Guid id, DateTime returnBy);
        Task<Guid> Update(Book book);
    }
}