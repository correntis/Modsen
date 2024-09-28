using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class DeleteBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(Guid id)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(id)
                ?? throw new NotFoundException();

            _unitOfWork.BooksRepository.Delete(bookEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
