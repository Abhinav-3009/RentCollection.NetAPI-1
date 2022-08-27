using System;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using Azure.Storage.Blobs.Models;

namespace RentCollection.NetAPI.Azure
{
    public class Blob
    {

        private readonly string ContainerName = "angularblob";

        private readonly string ConnectionString = "";

        private readonly BlobClient BlobManagerClient;
        private readonly BlobContainerClient BlobContainerManagerClient;

        public Blob(string blobName)
        {
            this.BlobManagerClient = new BlobClient(connectionString: this.ConnectionString, blobContainerName: ContainerName, blobName: blobName);

            this.BlobContainerManagerClient = new BlobContainerClient(this.ConnectionString, this.ContainerName);
        }

        public void Upload(Stream stream)
        {
            try
            {
                this.BlobManagerClient.Upload(stream);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void Delete()
        {
            try
            {
                this.BlobManagerClient.Delete();
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public List<string> View()
        {
            List<string> blobsList = new List<string>();
            try
            {
                this.BlobContainerManagerClient.CreateIfNotExists();
                var blobs = this.BlobContainerManagerClient.GetBlobs();
                foreach (BlobItem blobItem in blobs)
                {
                    blobsList.Add(blobItem.Name);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return blobsList;
        }
    }
}

