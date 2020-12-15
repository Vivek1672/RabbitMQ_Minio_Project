using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_MinIO
{
    public class Send
    {
        public static string Rabbit_Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Minio",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "http://play.min.io/minio/vivek/";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Minio",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("Message has been sent ! Please check your Inbox");
            }

            Console.WriteLine(" Press Enter to Consume the message ");
            Console.ReadLine();
            return "Done";
        }
    }
}
