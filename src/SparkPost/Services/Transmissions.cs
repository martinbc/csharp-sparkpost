using System.Net;
using System.Net.Http;

namespace SparkPost
{
    public class Transmissions : ITransmissions
    {
        private readonly Client client;
        private readonly RequestSender requestSender;
        private readonly DataMapper dataMapper;

        public Transmissions(Client client, RequestSender requestSender, DataMapper dataMapper)
        {
            this.client = client;
            this.requestSender = requestSender;
            this.dataMapper = dataMapper;
        }

        public SendTransmissionResponse Send(Transmission transmission)
        {
            var request = new Request
            {
                Url = string.Format("api/{0}/transmissions", client.Version),
                Method = HttpMethod.Post,
                Data = dataMapper.ToDictionary(transmission)
            };

            var response = requestSender.SendSync<SendTransmissionResponse>(request);

            if (response.StatusCode != HttpStatusCode.OK) throw new ResponseException<SendTransmissionResponse>(response);

            return response.Results;
        }
    }
}