using System.ComponentModel.DataAnnotations;

namespace Library.API.Contracts
{
    public class LoginContract
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
