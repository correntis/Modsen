using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class AddBookAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddBookAuthorUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(Guid bookId, Guid authorId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId)
                 ?? throw new NotFoundException();

            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(authorId)
                ?? throw new NotFoundException();

            bookEntity.Authors.Add(authorEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
