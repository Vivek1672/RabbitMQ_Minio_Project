using Minio;
using Minio.Exceptions;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace RabbitMQ_MinIO
{
    public class Program : Send
    {
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string HostName = "localhost";
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            { HostName = HostName, UserName = UserName, Password = Password, };

            Send sd = new Send();
            var endpoint = "play.min.io";
            var accessKey = "Q3AM3UQ867SPQQA43P2F";
            var secretKey = "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG";
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey).WithSSL();
                Program.Run(minio).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            string s = "yes";
            Console.WriteLine("You want to save you file in local system?" + "Please enter " + s + "  for download the file");
            string ss = Console.ReadLine();
            if (s == ss)
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey).WithSSL();
                Download.Run(minio).Wait();
            }

            Console.WriteLine("Calling Rabbit_Send Method to send the message as a Producer ! ");
            var sx = Send.Rabbit_Send();
            Console.WriteLine("Sending operation is : " + sx);

            Console.WriteLine("Calling Rabbit_Received Method to received the message as a Consumer !");
            Received.Rabbit_Received();
            Console.ReadLine();
        }

        // File uploader task.
        private async static Task Run(MinioClient minio)
        {
            var bucketName = "vivek";
            var location = "us-east-1";
            var objectName = "example.pdf";
            var filePath = "C:\\Users\\user\\.p2\\Desktop\\Outgoing\\example.pdf";
            var contentType = "application/pdf";
            Console.WriteLine("Step:1 -- We have example.pdf file in Outgoing folder");

            try
            {
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    Console.WriteLine("Step:2 -- Folder is not found, So we created the folder for you !");
                    await minio.MakeBucketAsync(bucketName, location);
                }
                Console.WriteLine("Step:3 -- Upload a file to bucket");
                await minio.PutObjectAsync(bucketName, objectName, filePath, contentType);
                Console.WriteLine("Successfully uploaded " + objectName);
                var url = "http://" + "play.min.io/" + "minio/" + bucketName + "/";
                //var uploadPath = "http://play.min.io/minio/vivek/";
                Console.WriteLine("Your file saved at location :-" + url);
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

    }
}
