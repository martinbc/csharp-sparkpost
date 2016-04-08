namespace SparkPost
{
    public class Client : IClient
    {
        public Client(string apiKey, string apiHost = "https://api.sparkpost.com")
        {
            ApiKey = apiKey;
            ApiHost = apiHost;
            Transmissions = new Transmissions(this, new RequestSender(this), new DataMapper(Version));
            Templates = new Templates(this, new RequestSender(this), new DataMapper(Version));
        }

        public string ApiKey { get; set; }
        public string ApiHost { get; set; }

        public ITransmissions Transmissions { get; private set; }
        public ITemplates Templates { get; private set; }

        public string Version
        {
            get { return "v1"; }
        }
    }
}