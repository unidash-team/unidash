using System.ComponentModel.DataAnnotations;
using Unidash.Core.Domain;

namespace Unidash.Chat.Application.DataModels
{
    public class ChatUser : Entity
    {
        [Required]
        public string DisplayName { get; set; }
    }
}