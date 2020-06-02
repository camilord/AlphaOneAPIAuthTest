using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlphaOneAPITest
{
    class Program
    {
        private static String API_BASE_URL = "https://council-api.abcs.co.nz";
        private static String API_USERNAME = "{SOME_USERNAME}";
        private static String API_PASSWORD = "{SOME_SECRET_KEY}";

        // METHOD 1 ================================================================================================================ [start]
        static async Task Main1(string[] args)
        {
            var json = await (API_BASE_URL + "/v1/authenticate").PostUrlEncodedAsync(
                new
                {
                    username = API_USERNAME,
                    key = API_PASSWORD
                }).ReceiveString();


            Console.WriteLine(json);
        }
        // METHOD 1 ================================================================================================================ [end]

        // METHOD 2 ================================================================================================================ [start]
        static void Main2(string[] args)
        {
            //var response = authenticate();
            //Console.WriteLine(response);

            Task<String> task = Task.Run<String>(async () => await authenticate());
            task.Wait();
            String console_output = task.Result.ToString();
            Console.WriteLine(console_output);
        }

        static async Task<String> authenticate()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", API_USERNAME },
                { "key", API_PASSWORD }
            };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync((API_BASE_URL + "/v1/authenticate"), content);
            var json = await response.Content.ReadAsStringAsync();

            return json;
        }
        // METHOD 2 ================================================================================================================ [end]

        // METHOD 3 ================================================================================================================ [start]
        static void Main(string[] args)
        {
            String url = (API_BASE_URL + "/v1/authenticate");
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // manual concat credentials
            String post_data = "username=" + API_USERNAME + "&key=" + API_PASSWORD;

            // using string builder
            StringBuilder formData = new StringBuilder();
            formData.AppendFormat("{0}={1}", "username", API_USERNAME);
            formData.AppendFormat("&{0}={1}", "key", API_PASSWORD);
            post_data = formData.ToString();

            Stream stream = request.GetRequestStream();
            byte[] postArray = Encoding.ASCII.GetBytes(post_data);
            stream.Write(postArray, 0, postArray.Length);
            stream.Close();

            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            string Result = sr.ReadToEnd();

            Console.WriteLine(Result.ToString());
        }
        // METHOD 3 ================================================================================================================ [end]

    }
}
