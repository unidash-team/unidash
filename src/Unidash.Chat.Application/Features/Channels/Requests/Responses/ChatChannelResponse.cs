using System.Collections.Generic;
using Unidash.Core.Domain;

namespace Unidash.Chat.Application.Features.Channels.Requests.Responses
{
    public class ChatChannelResponse : EntityDto
    {
        public string Title { get; set; }

        public List<string> ParticipantUserIds { get; set; }
    }
}
