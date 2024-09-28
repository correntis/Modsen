using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class UpdateBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateBookUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task ExecuteAsync(Book book)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(book.Id)
                ?? throw new NotFoundException();

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
    }
}
