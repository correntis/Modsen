using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorsService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(Author author)
        {
            var authorEntity = _mapper.Map<AuthorEntity>(author);

            await _unitOfWork.AuthorsRepository.AddAsync(authorEntity);
            await _unitOfWork.SaveChangesAsync();

            return authorEntity.Id;
        }

        public async Task UpdateAsync(Author author)
        {
            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(author.Id);
            ThrowNotFoundIfNull(authorEntity);

            authorEntity.Name = author.Name;
            authorEntity.Surname = author.Surname;
            authorEntity.Birthday = author.Birthday;
            authorEntity.Country = author.Country;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var authorEntity = await _unitOfWork.AuthorsRepository.GetAsync(id);
            ThrowNotFoundIfNull(authorEntity);

            _unitOfWork.AuthorsRepository.Delete(authorEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AuthorEntity> GetAsync(Guid id)
        {
            var authorEnity = await _unitOfWork.AuthorsRepository.GetAsync(id);
            ThrowNotFoundIfNull(authorEnity);

            return authorEnity;
        }

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync()
        {
            return await _unitOfWork.AuthorsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AuthorEntity>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _unitOfWork.AuthorsRepository.GetPageAsync(pageIndex, pageSize);
        }

        private void ThrowNotFoundIfNull<T>(T entity)
        {
            if(entity is null)
            {
                throw new NotFoundException();
            }
        }
    }
}
