using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetBooksPageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksPageUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Book>> ExecuteAsync(int pageIndex, int pageSize, BooksFilter filter)
        {
            var books = await _unitOfWork.BooksRepository.GetPageAsync(pageIndex, pageSize, filter);

            return _mapper.Map<List<Book>>(books);
        }
    }
}
