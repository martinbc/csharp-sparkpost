
namespace SparkPost
{
    public class Template
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Published { get; set; }
        public string Description { get; set; }

        public string LastUpdateTime { get; set; }
        public string LastUse { get; set; }

        public TemplateContent Content { get; set; }
    }

    public class TemplateContent
    {
        public string Html { get; set; }
    }
}
