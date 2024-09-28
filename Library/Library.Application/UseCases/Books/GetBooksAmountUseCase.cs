using Library.Core.Abstractions;
using Library.Core.Models;

namespace Library.Application.UseCases.Books
{
    public class GetBooksAmountUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBooksAmountUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> ExecuteAsync(BooksFilter filter)
        {
            return await _unitOfWork.BooksRepository.GetAmountAsync(filter);
        }
    }
}
