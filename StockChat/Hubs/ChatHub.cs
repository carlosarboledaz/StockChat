using Microsoft.AspNetCore.SignalR;
using StockChat.Data;
using StockChat.Data.Repositories;
using StockChat.Events;
using StockChat.Services;

namespace StockChat.Hubs
{
    public class ChatHub : Hub
    {
        public readonly ApplicationDbContext _context;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IStockService _stockService;
        private readonly IRepository<Message> _repository;

        public ChatHub(ApplicationDbContext context, IRabbitMQProducer rabbitMQProducer, IStockService stockService, IRepository<Message> repository)
        {
            _context = context;
            _rabbitMQProducer = rabbitMQProducer;
            _stockService = stockService;
            _repository = repository;
        }

        public async Task SendMessage(string user, string message, string userId)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            else if (message.Contains("/stock="))
            {
                var stockCode = message.Split("=")[1];
                var stockMessage = await _stockService.GetStockInfo(stockCode);
                if (string.IsNullOrEmpty(stockMessage))
                {
                    message = "The provided stock code is invalid. Please double check it";
                    await Clients.All.SendAsync("ReceiveMessage", "StockBot", message);
                }
                else
                {
                    await Clients.All.SendAsync("ReceiveMessage", user, message);
                    _rabbitMQProducer.SendStockMessage(stockMessage);
                }
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
                //Save messages on the database
                Message messageObject = new Message {
                    Username = user,
                    Text = message,
                    UserId = userId
                };
                _repository.Add(messageObject);
                _repository.SaveChanges();
            }
        }

    }
}
