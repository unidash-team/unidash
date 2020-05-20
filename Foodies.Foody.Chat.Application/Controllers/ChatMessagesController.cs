using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foodies.Foody.Chat.Application.Data;
using Foodies.Foody.Chat.Application.Hubs;
using Foodies.Foody.Chat.Application.Models;
using Microsoft.AspNetCore.SignalR;

namespace Foodies.Foody.Chat.Application.Controllers
{
    [Route("messages")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChatMessagesController(ChatDbContext chatDbContext, IHubContext<ChatHub> chatHubContext)
        {
            _chatDbContext = chatDbContext;
            _chatHubContext = chatHubContext;
        }

        // GET: api/ChatMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatMessages()
        {
            return await _chatDbContext.Messages.ToListAsync();
        }

        // GET: api/ChatMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMessage>> GetChatMessage(string id)
        {
            var chatMessage = await _chatDbContext.Messages.FindAsync(id);

            if (chatMessage == null)
            {
                return NotFound();
            }

            return chatMessage;
        }

        // POST: api/ChatMessages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatMessage>> PostChatMessage(ChatMessage chatMessage)
        {
            _chatDbContext.Messages.Add(chatMessage);

            try
            {
                await _chatDbContext.SaveChangesAsync();

                await _chatHubContext.Clients
                    .Users(chatMessage.UserId)
                    .SendAsync("ReceiveMessage", chatMessage);
            }
            catch (DbUpdateException)
            {
                if (ChatMessageExists(chatMessage.Id))
                    return Conflict(); 
                
                throw;
            }

            return CreatedAtAction("GetChatMessage", new { id = chatMessage.Id }, chatMessage);
        }

        // DELETE: api/ChatMessages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatMessage>> DeleteChatMessage(string id)
        {
            var chatMessage = await _chatDbContext.Messages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            _chatDbContext.Messages.Remove(chatMessage);
            await _chatDbContext.SaveChangesAsync();

            return chatMessage;
        }

        private bool ChatMessageExists(string id)
        {
            return _chatDbContext.Messages.Any(e => e.Id == id);
        }
    }
}
