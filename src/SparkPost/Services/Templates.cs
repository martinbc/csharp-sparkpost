using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SparkPost
{
    public class Templates : ITemplates
    {
        private readonly Client client;
        private readonly RequestSender requestSender;
        private readonly DataMapper dataMapper;

        public Templates(Client client, RequestSender requestSender, DataMapper dataMapper)
        {
            this.client = client;
            this.requestSender = requestSender;
            this.dataMapper = dataMapper;
        }

        public List<Template> GetTemplates()
        {
            var request = new Request
            {
                Url = string.Format("api/{0}/templates", client.Version),
                Method = HttpMethod.Get
            };

            var response = requestSender.SendSync<List<Template>>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ResponseException<List<Template>>(response);
            }

            return response.Results;
        }

        public Template GetTemplate(string templateId)
        {
            var request = new Request
            {
                Url = string.Format("api/{0}/templates/{1}", client.Version, templateId),
                Method = HttpMethod.Get
            };

            var response = requestSender.SendSync<Template>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ResponseException<Template>(response);
            }

            return response.Results;
        }
    }
}
