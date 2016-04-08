using System.Collections.Generic;

namespace SparkPost
{
    /// <summary>
    /// Provides access to the templates resource of the SparkPost API.
    /// </summary>
    public interface ITemplates
    {
        /// <summary>
        /// Obtain the temples
        /// </summary>
        /// <returns>The templates list</returns>
        List<Template> GetTemplates();

        /// <summary>
        /// Obtain a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Template GetTemplate(string templateId);
    }
}
