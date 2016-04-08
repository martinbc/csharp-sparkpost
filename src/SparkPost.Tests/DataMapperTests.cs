using NUnit.Framework;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SparkPost.Tests
{
    public class DataMapperTests
    {

        [TestFixture]
        public class RecipientMappingTests
        {
            [SetUp]
            public void Setup()
            {
                recipient = new Recipient();
                mapper = new DataMapper("v1");
            }

            private DataMapper mapper;
            private Recipient recipient;

            [Test]
            public void address()
            {
                var value = Guid.NewGuid().ToString();
                recipient.Address.Email = value;
                mapper.ToDictionary(recipient)
                    ["address"]
                    .CastAs<IDictionary<string, object>>()
                    ["email"]
                    .ShouldEqual(value);
            }

            [Test]
            public void return_path()
            {
                var value = Guid.NewGuid().ToString();
                recipient.ReturnPath = value;
                mapper.ToDictionary(recipient)["return_path"].ShouldEqual(value);
            }

            [Test]
            public void tags()
            {
                var tag1 = Guid.NewGuid().ToString();
                var tag2 = Guid.NewGuid().ToString();
                recipient.Tags.Add(tag1);
                recipient.Tags.Add(tag2);
                var theTags = mapper.ToDictionary(recipient)
                    ["tags"];
                theTags
                    .CastAs<IEnumerable<object>>()
                    .Count().ShouldEqual(2);
                mapper.ToDictionary(recipient)
                    ["tags"]
                    .CastAs<IEnumerable<object>>()
                    .ShouldContain(tag1);
                mapper.ToDictionary(recipient)
                    ["tags"]
                    .CastAs<IEnumerable<object>>()
                    .ShouldContain(tag2);
            }

            [Test]
            public void empty_tags_are_ignored()
            {
                mapper.ToDictionary(recipient)
                    .Keys.ShouldNotContain("tags");
            }

            [Test]
            public void metadata()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();
                recipient.Metadata[key] = value;
                mapper.ToDictionary(recipient)["metadata"]
                    .CastAs<IDictionary<string, object>>()[key].ShouldEqual(value);
            }

            [Test]
            public void do_not_include_empty_metadata()
            {
                mapper.ToDictionary(recipient).Keys.ShouldNotContain("metadata");
            }

            [Test]
            public void substitution_data()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();
                recipient.SubstitutionData[key] = value;
                mapper.ToDictionary(recipient)["substitution_data"]
                    .CastAs<IDictionary<string, object>>()[key].ShouldEqual(value);
            }

            [Test]
            public void do_not_include_empty_substitution_data()
            {
                mapper.ToDictionary(recipient).Keys.ShouldNotContain("substitution_data");
            }

            [Test]
            public void The_type_should_be_ignored()
            {
                recipient.Type = RecipientType.CC;
                mapper.ToDictionary(recipient).Keys.ShouldNotContain("type");
            }
        }

        [TestFixture]
        public class AddressMappingTests
        {
            [SetUp]
            public void Setup()
            {
                address = new Address();
                mapper = new DataMapper("v1");
            }

            private Address address;
            private DataMapper mapper;

            [Test]
            public void email()
            {
                var value = Guid.NewGuid().ToString();
                address.Email = value;
                mapper.ToDictionary(address)["email"].ShouldEqual(value);
            }

            [Test]
            public void name()
            {
                var value = Guid.NewGuid().ToString();
                address.Name = value;
                mapper.ToDictionary(address)["name"].ShouldEqual(value);
            }

            [Test]
            public void header_to()
            {
                var value = Guid.NewGuid().ToString();
                address.HeaderTo = value;
                mapper.ToDictionary(address)["header_to"].ShouldEqual(value);
            }

            [Test]
            public void header_to_is_not_returned_if_empty()
            {
                mapper.ToDictionary(address)
                    .Keys.ShouldNotContain("header_to");
            }
        }

        [TestFixture]
        public class TransmissionMappingTests
        {
            [SetUp]
            public void Setup()
            {
                transmission = new Transmission();
                mapper = new DataMapper("v1");
            }

            private Transmission transmission;
            private DataMapper mapper;

            [Test]
            public void It_should_set_the_content_dictionary()
            {
                var email = Guid.NewGuid().ToString();
                transmission.Content.From = new Address { Email = email };
                mapper.ToDictionary(transmission)["content"]
                    .CastAs<IDictionary<string, object>>()["from"]
                    .CastAs<IDictionary<string, object>>()["email"]
                    .ShouldEqual(email);
            }

            [Test]
            public void It_should_set_the_recipients()
            {
                var recipient1 = new Recipient { Address = new Address { Email = Guid.NewGuid().ToString() } };
                var recipient2 = new Recipient { Address = new Address { Email = Guid.NewGuid().ToString() } };

                transmission.Recipients = new List<Recipient> { recipient1, recipient2 };

                var result = mapper.ToDictionary(transmission)["recipients"] as IEnumerable<IDictionary<string, object>>;
                result.Count().ShouldEqual(2);
                result.ToList()[0]["address"]
                    .CastAs<IDictionary<string, object>>()
                    ["email"].ShouldEqual(recipient1.Address.Email);
                result.ToList()[1]["address"]
                    .CastAs<IDictionary<string, object>>()
                    ["email"].ShouldEqual(recipient2.Address.Email);
            }

            [Test]
            public void It_should_set_the_recipients_to_a_list_id_if_a_list_id_is_provided()
            {
                var listId = Guid.NewGuid().ToString();
                transmission.ListId = listId;

                var result = mapper.ToDictionary(transmission)["recipients"] as IDictionary<string, object>;
                result["list_id"].ShouldEqual(listId);
            }

            [Test]
            public void campaign_id()
            {
                var value = Guid.NewGuid().ToString();
                transmission.CampaignId = value;
                mapper.ToDictionary(transmission)["campaign_id"].ShouldEqual(value);
            }

            [Test]
            public void description()
            {
                var value = Guid.NewGuid().ToString();
                transmission.Description = value;
                mapper.ToDictionary(transmission)["description"].ShouldEqual(value);
            }

            [Test]
            public void return_path()
            {
                var value = Guid.NewGuid().ToString();
                transmission.ReturnPath = value;
                mapper.ToDictionary(transmission)["return_path"].ShouldEqual(value);
            }

            [Test]
            public void do_not_send_the_return_path_if_it_is_not_provided()
            {
                mapper.ToDictionary(transmission).Keys.ShouldNotContain("return_path");
            }

            [Test]
            public void metadata()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();
                transmission.Metadata[key] = value;
                mapper.ToDictionary(transmission)["metadata"]
                    .CastAs<IDictionary<string, object>>()[key].ShouldEqual(value);
            }

            [Test]
            public void do_not_include_empty_metadata()
            {
                mapper.ToDictionary(transmission).Keys.ShouldNotContain("metadata");
            }

            [Test]
            public void substitution_data()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();
                transmission.SubstitutionData[key] = value;
                mapper.ToDictionary(transmission)["substitution_data"]
                    .CastAs<IDictionary<string, object>>()[key].ShouldEqual(value);
            }

            [Test]
            public void do_not_include_empty_substitution_data()
            {
                mapper.ToDictionary(transmission).Keys.ShouldNotContain("substitution_data");
            }

            [Test]
            public void options()
            {
                transmission.Options.ClickTracking = true;
                mapper.ToDictionary(transmission)["options"]
                    .CastAs<IDictionary<string, object>>()
                    ["click_tracking"].ShouldEqual(true);

                transmission.Options.ClickTracking = false;
                mapper.ToDictionary(transmission)["options"]
                    .CastAs<IDictionary<string, object>>()
                    ["click_tracking"].ShouldEqual(false);
            }
        }

        [TestFixture]
        public class MappingCcFields
        {
            [SetUp]
            public void Setup()
            {
                transmission = new Transmission();
                mapper = new DataMapper("v1");
            }

            private Transmission transmission;
            private DataMapper mapper;

            [Test]
            public void It_should_set_the_CC_Header_for_the_cc_emails()
            {
                var recipient1 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = Guid.NewGuid().ToString()}};
                var recipient2 = new Recipient {Type = RecipientType.To, Address = new Address {Email = Guid.NewGuid().ToString()}};
                var recipient3 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = Guid.NewGuid().ToString()}};

                transmission.Recipients = new List<Recipient> {recipient1, recipient2, recipient3};

                var cc = mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    ["headers"]
                    .CastAs<IDictionary<string, string>>()
                    ["CC"];

                cc.ShouldEqual("<" + recipient1.Address.Email + ">,<" + recipient3.Address.Email + ">");
            }

            [Test]
            public void It_should_not_overwrite_any_existing_headers()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();

                var recipient1 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = Guid.NewGuid().ToString()}};
                transmission.Recipients = new List<Recipient> {recipient1};

                transmission.Content.Headers[key] = value;

                 mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    ["headers"]
                    .CastAs<IDictionary<string, string>>()
                    [key].ShouldEqual(value);
            }

            [Test]
            public void It_should_not_set_the_cc_if_there_are_no_cc_emails()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();

                var recipient1 = new Recipient {Type = RecipientType.To, Address = new Address {Email = Guid.NewGuid().ToString()}};
                var recipient2 = new Recipient {Type = RecipientType.BCC, Address = new Address {Email = Guid.NewGuid().ToString()}};
                transmission.Recipients = new List<Recipient> {recipient1, recipient2};

                transmission.Content.Headers[key] = value;

                 mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    ["headers"]
                    .CastAs<IDictionary<string, string>>()
                    .ContainsKey("CC")
                    .ShouldBeFalse();
            }

            [Test]
            public void It_should_not_set_a_header_value_if_there_are_no_ccs()
            {
                var recipient1 = new Recipient {Type = RecipientType.To, Address = new Address {Email = Guid.NewGuid().ToString()}};
                var recipient2 = new Recipient {Type = RecipientType.BCC, Address = new Address {Email = Guid.NewGuid().ToString()}};
                transmission.Recipients = new List<Recipient> {recipient1, recipient2};

                 mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    .ContainsKey("headers")
                    .ShouldBeFalse();
            }

            [Test]
            public void It_should_ignore_empty_ccs()
            {
                var recipient1 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = ""}};
                var recipient2 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = null}};
                var recipient3 = new Recipient {Type = RecipientType.CC, Address = new Address {Email = " "}};
                transmission.Recipients = new List<Recipient> {recipient1, recipient2, recipient3};

                 mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    .ContainsKey("headers")
                    .ShouldBeFalse();
            }

            [Test]
            public void It_should_ignore_any_cc_recipients_with_no_address()
            {
                var recipient1 = new Recipient {Type = RecipientType.CC, Address = null};
                transmission.Recipients = new List<Recipient> {recipient1};

                 mapper.ToDictionary(transmission)
                    ["content"]
                    .CastAs<IDictionary<string, object>>()
                    .ContainsKey("headers")
                    .ShouldBeFalse();
            }

        }

        [TestFixture]
        public class ContentMappingTests
        {
            [SetUp]
            public void Setup()
            {
                content = new Content();
                mapper = new DataMapper("v1");
            }

            private Content content;
            private DataMapper mapper;

            [Test]
            public void from()
            {
                content.From.Email = Guid.NewGuid().ToString();
                content.From.HeaderTo = Guid.NewGuid().ToString();
                content.From.Name = Guid.NewGuid().ToString();

                var result = mapper.ToDictionary(content)["from"].CastAs<IDictionary<string, object>>();

                result["email"].ShouldEqual(content.From.Email);
                result["header_to"].ShouldEqual(content.From.HeaderTo);
                result["name"].ShouldEqual(content.From.Name);
            }

            [Test]
            public void subject()
            {
                var value = Guid.NewGuid().ToString();
                content.Subject = value;
                mapper.ToDictionary(content)["subject"].ShouldEqual(value);
            }

            [Test]
            public void text()
            {
                var value = Guid.NewGuid().ToString();
                content.Text = value;
                mapper.ToDictionary(content)["text"].ShouldEqual(value);
            }

            [Test]
            public void template_id()
            {
                var value = Guid.NewGuid().ToString();
                content.TemplateId = value;
                mapper.ToDictionary(content)["template_id"].ShouldEqual(value);
            }

            [Test]
            public void html()
            {
                var value = Guid.NewGuid().ToString();
                content.Html = value;
                mapper.ToDictionary(content)["html"].ShouldEqual(value);
            }

            [Test]
            public void reply_to()
            {
                var value = Guid.NewGuid().ToString();
                content.ReplyTo = value;
                mapper.ToDictionary(content)["reply_to"].ShouldEqual(value);
            }

            [Test]
            public void headers()
            {
                var key = Guid.NewGuid().ToString();
                var value = Guid.NewGuid().ToString();
                content.Headers[key] = value;
                mapper.ToDictionary(content)["headers"]
                    .CastAs<IDictionary<string, string>>()
                    [key].ShouldEqual(value);
            }

            [Test]
            public void do_not_include_empty_headers()
            {
                mapper.ToDictionary(content)
                    .Keys.ShouldNotContain("headers");
            }

            [Test]
            public void attachments()
            {
                var firstName = Guid.NewGuid().ToString();
                var secondName = Guid.NewGuid().ToString();
                content.Attachments.Add(new Attachment { Name = firstName });
                content.Attachments.Add(new Attachment { Name = secondName });

                var names = mapper.ToDictionary(content)["attachments"]
                    .CastAs<IEnumerable<object>>()
                    .Select(x => x.CastAs<Dictionary<string, object>>())
                    .Select(x => x["name"]);

                names.Count().ShouldEqual(2);
                names.ShouldContain(firstName);
                names.ShouldContain(secondName);
            }

            [Test]
            public void no_attachments_should_not_include_the_attachments_block()
            {
                mapper.ToDictionary(content).Keys.ShouldNotContain("attachments");
            }

            [Test]
            public void inline_images()
            {
                var firstName = Guid.NewGuid().ToString();
                var secondName = Guid.NewGuid().ToString();
                content.InlineImages.Add(new InlineImage { Name = firstName });
                content.InlineImages.Add(new InlineImage { Name = secondName });

                var mappedAttachments = mapper.ToDictionary(content)["inline_images"];
                var names = mappedAttachments
                    .CastAs<IEnumerable<object>>()
                    .Select(x => x.CastAs<Dictionary<string, object>>())
                    .Select(x => x["name"]);

                names.Count().ShouldEqual(2);
                names.ShouldContain(firstName);
                names.ShouldContain(secondName);
            }

            [Test]
            public void no_inline_images_should_not_include_the_inline_images_block()
            {
                mapper.ToDictionary(content).Keys.ShouldNotContain("inline_images");
            }
        }

        [TestFixture]
        public class OptionsMappingTests
        {
            [SetUp]
            public void Setup()
            {
                options = new Options();
                mapper = new DataMapper("v1");
            }

            private DataMapper mapper;
            private Options options;

            [Test]
            public void It_should_default_to_returning_null()
            {
                mapper.ToDictionary(options).ShouldBeNull();
            }

            [Test]
            public void open_tracking()
            {
                options.OpenTracking = true;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["open_tracking"].ShouldEqual(true);

                options.OpenTracking = false;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["open_tracking"].ShouldEqual(false);
            }

            [Test]
            public void click_tracking()
            {
                options.ClickTracking = true;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["click_tracking"].ShouldEqual(true);

                options.ClickTracking = false;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["click_tracking"].ShouldEqual(false);
            }

            [Test]
            public void transactional()
            {
                options.Transactional = true;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["transactional"].ShouldEqual(true);

                options.Transactional = false;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["transactional"].ShouldEqual(false);
            }

            [Test]
            public void sandbox()
            {
                options.Sandbox = true;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["sandbox"].ShouldEqual(true);

                options.Sandbox = false;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["sandbox"].ShouldEqual(false);
            }

            [Test]
            public void skip_suppression()
            {
                options.SkipSuppression = true;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["skip_suppression"].ShouldEqual(true);

                options.SkipSuppression = false;
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["skip_suppression"].ShouldEqual(false);
            }

            [Test]
            public void start_time()
            {
                var startTime = "2015-02-11T08:00:00-04:00";
                options.StartTime = DateTimeOffset.Parse(startTime);
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["start_time"].ShouldEqual(startTime);

                startTime = "2015-02-11T08:00:00-14:00";
                options.StartTime = DateTimeOffset.Parse(startTime);
                mapper.ToDictionary(options).CastAs<IDictionary<string, object>>()
                    ["start_time"].ShouldEqual(startTime);
            }

            [Test]
            public void hide_start_time_if_it_is_missing()
            {
                options.OpenTracking = true;
                mapper.ToDictionary(options)
                    .CastAs<IDictionary<string, object>>()
                    .Keys.ShouldNotContain("start_time");
            }
        }

        [TestFixture]
        public class FileMappingTests
        {
            private File file;
            private DataMapper mapper;

            [SetUp]
            public void Setup()
            {
                file = new Attachment();
                mapper = new DataMapper("v1");
            }

            [Test]
            public void name()
            {
                var value = Guid.NewGuid().ToString();
                file.Name = value;
                mapper.ToDictionary(file)["name"].ShouldEqual(value);
            }

            [Test]
            public void type()
            {
                var value = Guid.NewGuid().ToString();
                file.Type = value;
                mapper.ToDictionary(file)["type"].ShouldEqual(value);
            }

            [Test]
            public void data()
            {
                var value = Guid.NewGuid().ToString();
                file.Data = value;
                mapper.ToDictionary(file)["data"].ShouldEqual(value);
            }
        }
    }
}