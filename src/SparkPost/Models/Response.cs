using Newtonsoft.Json;
using System.Net;

namespace SparkPost
{
    public class Response<T>
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore]
        public string ReasonPhrase { get; set; }

        [JsonIgnore]
        public string RawResponse { get; set; }

        /// <summary>
        ///     This is the actual content of the Response
        /// </summary>
        public T Results { get; set; }
    }
}