using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Unidash.Core.Domain;

namespace Unidash.Chat.Application.DataModels
{
    public class ChatChannel : Entity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public ICollection<string> ParticipantUserIds { get; set; }

        [Required]
        public string CreatorUserId { get; set; }
    }
}