using Discord;
using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Modules
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {

        // Get info on a user, or the user who invoked the command if one is not specified
        [Command("userinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user = user ?? Context.User;

            await ReplyAsync(user.ToString());
        }

        // Ban a user
        [Command("ban")]
        [Alias("бан")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(
            GuildPermission.BanMembers, 
            ErrorMessage = "User executing this command must be able to ban")]
        // make sure the bot itself can ban
        [RequireBotPermission(
            GuildPermission.BanMembers, 
            ErrorMessage = "Bot executing this command must be able to ban")]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            if (user == Context.User || user.IsBot)
                await ReplyAsync("No. :rage: ");
            else
            {
                var reasonProved = !string.IsNullOrEmpty(reason) && !string.IsNullOrWhiteSpace(reason);
                try
                {
                    await user.SendMessageAsync($"You have been banned on a server {Context.Guild.Name}. {(reasonProved ? $"Reason: {reason}" : "")}");
                }
                catch { }
                await user.Guild.AddBanAsync(user, reason: reason);
                await ReplyAsync($"User {user} has been successfully banned {(reasonProved ? $"for a reason: {reason}" : ".")}");
            }
        }

        // Ban a user
        [Command("unban")]
        [Alias("разбан", "разбань")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(
            GuildPermission.BanMembers, 
            ErrorMessage = "User executing this command must be able to ban")]
        // make sure the bot itself can ban
        [RequireBotPermission(
            GuildPermission.BanMembers, 
            ErrorMessage = "Bot executing this command must be able to ban")]
        public async Task UnbanUserAsync(string uName)
        {
            var name = uName.ToLower();
            var bans = await Context.Guild.GetBansAsync();
            var ban = bans.Where((ban) => ban.User.Username.ToLower() == name)?.FirstOrDefault();
            if(ban != null)
            {
                await Context.Guild.RemoveBanAsync(ban.User);
                await ReplyAsync($"Successfully unbanned {ban.User}! Now you can invite him via link: {(await Context.Guild.GetInvitesAsync()).FirstOrDefault().Url}");
            }
            else
            {
                await ReplyAsync($"User {name} is not banned.\nWhy to unban him? :thinking:");
            }
        }

        // Sends direct message to a user
        [Command("dm")]
        public async Task DmUser(IUser user = null, string message = "?")
        {
            user = user ?? Context.User;
            await user.SendMessageAsync(message);
        }
    }
}
