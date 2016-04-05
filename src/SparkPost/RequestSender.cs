using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SparkPost
{
    public class RequestSender
    {

        private readonly Client client;
        private readonly RestClient restClient;
        private readonly NewtonsoftJsonSerializer jsonSerializer;

        public RequestSender(Client client)
        {
            this.client = client;
            this.restClient = new RestClient(client.ApiHost);
            this.restClient.AddDefaultHeader("Authorization", client.ApiKey);

            this.jsonSerializer =  new NewtonsoftJsonSerializer();
        }

        public async Task<Response> Send(Request request)
        {
            using (var c = new HttpClient())
            {
                c.BaseAddress = new Uri(client.ApiHost);
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Add("Authorization", client.ApiKey);

                var result = await c.PostAsync(request.Url, BuildContent(request.Data));

                return new Response
                {
                    StatusCode = result.StatusCode,
                    ReasonPhrase = result.ReasonPhrase,
                    Content = await result.Content.ReadAsStringAsync(),
                };
            }
        }

        public Response SendSync(Request request)
        {
            RestRequest req = new RestRequest(request.Url);
            req.JsonSerializer = jsonSerializer;
            req.AddJsonBody(request.Data);

            // Execute
            IRestResponse res = this.restClient.Post(req);

            return new Response
            {
                StatusCode = res.StatusCode,
                // ReasonPhrase is not supported by RestSharp
                ReasonPhrase = res.StatusDescription,
                Content = res.Content,
            };
        }

        private static StringContent BuildContent(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None }));
        }
    }
}