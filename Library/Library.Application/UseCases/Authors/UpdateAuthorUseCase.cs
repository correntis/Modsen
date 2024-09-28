using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.UseCases.Authors
{
    public class UpdateAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAuthorUseCase(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Author author)
        {
            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(author.Id)
                ?? throw new NotFoundException();

            authorEntity.Name = author.Name;
            authorEntity.Surname = author.Surname;
            authorEntity.Birthday = author.Birthday;
            authorEntity.Country = author.Country;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
