using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Models;
using System.Net;

namespace Library.Application.Services
{
    public class BooksService : IBooksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public BooksService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Guid> AddAsync(Book book)
        {
            string imagePath = await _fileService.SaveAsync(book.ImageFile);

            var bookEntity = _mapper.Map<BookEntity>(book);
            bookEntity.TakenAt = DateTime.MinValue;
            bookEntity.ReturnBy = DateTime.MinValue;
            bookEntity.ImagePath = imagePath;

            await _unitOfWork.BooksRepository.AddAsync(bookEntity);
            await _unitOfWork.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task AddAuthorAsync(Guid bookId, Guid authorId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId);
            ThrowNotFoundIfNull(bookEntity);

            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(authorId);
            ThrowNotFoundIfNull(bookEntity);

            bookEntity.Authors.Add(authorEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(book.Id);
            ThrowNotFoundIfNull(bookEntity);

            bookEntity.ISBN = book.ISBN;
            bookEntity.Name = book.Name;
            bookEntity.Description = book.Description;
            bookEntity.Genre = book.Genre;
            bookEntity.TakenAt = book.TakenAt;
            bookEntity.ReturnBy = book.ReturnBy;

            if(book.ImageFile != null)
            {
                bookEntity.ImagePath = await _fileService.SaveAsync(book.ImageFile);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(id);
            ThrowNotFoundIfNull(bookEntity);

            _unitOfWork.BooksRepository.Delete(bookEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(Guid bookId, Guid authorId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId);
            ThrowNotFoundIfNull(bookEntity);

            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(authorId);
            ThrowNotFoundIfNull(authorEntity);

            bookEntity.Authors.Remove(authorEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BookEntity> GetAsync(Guid id)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(id);
            ThrowNotFoundIfNull(bookEntity);

            return bookEntity;
        }

        public async Task<BookEntity> GetByAuthorAsync(Guid authorId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetByAuthorAsync(authorId);
            ThrowNotFoundIfNull(bookEntity);

            return bookEntity;
        }

        public async Task<BookEntity> GetByIsbnAsync(string isbn)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetByIsbnAsync(isbn);
            ThrowNotFoundIfNull(bookEntity);

            return bookEntity;
        }

        public async Task<IEnumerable<BookEntity>> GetAllAsync()
        {
            return await _unitOfWork.BooksRepository.GetAllAsync();
        }

        public async Task<IEnumerable<BookEntity>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter)
        {
            return await _unitOfWork.BooksRepository.GetPageAsync(pageIndex, pageSize, filter);
        }

        public async Task<int> GetAmountAsync(BooksFilter filter)
        {
            return await _unitOfWork.BooksRepository.GetAmountAsync(filter);
        }

        private void ThrowNotFoundIfNull<T>(T entity)
        {
            if(entity is null)
            {
                throw new NotFoundException();
            }
        }
    }
}
