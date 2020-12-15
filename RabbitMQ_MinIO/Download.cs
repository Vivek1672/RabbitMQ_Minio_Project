using Minio;
using System;
using System.Threading.Tasks;

namespace RabbitMQ_MinIO
{
    public class Download
    {
        public async static Task Run(MinioClient minio)
        {
            var bucketName = "vivek";
            var objectName = "example.pdf";
            var filePath = "C:\\Users\\user\\.p2\\Desktop\\Incoming\\" + objectName;
            Console.WriteLine("Step:4 -- We have example.pdf file at Minio Server in vivek folder");
            try
            {
                await minio.GetObjectAsync(bucketName, objectName, filePath);
                Console.WriteLine("Step:5 -- File Downloaded Successfully, path is- " + "C:\\Users\\user\\.p2\\Desktop\\Incoming");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Download : " + ex.ToString());
            }

            Console.WriteLine("Press Enter to Send link via message to RabbitMQ ");
            Console.ReadLine();

        }
    }
}