using System.ComponentModel.DataAnnotations;

namespace Library.API.Contracts
{
    public class RegisterContract
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
