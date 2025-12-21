using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail (string email,CancellationToken ct =default);

        public Task CreateUser(User user,CancellationToken ct = default);
    }
}
