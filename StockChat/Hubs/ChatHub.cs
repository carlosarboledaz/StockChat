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

            //validate the format of the message. If it's for the command, call RabbitMQ to process the API call

            await Clients.All.SendAsync("ReceiveMessage", user, message);
            //Save messages on the database
            Message messageObject = new Message();
            messageObject.Username = user;
            messageObject.Text = message;
            messageObject.UserId = userId;
            await _context.Messages.AddAsync(messageObject);
            await _context.SaveChangesAsync();
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.Others.SendAsync("ReceiveNotification", $"{Context.User?.Identity?.Name} join chat");
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Clients.Others.SendAsync("ReceiveNotification", $"{Context.User?.Identity?.Name} left chat");
        //}

    }
}
