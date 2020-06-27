using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unidash.Chat.Application.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// Stores the message in the database and broadcasts
        /// it to all participants who are connected via SignalR.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync(string channelId, string message);
    }
}
