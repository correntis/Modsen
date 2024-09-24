using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IMapper _mapper;

        public AuthorsService(
            IAuthorsRepository authorsRepository,
            IMapper mapper
            )
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(Author author)
        {
            return await _authorsRepository.AddAsync(author);
        }

        public async Task<Guid> UpdateAsync(Author author)
        {
            var guid = await _authorsRepository.UpdateAsync(author);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var guid =  await _authorsRepository.DeleteAsync(id);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Author> GetAsync(Guid id)
        {
            var author = await _authorsRepository.GetAsync(id);

            ThrowNotFoundIfAuthorIsNull(author);

            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _authorsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Author>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _authorsRepository.GetPageAsync(pageIndex, pageSize);
        }

        private void ThrowNotFoundIfEmptyGuid(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                throw new NotFoundException("Author not found.");
            }
        }

        private void ThrowNotFoundIfAuthorIsNull(Author author)
        {
            if(author is null)
            {
                throw new NotFoundException("Author not found.");
            }
        }
    }
}
