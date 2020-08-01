using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Azure;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Glihmware.Azure.Storage
{
    public static class Blob
    {
        /// <summary>
        ///   Lists blob's names inside the given container.
        ///   Flat listing, only returning the name.
        /// </summary>
        /// 
        /// <param name="serviceClient"> Blob service client of storage account. </param>
        /// 
        /// <param name="containerName"> Container name to be listed. </param>
        public static async Task<List<string>>
        ListFlat
        (
            BlobServiceClient serviceClient,
            string containerName
        )
        {
            List<string> blobNames = new List<string>();

            BlobContainerClient bcc
                = serviceClient.GetBlobContainerClient(containerName);

            await foreach (BlobItem bi in bcc.GetBlobsAsync(BlobTraits.None))
            {
                blobNames.Add(bi.Name);
            }

            return blobNames;
        }





        /// <summary>
        ///   Downloads a blob.
        /// </summary>
        /// 
        /// <param name="serviceClient"> Blob service client of storage account. </param>
        /// 
        /// <param name="containerName"> Container where the blob is stored. </param>
        /// 
        /// <param name="blobName"> Name of the blob to be downloaded. </param>
        /// 
        /// <returns> `MemoryStream` with the blob content if success, `null` otherwise. </returns>
        public static async Task<MemoryStream>
        Download
        (
            BlobServiceClient serviceClient,
            string containerName,
            string blobName
        )
        {
            BlobContainerClient bcc = serviceClient.GetBlobContainerClient(containerName);
            BlobClient bc = bcc.GetBlobClient(blobName);

            Response r;
            MemoryStream blobContent = new MemoryStream();

            try
            {
                r = await bc.DownloadToAsync(blobContent);
            }
            catch (RequestFailedException e)
            {
                // Need simple logger to be used everywhere...!
                Console.WriteLine($"{e.Message}");
                return null;
            }

            return blobContent;
        }



        /// <summary>
        ///   Uploads a blob.
        /// </summary>
        /// 
        /// <param name="serviceClient"> Blob service client of storage account. </param>
        /// 
        /// <param name="containerName"> Container where the blob will be uploaded. </param>
        /// 
        /// <param name="blobName"> Name of the blob (including virtual folders). </param>
        /// 
        /// <param name="blobContent"> Blob's content. </param>
        /// 
        /// <returns> `true` if the blob was uploaded successfully, `false` otherwise.</returns>
        public static async Task<bool>
        Upload
        (
            BlobServiceClient serviceClient,
            string containerName,
            string blobName,
            MemoryStream blobContent,
            bool overwrite = false
        )
        {
            BlobContainerClient bcc = serviceClient.GetBlobContainerClient(containerName);
            BlobClient bc = bcc.GetBlobClient(blobName);

            BlobContentInfo r;

            try
            {
                blobContent.Position = 0;
                r = await bc.UploadAsync(blobContent, overwrite);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"{e.Message}");
                return false;
            }

            return true;
        }


    }
}
