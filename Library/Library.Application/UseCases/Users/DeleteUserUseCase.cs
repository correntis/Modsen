using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(id)
            ?? throw new NotFoundException();

            _unitOfWork.UsersRepository.Delete(userEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
