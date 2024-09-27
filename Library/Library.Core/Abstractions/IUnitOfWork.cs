namespace Library.Core.Abstractions
{
    public interface IUnitOfWork
    {
        IAuthorsRepository AuthorsRepository { get; }
        IBooksRepository BooksRepository { get; }
        IUsersRepository UsersRepository { get; }

        Task SaveChangesAsync();
    }
}