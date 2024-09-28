using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetBookByIsbnUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookByIsbnUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Book> ExecuteAsync(string isbn)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetByIsbnAsync(isbn)
                ?? throw new NotFoundException();

            return _mapper.Map<Book>(bookEntity);
        }
    }
}
