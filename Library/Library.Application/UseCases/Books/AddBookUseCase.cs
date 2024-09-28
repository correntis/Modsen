using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class AddBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AddBookUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Guid> ExecuteAsync(Book book)
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
    }
}
