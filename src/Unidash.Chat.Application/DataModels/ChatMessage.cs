using System;
using System.ComponentModel.DataAnnotations;
using Unidash.Core.Domain;

namespace Unidash.Chat.Application.DataModels
{
    public class ChatMessage : Entity
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string ChannelId { get; set; }
    }
}
