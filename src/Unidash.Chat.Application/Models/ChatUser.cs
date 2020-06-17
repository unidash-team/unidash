using System.ComponentModel.DataAnnotations;

namespace Unidash.Chat.Application.Models
{
    public class ChatUser
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}