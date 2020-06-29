using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Unidash.Chat.Application.DataModels;
using Unidash.Core.Infrastructure;

namespace Unidash.Chat.Application.Features.Messages.Queries
{
    public class ListChatMessagesQuery : IRequest<List<ChatMessage>>
    {
        public string UserId { get; set; }

        public string ChannelId { get; set; }

        public class Handler : IRequestHandler<ListChatMessagesQuery, List<ChatMessage>>
        {
            private readonly IEntityRepository<ChatMessage> _entityRepository;

            public Handler(IEntityRepository<ChatMessage> entityRepository)
            {
                _entityRepository = entityRepository;
            }

            public Task<List<ChatMessage>> Handle(ListChatMessagesQuery request, CancellationToken cancellationToken)
            {
                return Task.Run(() =>
                {
                    var query = _entityRepository.AsQueryable();

                    if (!string.IsNullOrEmpty(request.UserId))
                        query = query.Where(x => x.UserId == request.UserId);

                    if (!string.IsNullOrEmpty(request.ChannelId))
                        query = query.Where(x => x.ChannelId == request.ChannelId);

                    return query.ToList();
                }, cancellationToken);
            }
        }
    }
}
