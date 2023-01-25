﻿using Microsoft.AspNetCore.SignalR;
using StockChat.Data;
using StockChat.Events;
using StockChat.Services;

namespace StockChat.Hubs
{
    public class ChatHub : Hub
    {
        public readonly ApplicationDbContext _context;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IStockService _stockService;

        public ChatHub(ApplicationDbContext context, IRabbitMQProducer rabbitMQProducer, IStockService stockService)
        {
            _context = context;
            _rabbitMQProducer = rabbitMQProducer;
            _stockService = stockService;
        }

        public async Task SendMessage(string user, string message, string userId)
        {
            if (message.Contains("/stock="))
            {
                var stockCode = message.Split("=")[1];
                var stockMessage = await _stockService.GetStockInfo(stockCode);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
                _rabbitMQProducer.SendStockMessage(stockMessage);
            }
            else
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
}
