using Library.Core.Entities;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions
{
    public interface IAuthService
    {
        Task<(UserEntity, UserToken)> Login(string email, string password);
        Task<UserToken> Register(string username, string email, string password);
    }
}
