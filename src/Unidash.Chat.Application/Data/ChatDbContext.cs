using Microsoft.EntityFrameworkCore;
using Unidash.Chat.Application.Models;

namespace Unidash.Chat.Application.Data
{
    public sealed class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<ChatMessage> Messages { get; set; }

        public DbSet<ChatChannel> Channels { get; set; }

        public DbSet<ChatUser> Users { get; set; }
    }
}
