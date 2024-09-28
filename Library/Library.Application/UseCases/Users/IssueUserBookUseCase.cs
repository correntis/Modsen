using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Users
{
    public class IssueUserBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public IssueUserBookUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid userId, Guid bookId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId)
            ?? throw new NotFoundException();

            var userEntity = await _unitOfWork.UsersRepository.GetAsync(userId)
                ?? throw new NotFoundException();

            bookEntity.TakenAt = DateTime.UtcNow;
            bookEntity.ReturnBy = DateTime.UtcNow.AddDays(7);
            userEntity.Books.Add(bookEntity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
