﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.DataTransfer;
using Unidash.Chat.Application.DataTransfer.Partials;

namespace Unidash.Chat.Application.Features.Channels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatChannel, ChatChannelResponse>(MemberList.Destination);
            CreateMap<ChatChannel, ChatChannelPartialResponse>(MemberList.Destination);
        }
    }
}
