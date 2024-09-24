using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
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
            book.TakenAt = DateTime.MinValue;
            book.ReturnBy = DateTime.MinValue;
            return await _booksRepository.AddAsync(book);
        }

        public async Task<Guid> AddAuthorAsync(Guid bookId, Guid authorId)
        {
            var guid = await _booksRepository.AddAuthorAsync(bookId, authorId);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> UpdateAsync(Book book)
        {
            var guid = await _booksRepository.UpdateAsync(book);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var guid = await _booksRepository.DeleteAsync(id);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> DeleteAuthorAsync(Guid bookId, Guid authorId)
        {
            var guid = await _booksRepository.DeleteAuthorAsync(bookId, authorId);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Book> GetAsync(Guid id)
        {
            var book =  await _booksRepository.GetAsync(id);

            ThrowNotFoundIfBookIsNull(book);

            return book;
        }

        public async Task<Book> GetByAuthorAsync(Guid authorId)
        {
            var book = await _booksRepository.GetByAuthorAsync(authorId);
            
            ThrowNotFoundIfBookIsNull(book);

            return book;
        }

        public async Task<Book> GetByIsbnAsync(string isbn)
        {
            var book =  await _booksRepository.GetByIsbnAsync(isbn);

            ThrowNotFoundIfBookIsNull(book);

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _booksRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Book>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter)
        {
            return await _booksRepository.GetPageAsync(pageIndex, pageSize, filter);
        }

        public async Task<int> GetAmountAsync(BooksFilter filter)
        {
            return await _booksRepository.GetAmountAsync(filter);
        }


        private void ThrowNotFoundIfEmptyGuid(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                throw new NotFoundException("User not found.");
            }
        }

        private void ThrowNotFoundIfBookIsNull(Book author)
        {
            if(author is null)
            {
                throw new NotFoundException("Book not found.");
            }
        }
    }
}
