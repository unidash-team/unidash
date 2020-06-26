using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Unidash.Chat.Application.Features.Channels.Requests;
using Unidash.Chat.Application.Features.Channels.Requests.Responses;
using Unidash.Chat.Application.Features.Messages.Requests;

namespace Unidash.Chat.Application.Controllers
{
    [Route("channels")]
    [ApiController]
    [Authorize]
    public class ChannelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChannelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new chat channel with the participants as specified.
        /// This action automatically adds the authenticated user as a participant.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> PostChannel([FromBody] PostChannelRequest request) => _mediator.Send(request);

        /// <summary>
        /// Returns a collection of channels that the user has access to. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChatChannelResponse>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetChannels() => _mediator.Send(new GetChannelListRequest());

        /// <summary>
        /// Removes the authenticated participant from the channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpDelete("{channelId}/participants/@me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> DeleteParticipantFromChannel(string channelId) =>
            _mediator.Send(new DeleteCurrentParticipantFromChannelRequest(channelId));

        [HttpPost("{channelId}/messages")]
        public Task<IActionResult> PostMessage(string channelId, [FromBody] string message) => 
            _mediator.Send(new PostMessageRequest(channelId, message));

        [HttpGet("{channelId}/messages")]
        public Task<IActionResult> GetMessages(string channelId) => 
            _mediator.Send(new GetMessageListRequest(channelId));
    }
}
