using _5HeadBot.Modules.Internal;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using Discord;
using Discord.Commands;
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
        [Command("help", RunMode = RunMode.Async)]
        [Summary("Returns list of all commands")]
        [Alias("команды")]
        public async Task Help()
        {
            // key - module command belongs to.
            // value - list of all commands in the module
            var dict = new Dictionary<ModuleInfo, List<CommandInfo>>();

            foreach (var c in Commands.Commands)
            {
                if (dict.ContainsKey(c.Module))
                    dict[c.Module].Add(c);

                else dict[c.Module] = new List<CommandInfo>() { c };
            }

            static string asCode(string s) => $"`{s}`";
            foreach (var kvp in dict)
            {
                var fields = kvp.Value.Select(
                    cInfo => new EmbedFieldBuilder()
                        // in order to get all command's aliases without prefixes
                        // we take only commandAlliasesCount / moduleAlliasesCount first command's aliases
                        .WithName(string.Join(", ", cInfo.Aliases.OrderBy(a => a.Length).Take(cInfo.Aliases.Count / (kvp.Key.Aliases.Count > 0 ? kvp.Key.Aliases.Count : 1)).Select(str => asCode(str))))
                        .WithValue(string.Join("\n ", cInfo.Parameters.Select(arg => !arg.IsOptional ? $"> {arg}" : $"> [{arg}={arg.DefaultValue ?? ""}]")) + "\u200B")
                ).ToList();

                await ReplyAsync(new BotMessageBuilder()
                    // add command prefixes as title
                    .WithEmbedWithTitle(string.Join(" or ", kvp.Key.Aliases?.Select(a => string.IsNullOrEmpty(a) ? "`*Without prefix*`" : asCode(a))))
                    // add commands with their arguments as fields
                    .WithEmbedWithFields(fields)
                    .WithDisplayType(BotMessageStyle.Success)
                    .Build());
            }
        }
    }
}
