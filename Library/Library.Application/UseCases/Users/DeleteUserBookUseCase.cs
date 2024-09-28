using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Users
{
    public class DeleteUserBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserBookUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid userId, Guid bookId)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(userId)
            ?? throw new NotFoundException();

            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId)
                ?? throw new NotFoundException();

            bookEntity.TakenAt = DateTime.MinValue;
            bookEntity.ReturnBy = DateTime.MinValue;
            userEntity.Books.Remove(bookEntity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
