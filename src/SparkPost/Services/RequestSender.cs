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

            this.jsonSerializer = new NewtonsoftJsonSerializer();
        }

        // FIXME this method supports POST only
        public async Task<Response<T>> Send<T>(Request request)
        {
            using (var c = new HttpClient())
            {
                c.BaseAddress = new Uri(client.ApiHost);
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Add("Authorization", client.ApiKey);

                var result = await c.PostAsync(request.Url, BuildContent(request.Data));

                string resultContent = await result.Content.ReadAsStringAsync();

                return new Response<T>
                {
                    StatusCode = result.StatusCode,
                    ReasonPhrase = result.ReasonPhrase,
                    Results = JsonConvert.DeserializeObject<GenericResult<T>>(resultContent).Results,
                    RawResponse = resultContent
                };
            }
        }

        public Response<T> SendSync<T>(Request request)
        {
            IRestResponse res = Request(request);

            return new Response<T>
            {
                StatusCode = res.StatusCode,
                // ReasonPhrase is not supported by RestSharp
                ReasonPhrase = res.StatusDescription,
                Results = JsonConvert.DeserializeObject<GenericResult<T>>(res.Content).Results,
                RawResponse = res.Content
            };
        }

        private IRestResponse Request(Request request)
        {
            RestRequest req = new RestRequest(request.Url);
            req.JsonSerializer = jsonSerializer;
            req.AddJsonBody(request.Data);


            if (request.Method == HttpMethod.Get)
            {
                req.Method = Method.GET;
            }

            if (request.Method == HttpMethod.Post)
            {
                req.Method = Method.POST;
            }

            // Execute
            return this.restClient.Execute(req);
        }

        private static StringContent BuildContent(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None }));
        }
    }
}