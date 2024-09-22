using System.ComponentModel.DataAnnotations;

namespace Library.API.Contracts
{
    public class RegisterContract
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
