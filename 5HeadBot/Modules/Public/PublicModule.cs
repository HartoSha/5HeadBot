using _5HeadBot.Modules.Internal;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using _5HeadBot.Services.Feature;
using _5HeadBot.Services.Feature.PictureService;
using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    public class PublicModule : MessageSenderModuleBase
    {
        public PictureService PictureService { get; set; }
        public SearchService SearchService { get; set; }

        [Command("ping")]
        public async Task PingAsync()
        {
            var messageCreatedDate = Context.Message.CreatedAt.ToLocalTime();
            var now = DateTimeOffset.Now;

            var pingMs = now.Subtract(messageCreatedDate).TotalMilliseconds;

            var messageBuilder = new BotMessageBuilder();
            messageBuilder.WithEmbedWithTitle($":satellite: Bot's ping is {pingMs.ToString(".##")} ms");
            Color messageColor = Color.Red;
            if (pingMs <= 500)
                messageColor = Color.Green;
            else if (pingMs <= 700)
                messageColor = Color.DarkGreen;
            else if (pingMs <= 1000)
                messageColor = Color.Orange;
            messageBuilder.WithEmbedColor(messageColor);

            await ReplyAsync(messageBuilder);
        }

        [Command("cat", RunMode = RunMode.Async)]
        [Alias("кот")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCatPictureAsync();
            if (stream == null)
            {
                await ReplyAsync("No cat found =(");
                return;
            }

            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        // [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
        [Command("echo")] 
        public Task EchoAsync([Remainder] string text)
            // Insert a ZWSP before the text to prevent triggering other bots!
            => ReplyAsync('\u200B' + text);

        // Search for something
        [Command("Search", RunMode = RunMode.Async)]
        [Summary("Searches for an ansver in the internet")]
        [Alias("google", "find", "гугл", "поиск", "найди")]
        public async Task SearchIt(params string[] query) 
        {
            var queryString = string.Join(" ", query);

            if (string.IsNullOrEmpty(queryString) || string.IsNullOrWhiteSpace(queryString))
            {
                await ReplyAsync(
                     new BotMessageBuilder()
                    .WithEmbedWithTitle(
                        "A blank request is given.\n" +
                        "There is only the great void and nothing else.")
                    .WithEmbedColor(Color.DarkerGrey)
                );
                return;
            }

            var searchResult = await SearchService.SearchAsync(queryString);

            if(searchResult == null)
                return;

            if (searchResult.Error != null)
            {
                await ReplyAsync(
                     new BotMessageBuilder()
                    .WithEmbedWithTitle(searchResult.Error.Message)
                    .WithDisplayType(BotMessageStyle.Error)
                );
                return;
            }

            if (searchResult.Items == null || searchResult.Items.Count == 0)
            {
                await ReplyAsync(
                     new BotMessageBuilder()
                    .WithEmbedWithTitle("No results were found for your search.")
                    .WithEmbedColor(Color.DarkerGrey)
                );
                return;
            }

            foreach (var item in searchResult.Items)
            {
                var page = new EmbedBuilder();
                page.AddField(item.Title, $"[{item.Snippet}]({item.Link})").
                    WithUrl(item.Link);

                if (item.Thumbnail?.Images?.Count > 0)
                    page.WithImageUrl(item.Thumbnail?.Images?.FirstOrDefault().Src);

                await ReplyAsync(embed: page.Build());
            }
        }
    }
}