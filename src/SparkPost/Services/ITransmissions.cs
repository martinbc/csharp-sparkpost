
namespace SparkPost
{
    /// <summary>
    /// Provides access to the transmissions resource of the SparkPost API.
    /// </summary>
    public interface ITransmissions
    {
        /// <summary>
        /// Sends an email transmission.
        /// </summary>
        /// <param name="transmission">The properties of the transmission to send.</param>
        /// <returns>The response from the API.</returns>
        SendTransmissionResponse Send(Transmission transmission);
    }
}