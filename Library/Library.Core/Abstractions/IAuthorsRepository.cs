using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IAuthorsRepository
    {
        Task<Guid> Create(Author author);
        Task<Guid> Delete(Guid id);
        Task<Author> Get(Guid id);
        Task<IEnumerable<Author>> GetAll();
        Task<IEnumerable<Author>> GetPage(int pageIndex, int pageSize);
        Task<Guid> Update(Author author);
    }
}