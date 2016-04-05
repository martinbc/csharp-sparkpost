using Newtonsoft.Json;
using System.Net;

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
                Method = "POST",
                Data = dataMapper.ToDictionary(transmission)
            };

            var response = requestSender.SendSync(request);

            if (response.StatusCode != HttpStatusCode.OK) throw new ResponseException(response);

            var results = JsonConvert.DeserializeObject<dynamic>(response.Content).results;
            return new SendTransmissionResponse()
            {
                Id = results.id,
                ReasonPhrase = response.ReasonPhrase,
                StatusCode = response.StatusCode,
                Content = response.Content,
                TotalAcceptedRecipients = results.total_accepted_recipients,
                TotalRejectedRecipients = results.total_rejected_recipients,
            };
        }
    }
}