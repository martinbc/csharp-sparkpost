using System;

namespace SparkPost
{
    public class ResponseException<T> : Exception
    {
        private readonly Response<T> response;

        public ResponseException(Response<T> response)
        {
            this.response = response;
        }

        public override string Message
        {
            get { return response.RawResponse; }
        }
    }
}