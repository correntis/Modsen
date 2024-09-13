using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.DataAccess.Repositories
{
    public class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsRepository> _logger;

        public AuthorsRepository(
            LibraryDbContext context,
            IMapper mapper,
            ILogger<AuthorsRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Author author)
        {
            var authorEntity = _mapper.Map<AuthorEntity>(author);

            _context.Authors.Add(authorEntity);
            await _context.SaveChangesAsync();

            return authorEntity.Id;
        }

        public async Task<Guid> UpdateAsync(Author author)
        {
            var authorEntity = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == author.Id);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            authorEntity.Name = author.Name;
            authorEntity.Surname = author.Surname;
            authorEntity.Birthday = author.Birthday;
            authorEntity.Country = author.Country;

            await _context.SaveChangesAsync();

            return authorEntity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var authorEntity = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            _context.Authors.Remove(authorEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<Author> GetAsync(Guid id)
        {
            var authorEntity = await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if(authorEntity is null)
            {
                return null;
            }

            return _mapper.Map<Author>(authorEntity);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var authorEntities = await _context.Authors
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<Author>>(authorEntities);
        }

        public async Task<IEnumerable<Author>> GetPageAsync(int pageIndex, int pageSize)
        {
            var authorEntities = await _context.Authors
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<Author>>(authorEntities);
        }
    }
}
