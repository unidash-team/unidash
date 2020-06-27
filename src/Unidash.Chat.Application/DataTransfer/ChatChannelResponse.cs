using System.Collections.Generic;
using AutoMapper;
using Unidash.Chat.Application.DataTransfer.Partials;
using Unidash.Core.Domain;

namespace Unidash.Chat.Application.DataTransfer
{
    public class ChatChannelResponse : EntityDto
    {
        public string Title { get; set; }

        [IgnoreMap]
        public List<ChatUserPartialResponse> Participants { get; set; } = new List<ChatUserPartialResponse>();
    }
}
