﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager
{
    class StockManager
    {
        
        static void Main(string[] args)
        {
            Stock Stock = new Stock();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "stock_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "stock_queue", autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");


                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        string[] messageSplit = message.Split(":");
                        string type = messageSplit[0];
                        if(type == "1")
                        {
                            string nomProduit = messageSplit[1];
                            int quantite = quantite = Int32.Parse(messageSplit[2]);
                            response = Stock.ReleaseItem(nomProduit,quantite);
                        }
                        else
                        {
                            string nomProduit = messageSplit[1];
                            int quantite = Int32.Parse(messageSplit[2]);
                            response = Stock.ReserveItem(quantite, nomProduit);
                        }
                        
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
