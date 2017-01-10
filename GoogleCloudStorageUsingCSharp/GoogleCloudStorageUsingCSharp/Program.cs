using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;

namespace GoogleCloudStorageUsingCSharp
{
    class Program
    {
        // Google Cloud Platform project
        static string projectId = "cloudstoragesandbox";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter a bucket name to use or create");
                var bucketname = Console.ReadLine();
                CreateBucket(bucketname);

                Console.WriteLine("Enter a file path for upload");
                var filepath = Console.ReadLine();
                if (File.Exists(filepath))
                {
                    UploadObject(bucketname, filepath);
                }
                else
                {
                    Console.WriteLine("File does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e.Message}");
            }

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }

        static void CreateBucket(string bucketName)
        {
            StorageClient storageClient = StorageClient.Create();
            if (storageClient.ListBuckets(projectId).All(b => b.Name != bucketName))
            {
                // Creates the new bucket
                storageClient.CreateBucket(projectId, bucketName);
                Console.WriteLine($"Created new bucket {bucketName}");
                return;
            }
            Console.WriteLine($"Bucket {bucketName} found");
        }

        static void UploadObject(string bucket, string filepath)
        {
            StorageClient storageClient = StorageClient.Create();
            using (var f = File.OpenRead(filepath))
            {
                var objectName = Path.GetFileName(filepath);
                storageClient.UploadObject(bucket, objectName, null, f);
                Console.WriteLine($"{objectName} uploaded to bucket {bucket}");
            }
        }

    }
}
