using _5HeadBot.Modules.Internal;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    public class HelpModule : MessageSenderModuleBase
    {
        private CommandService Commands { get; }
        public HelpModule(CommandService commands)
        {
            Commands = commands;
        }
        // Get list of all avaliable commands
        [Command("help")]
        [Summary("Returns list of all commands")]
        [Alias("команды")]
        public Task Help()
        {
            var groups = Commands.Modules.SelectMany(m => m.Commands).GroupBy(c => c.Module.Aliases);
            var messages = new List<Embed>();

            foreach (var g in groups)
            {
                var embed = new EmbedBuilder();

                if (g.Key.Where(name => name.Length > 0).Count() > 0)
                {
                    embed.WithTitle(
                    string.Join(" or ", g.Key.Select(
                            name => string.IsNullOrEmpty(name) ? "`*Without prefix*`" : $"`{name}`"
                        ))
                    );
                }

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

                    var commandArgs = string.Join("\n> ", c.Parameters.Select(arg => !arg.IsOptional ? $"{arg}" : $"[{arg}={(arg.DefaultValue ?? "")}]"));

                    var fieldBuilder = new EmbedFieldBuilder
                    {
                        Name = commandNames,
                        Value = commandArgs.Length > 0 ? $"> {commandArgs}" : "\u200B"
                    };
                    fields.Add(fieldBuilder);
                }

                embed.WithFields(fields)
                    .WithColor(Color.Green);
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
