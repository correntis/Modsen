using Library.Core.Abstractions;
using Library.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly LibraryDbContext _context;
        public IBooksRepository BooksRepository { get; private set; }
        public IAuthorsRepository AuthorsRepository { get; private set; }
        public IUsersRepository UsersRepository { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;

            BooksRepository = new BooksRepository(_context);
            AuthorsRepository = new AuthorsRepository(_context);
            UsersRepository = new UsersRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
