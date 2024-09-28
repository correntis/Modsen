using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Application.UseCases.Authors
{
    public class AddAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddAuthorUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> ExecuteAsync(Author author)
        {
            var authorEntity = _mapper.Map<AuthorEntity>(author);

            await _unitOfWork.AuthorsRepository.AddAsync(authorEntity);
            await _unitOfWork.SaveChangesAsync();

            return authorEntity.Id;
        }
    }
}
