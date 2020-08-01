using System;
using System.IO;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Glihmware.Azure.Storage;

namespace Tests
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Start");
            string cs = "DefaultEndpointsProtocol=https;AccountName=glihms2;AccountKey=IG492Wf8yC2tiM1wGzAa+qDy54T24SmIKgwrlfB5RtFdeRlhzqBWRIIA9wRRUD6lyf8BuTkkmS0y0Z+sUSYCOA==;EndpointSuffix=core.windows.net";

            BlobServiceClient bsc = new BlobServiceClient(cs);

            MemoryStream ms = await Blob.Download(bsc, "cont1", "img.jpg");
            if (ms == null)
            {
                Console.WriteLine("Nothing retuned...");
            }
            else
            {
                File.WriteAllBytes(@"/tmp/test.jpg", ms.ToArray());
                Console.WriteLine("Dl ok");
            }

            MemoryStream msc = new MemoryStream();
            using (FileStream fs = new FileStream(@"/tmp/test.jpg", FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(msc);
            }

            bool r = await Blob.Upload(bsc, "cont1", "img2.jpg", msc);
            Console.WriteLine(r);
        }
    }
}
