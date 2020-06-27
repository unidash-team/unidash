using System.Threading.Tasks;
using Unidash.Chat.Application.DataModels;

namespace Unidash.Chat.Application.Hubs
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}