using _5HeadBot.Services.BotMessageService.Data;
using Discord;
using System;
using System.Collections.Generic;

namespace _5HeadBot.Services.BotMessageService
{
    // I don't add this 'BotMessageBuilder' to my dependency injection container
    // because this builder never gonna change and thus also doesn't have an interface.
    // This practice also used with Discord.Net EmbedBuilder my class build upon;

    // But the most important reason, is that following code will not work as it's intended to
    /*
     * //some injection configuration 
     * ...
     * ServiceCollection().AddTransient<EmbedBuilder>();
     * ...
     * class C 
     * {
     *    // actual injection
     *    EmbedBuilder b  { get; set; }
     * 
     *    someMethod() 
     *    {
     *        for (int i = 0; i < 2; i++)
     *        {
     *            // as I expected, b should be created on each it's call
     *            b.WithFields(new List<EmbedFieldBuilder>() { new EmbedFieldBuilder().WithName("what1").WithValue("what1") });
     *            
     *            // but in reality, it's not! The line above creates it and the lines below mutate it. Like if b would be injected as Scoped.
     *            // at the end of the loop, b contains all the fields added from the loop and that is totally wrong and unexpected.
     *            await ReplyAsync(embed: b.WithTitle("onlyThisShoulBeMessagedButItIsNotOnlyItInReality").Build());
     *            
     *            b.WithFields(new List<EmbedFieldBuilder>() { new EmbedFieldBuilder().WithName("what2").WithValue("what2") })
     *        }
     *    }
     * }
     */
    public class BotMessageBuilder
    {
        private EmbedBuilder EmbedBuilderInstance;
        private EmbedBuilder EmbedBuilder
        {
            get
            {
                if (EmbedBuilderInstance is null)
                    EmbedBuilderInstance = new EmbedBuilder();
                return EmbedBuilderInstance;
            }
            set { EmbedBuilderInstance = value; }
        }

        private string _text;
        private BotMessageStyle _displayType = BotMessageStyle.Default;
        public BotMessageBuilder WithEmbed(Embed e)
        {
            EmbedBuilder = e.ToEmbedBuilder();
            return this;
        }

        #region EmbedBuilder interface copy
        public BotMessageBuilder WithEmbedAuthor(string author)
        {
            EmbedBuilder.WithAuthor(author);
            return this;
        }

        public BotMessageBuilder WithEmbedColor(Color color)
        {
            EmbedBuilder.WithColor(color);
            return this;
        }

        public BotMessageBuilder WithEmbedCurrentTimestamp()
        {
            EmbedBuilder.WithCurrentTimestamp();
            return this;
        }

        public BotMessageBuilder WithEmbedDescription(string description)
        {
            EmbedBuilder.WithDescription(description);
            return this;
        }

        public BotMessageBuilder WithEmbedWithFields(IReadOnlyCollection<EmbedFieldBuilder> fields)
        {
            EmbedBuilder.WithFields(new List<EmbedFieldBuilder>(fields));
            return this;
        }

        public BotMessageBuilder WithEmbedWithFooter(EmbedFooterBuilder footer)
        {
            EmbedBuilder.WithFooter(footer);
            return this;
        }

        public BotMessageBuilder WithEmbedWithImageUrl(string imageUrl)
        {
            EmbedBuilder.WithImageUrl(imageUrl);
            return this;
        }

        public BotMessageBuilder WithEmbedWithThumbnailUrl(string imageUrl)
        {
            EmbedBuilder.WithThumbnailUrl(imageUrl);
            return this;
        }

        public BotMessageBuilder WithEmbedWithTimestamp(DateTimeOffset dateTimeOffset)
        {
            EmbedBuilder.WithTimestamp(dateTimeOffset);
            return this;
        }

        public BotMessageBuilder WithEmbedWithTitle(string title)
        {
            EmbedBuilder.WithTitle(title);
            return this;
        }

        public BotMessageBuilder WithEmbedWithUrl(string url)
        {
            EmbedBuilder.WithUrl(url);
            return this;
        }

        #endregion
        public BotMessageBuilder WithText(string text)
        {
            _text = text;
            return this;
        }
        public BotMessageBuilder WithDisplayType(BotMessageStyle displayType)
        {
            _displayType = displayType;
            return this;
        }
        public BotMessage Build()
        {
            return new BotMessage(new BotMessageContent(_text, embed: EmbedBuilder.Build()), _displayType);
        }
    }
}
