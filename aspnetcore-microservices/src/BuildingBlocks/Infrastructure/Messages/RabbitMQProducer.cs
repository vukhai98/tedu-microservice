﻿using Contracts.Common.Interfaces;
using Contracts.Messages;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class RabbitMQProducer : IMesssageProducer
    {
        private readonly ISerializeService _serializeService;

        public RabbitMQProducer(ISerializeService serializeService)
        {
            _serializeService = serializeService;
        }

        public void SendMessage<T>(T message)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orders", exclusive: false);

            var jsonData = _serializeService.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonData);

            // Tạo một phiên bản của IBasicProperties
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.Persistent = true; // Đánh dấu tin nhắn là bền vững nếu cần thiết

            // Sử dụng phiên bản chính xác của BasicPublish
            channel.BasicPublish("", "orders", basicProperties, body);

        }
    }
}
