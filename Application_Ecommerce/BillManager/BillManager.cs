using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillManager
{
    class BillManager
    {
        
        static void Main(string[] args)
        {
            List<ItemLine> panier = new List<ItemLine>();
            UserManager.User utilisateur = new UserManager.User();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bill_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "bill_queue", autoAck: false, consumer: consumer);
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
                        response = "";
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        if (message == "Facture")
                        {
                            Bill b=Bill.CreateBill(utilisateur,panier);
                            Console.WriteLine("Total" + b.sousTotalAvecTaxe);
                            b.afficher();
                            response = "Facture élaborée";
                            Console.WriteLine(response);
                        }
                        else {

                            string[] messageSplit = message.Split(":");

                            if(messageSplit[0] == "User")
                            {
                                utilisateur.nom = messageSplit[1];
                            }
                            else
                            {
                                string nomProduit = messageSplit[0];
                                int quantite = Int32.Parse(messageSplit[1]);


                                ItemLine selectionne = null;
                                foreach (ItemLine item in StockManager.Stock.getStock())
                                {
                                    if (item.item.nom == nomProduit)
                                    {
                                        selectionne = item;
                                    }
                                }
                                if (selectionne != null)
                                {
                                    panier.Add(new ItemLine(selectionne.item, quantite));
                                    response = "Commande enregistrée";
                                    Console.WriteLine(response);
                                }

                            }
                            

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
