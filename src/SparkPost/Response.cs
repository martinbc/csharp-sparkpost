using System.Net;

namespace SparkPost
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
    }
}