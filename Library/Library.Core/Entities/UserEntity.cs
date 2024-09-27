namespace Library.Core.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<UserRoleEntity> Roles { get; set; }
        public ICollection<BookEntity> Books { get; set; }
    }
}
