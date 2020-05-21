using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unidash.Chat.Application.Models;
using Microsoft.EntityFrameworkCore;

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
