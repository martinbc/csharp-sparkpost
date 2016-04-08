namespace SparkPost
{
    /// <summary>
    /// Provides access to the SparkPost API.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets or sets the key used for requests to the SparkPost API.
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the base URL of the SparkPost API.
        /// </summary>
        string ApiHost { get; set; }

        /// <summary>
        /// Gets access to the transmissions resource of the SparkPost API.
        /// </summary>
        ITransmissions Transmissions { get; }

        /// <summary>
        /// Gets the API version supported by this client.
        /// </summary>
        string Version { get; }
    }
}
