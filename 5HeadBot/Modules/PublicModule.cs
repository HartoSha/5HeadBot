using _5HeadBot.Services;
using _5HeadBot.Services.PictureService;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public PictureService PictureService { get; set; }
        public SearchService SearchService { get; set; }
        public CommandService Commands { get; set; }

        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("pong!");

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

        // Setting a custom ErrorMessage property will help clarify the precondition error
        [Command("guild_only")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        public Task GuildOnlyCommand()
            => ReplyAsync("Nothing to see here!");

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
                    "A blank request is given.\n" +
                    "There is only the great void and nothing else."
                );
                return;
            }

            var searchResult = await SearchService.SearchAsync(queryString);

            if(searchResult == null)
                return;

            if (searchResult.Error != null)
            {
                await ReplyAsync(searchResult.Error.Message);
                return;
            }

            if (searchResult.Items == null || searchResult.Items.Count == 0)
            {
                await ReplyAsync("No results were found for your search.");
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

        // Get list of all avaliable commands
        [Command("help")]
        [Summary("Returns list of all commands")]
        [Alias("команды")]
        public Task Help()
        {
            var g2 = Commands.Modules.SelectMany(m => m.Commands).GroupBy(c => c.Module.Aliases);
            var groups = Commands.Modules.GroupBy((module) => module.Commands.SelectMany(c => c.Aliases));
            var messages = new List<Embed>();

            foreach (var g in g2)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle(string.Join(" or ", g.Key.Select(name => string.IsNullOrEmpty(name) ? "~~<No prefix>~~" : $"`{name}`").Reverse()));

                var fields = new List<EmbedFieldBuilder>();
                foreach (var c in g)
                {
                    // create a set of command names and prefixes, remove prefixes
                    // создаем множество с названиями и префиксами текущей команды, удаляем из него префиксы
                    var alliasesWithoutGroupPrefixes =
                                    c.Aliases != null && c.Aliases.Count > 0
                                    ? c.Aliases.SelectMany(a => a.Split(" ")).ToHashSet().Where(el => el.ToLower().Equals(c.Name.ToLower()) || !g.Key.Select(e => e.ToLower()).Contains(el.ToLower()))
                                    : c.Aliases.ToHashSet();

                    var styledAlliases = alliasesWithoutGroupPrefixes.Select(a => $"`{a}`");
                    var commandNames = $"{string.Join(", ", styledAlliases)}{(string.IsNullOrEmpty(c.Summary) ? "" : $" - {c.Summary}")}";

                    var commandArgs = string.Join(", ", c.Parameters.Select(arg => !arg.IsOptional ? $"***{arg}***" : $"[***{arg}***={(arg.DefaultValue ?? "")}]"));

                    var fieldBuilder = new EmbedFieldBuilder
                    {
                        Name = commandNames,
                        Value = commandArgs.Length > 0 ? commandArgs : "\u200B"
                    };
                    fields.Add(fieldBuilder);
                }

                embed.WithFields(fields);
                messages.Add(embed.Build());
            }

            foreach (var mess in messages)
            {
                ReplyAsync(embed: mess);
            }
            return Task.CompletedTask;
        }
    }
}