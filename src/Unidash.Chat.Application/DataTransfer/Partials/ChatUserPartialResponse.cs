using Unidash.Core.Domain;
using Unidash.Core.Domain.Flags;

namespace Unidash.Chat.Application.DataTransfer.Partials
{
    public class ChatUserPartialResponse : IIdentityFlag
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
    }
}