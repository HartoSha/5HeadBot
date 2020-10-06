using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
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
                await user.Guild.AddBanAsync(user, reason: reason);
                await ReplyAsync("ok!");
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
        public async Task UnBanUserAsync(IGuildUser user)
        {
            IBan ban = null;
            try
            {
                ban = await user.Guild.GetBanAsync(user);
            }
            catch { }
            if (ban != null)
            {
                await user.Guild.RemoveBanAsync(user);
                await ReplyAsync($"Sucsessfuly unbanned {user}");
            }
            else
            {
                await ReplyAsync($"User {user} is not banned.\nWhy to unban him? :thinking:");
            }
        }
    }
}
