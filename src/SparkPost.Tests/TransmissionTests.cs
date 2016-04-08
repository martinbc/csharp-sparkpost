using NUnit.Framework;
using Should;

namespace SparkPost.Tests
{
    public class TransmissionTests
    {
        [TestFixture]
        public class ConstructorTests
        {
            [SetUp]
            public void Setup()
            {
                transmission = new Transmission();
            }

            private Transmission transmission;

            [Test]
            public void It_should_not_be_missing_a_recipients_list()
            {
                transmission.Recipients.ShouldNotBeNull();
            }

            [Test]
            public void It_should_not_be_missing_content()
            {
                transmission.Content.ShouldNotBeNull();
            }

            [Test]
            public void It_should_not_be_missing_metadata()
            {
                transmission.Metadata.ShouldNotBeNull();
            }

            [Test]
            public void It_should_not_be_missing_substition_data()
            {
                transmission.SubstitutionData.ShouldNotBeNull();
            }
        }
    }
}