using System;

namespace SparkPost
{
    public class ResponseException : Exception
    {
        private readonly Response response;

        public ResponseException(Response response)
        {
            this.response = response;
        }

        public override string Message
        {
            get { return response.Content; }
        }
    }
}