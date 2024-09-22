using System.ComponentModel.DataAnnotations;

namespace Library.API.Contracts
{
    public class LoginContract
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
