using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPJSScrapper.Constant;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;
namespace BPJSScrapper.Helpers
{
    class FirebaseHelper
    {

        FirebaseClient client;

        public string databaseUrl { get; set; }
        public string authToken { get; set; }

        public FirebaseHelper()
        {
            if(authToken != null)
            {
                this.client = new FirebaseClient(this.databaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authToken)
                });
            }
            else
            {
                this.client = new FirebaseClient(this.databaseUrl);
            }
            
        }

        public FirebaseHelper(string databaseUrl, string authToken = null)
        {
            this.databaseUrl = databaseUrl;
            this.authToken = authToken;

            if(authToken != null)
            {
                this.client = new FirebaseClient(this.databaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(this.authToken)
                });
            }
            else
            {
                this.client = new FirebaseClient(this.databaseUrl);
            }
            

        }

        public FirebaseClient getClient()
        {
            return this.client;
        }

        public bool testConnections()
        {
            bool result = false;
            if(this.client != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public async Task<FirebaseObject<T>[]> GetAllData<T>(string path)
        {
            return (FirebaseObject<T>[])await this.client
                .Child(path)
                .OnceAsync<T>();
        }

        public async Task<T> GetDataByKey<T>(string path, string key)
        {
            string fullpath = path + "/" + key;
            var result = await this.client
                .Child(fullpath)
                .OnceSingleAsync<T>();
            return result;
        }

        public async Task addData<T>(string path,string key, T data)
        {
            await this.client.Child(path).Child(key).PutAsync<T>(data);
        }

        public async Task UpdateData<T>(string path, T data)
        {
            await this.client.Child(path).PutAsync(data);
        }

        public async Task DeleteData<T>(string path)
        {
            await this.client.Child(path).DeleteAsync();
        }






    }
}
