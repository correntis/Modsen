using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;

        public BooksService(
            IBooksRepository booksRepository,
            IMapper mapper
            )
        {
            _booksRepository = booksRepository;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(Book book)
        {
            return await _booksRepository.AddAsync(book);
        }

        public async Task<Guid> AddAuthorAsync(Guid bookId, Guid authorId)
        {

            return await _booksRepository.AddAuthorAsync(bookId, authorId);
        }

        public async Task<Guid> UpdateAsync(Book book)
        {
            return await _booksRepository.UpdateAsync(book);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _booksRepository.DeleteAsync(id);
        }

        public async Task<Guid> DeleteAuthorAsync(Guid bookId, Guid authorId)
        {
            return await _booksRepository.DeleteAuthorAsync(bookId, authorId);
        }

        public async Task<Book> GetAsync(Guid id)
        {
            return await _booksRepository.GetAsync(id);
        }

        public async Task<Book> GetByAuthorAsync(Guid authorId)
        {
            return await _booksRepository.GetByAuthorAsync(authorId);
        }

        public async Task<Book> GetByIsbnAsync(string isbn)
        {
            return await _booksRepository.GetByIsbnAsync(isbn);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _booksRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Book>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _booksRepository.GetPageAsync(pageIndex, pageSize);
        }

    }
}
