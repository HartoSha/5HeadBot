using _5HeadBot.Services.Core.BotMessageService.Data;
using Discord;
using System;
using System.Collections.Generic;

namespace _5HeadBot.Services.Core.BotMessageService
{
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
