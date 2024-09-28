using Library.Core.Abstractions;
using Library.Core.Exceptions;

namespace Library.Application.UseCases.Authors
{
    public class DeleteAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAuthorUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(id)
            ?? throw new NotFoundException();

            _unitOfWork.AuthorsRepository.Delete(authorEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
