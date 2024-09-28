using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetAllBooksUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBooksUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Book>> ExecuteAsync()
        {
            var books = await _unitOfWork.BooksRepository.GetAllAsync();

            return _mapper.Map<List<Book>>(books);
        }
    }
}
