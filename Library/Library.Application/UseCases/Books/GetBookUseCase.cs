using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Book> ExecuteAsync(Guid id)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(id)
                ?? throw new NotFoundException();

            return _mapper.Map<Book>(bookEntity);
        }
    }
}
