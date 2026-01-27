using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistance
{
    public class ChatRepository : IChatRepository
    {
        private readonly LibraryContext _context;

        public ChatRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task CreateChat(Chat chat, CancellationToken ct = default)
        {
            await _context.Chats.AddAsync(chat, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<ChatNotifications>> GetAllChatsForNewBookNotification(CancellationToken ct)
        {
            return await _context.ChatNotifications
                .AsNoTracking()
                .Include(n => n.Notification)
                .Where(n => n.Notification.NotificationType == Domain.Enums.NotificationType.NewBookNotification)
                .ToListAsync(ct);
        }

        public async Task<Chat?> GetChat(long id, CancellationToken ct = default)
        {
            return await _context.Chats.FirstOrDefaultAsync(c => c.Id == id, ct);
        }
    }
}
