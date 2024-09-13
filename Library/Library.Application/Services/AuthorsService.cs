using AutoMapper;
using Library.Core.Abstractions;
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
            return await _authorsRepository.UpdateAsync(author);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _authorsRepository.DeleteAsync(id);
        }

        public async Task<Author> GetAsync(Guid id)
        {
            return await _authorsRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _authorsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Author>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _authorsRepository.GetPageAsync(pageIndex, pageSize);
        }
    }
}
