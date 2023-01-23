using Microsoft.AspNetCore.SignalR;
using StockChat.Data;

namespace StockChat.Hubs
{
    public class ChatHub : Hub
    {

        public readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message, string userId)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            //Save messages on the database
            Message messageObject = new Message();
            messageObject.Username = user;
            messageObject.Text = message;
            messageObject.UserId = userId;
            await _context.Messages.AddAsync(messageObject);
            await _context.SaveChangesAsync();
        }

    }
}
