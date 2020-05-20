using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foodies.Foody.Chat.Application.Models
{
    public class ChatChannel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<ChatMessage> Messages { get; set; }

        [Required]
        public ICollection<ChatUser> Participants { get; set; }
    }
}