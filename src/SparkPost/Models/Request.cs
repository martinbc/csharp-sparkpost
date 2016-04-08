using Newtonsoft.Json;
using System.Net.Http;

namespace SparkPost
{
    public class Request
    {
        public string Url { get; set; }
        public object Data { get; set; }
        public HttpMethod Method { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// The Generic Result is used to parse the Results (every SparkPost API returns a Results).
    /// </summary>
    /// <typeparam name="T">Results Type</typeparam>
    public class GenericResult<T>
    {
        public T Results { get; set; }
    }

    /// <summary>
    /// The Generic Result is used to parse the Results (every SparkPost API returns a Results).
    /// </summary>
    /// <typeparam name="T">Results Type</typeparam>
    /// <typeparam name="T2">Content Type</typeparam>
    public class GenericResult<T, T2> : GenericResult<T>
    {
        public T2 Content { get; set; }
    }
}