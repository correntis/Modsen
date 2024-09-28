using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetBookByAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookByAuthorUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Book> ExecuteAsync(Guid authorId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetByAuthorAsync(authorId)
                ?? throw new NotFoundException();

            return _mapper.Map<Book>(bookEntity);
        }
    }
}
