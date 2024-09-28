using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Users
{
    public class UpdateUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(User user)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(user.Id)
                ?? throw new NotFoundException();

            userEntity.UserName = user.UserName;
            userEntity.Email = user.Email;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
