using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IChatRepository
    {
        Task CreateChat(Chat chat, CancellationToken ct = default);

        Task<Chat?> GetChat(long id, CancellationToken ct = default);
    }
}
