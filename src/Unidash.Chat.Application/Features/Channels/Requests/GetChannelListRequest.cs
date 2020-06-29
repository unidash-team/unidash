using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.DataTransfer;
using Unidash.Core.Infrastructure;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Features.Channels.Requests
{
    public class GetChannelListRequest : IRequest<IActionResult>
    {
        public class Handler : IRequestHandler<GetChannelListRequest, IActionResult>
        {
            private readonly IEntityRepository<ChatChannel> _chatChannelRepository;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(IEntityRepository<ChatChannel> chatChannelRepository,
                ICurrentUserAccessor currentUserAccessor,
                IMapper mapper)
            {
                _chatChannelRepository = chatChannelRepository;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            public async Task<IActionResult> Handle(GetChannelListRequest request, CancellationToken cancellationToken)
            {
                var userId = _currentUserAccessor.GetUserId();
                var channels = await _chatChannelRepository.FindByPredicateAsync(x => x.ParticipantUserIds.Contains(userId));

                var response = _mapper.Map<IEnumerable<ChatChannelResponse>>(channels);
                return new OkObjectResult(response);
            }
        }
    }
}