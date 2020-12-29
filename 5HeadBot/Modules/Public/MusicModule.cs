using _5HeadBot.Modules.Internal;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using _5HeadBot.Services.Feature.MusicService;
using _5HeadBot.Services.Feature.MusicService.Attributes;
using _5HeadBot.Services.Feature.MusicService.Data;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    [Group("Music")]
    [Alias("Mus", "M")]
    [RequireUserToBeInAVoiceChannel(ErrorMessage = "You shold be in a voice channel to use this command.")]
    [RequireContext(ContextType.Guild, ErrorMessage = "You can use music commads only in guild.")]
    [RequireBotPermission(GuildPermission.Connect, ErrorMessage = "Bot should have permission to connect to a voice channel.")]
    [RequireBotPermission(GuildPermission.Speak, ErrorMessage = "Bot should have permission to speak.")]
    public class MusicModule : MessageSenderModuleBase
    {
        private readonly IMusicService _musicService;
        public MusicModule(IMusicService musicService)
        {
            _musicService = musicService;
        }
        
        [Command("Join")]
        public async Task Join()
        {
            var channelOfTheUser = (Context.User as IVoiceState)?.VoiceChannel;
            if((await channelOfTheUser.GetUserAsync(Context.Client.CurrentUser.Id)) != null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithDisplayType(BotMessageStyle.Info)
                    .WithEmbedWithTitle("I'm in the voice channel."));
                return;
            }

            await _musicService.JoinAsync(channelOfTheUser);
        }

        [RequireBotToBeInAVoiceChannel(ErrorMessage = "Bot should be in a voice channel. Consider using `join` command.")]
        [RequireUserAndBotToBeInTheSameVoiceChannel(ErrorMessage = "You must be in the same channel as the bot")]
        [Command("Leave")]
        public async Task Leave()
        {
            var channelOfTheUser = (Context.User as IVoiceState)?.VoiceChannel;
            if (channelOfTheUser != null)
                await _musicService.LeaveAsync(channelOfTheUser);
        }

        [RequireBotToBeInAVoiceChannel(ErrorMessage = "Bot should be in a voice channel. Consider using `join` command.")]
        [RequireUserAndBotToBeInTheSameVoiceChannel(ErrorMessage = "You must be in the same channel as the bot")]
        [Command("Play")]
        [Alias("Add")]
        public async Task Play(params string[] query)
        {
            var voiceChannel = (Context.User as IVoiceState)?.VoiceChannel;
            if (query.Length == 0)
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
            
            var addedTrack = await _musicService.PlayAsync(string.Join(" ", query), voiceChannel);
            var inQueueCount = (await _musicService.GetQueueAsync(voiceChannel)).Count();
            
            // no such track found
            if (addedTrack is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("No such track found")
                    .WithDisplayType(BotMessageStyle.Warning)
                );
                return;
            }

            if(inQueueCount > 0)
            {
                await ReplyAsync(
                    addedTrack
                    .AsBotMessageBuilder()
                    .WithText("Enqueued:")
                    .WithDisplayType(BotMessageStyle.Success)
                    .WithEmbedWithFooter(new EmbedFooterBuilder().WithText($"Tracks in the queue: {inQueueCount}"))
                );
                return;
            }

            await ReplyAsync(
                addedTrack
                .AsBotMessageBuilder()
                .WithText("Now playing:")
                .WithDisplayType(BotMessageStyle.Success)
            );
        }

        [RequireBotToBeInAVoiceChannel(ErrorMessage = "Bot should be in a voice channel. Consider using `join` command.")]
        [RequireUserAndBotToBeInTheSameVoiceChannel(ErrorMessage = "You must be in the same channel as the bot")]
        [Command("Skip")]
        public async Task Skip()
        {
            var voiceChannel = (Context.User as IVoiceState)?.VoiceChannel;
            var skippedTrack = await _musicService.SkipAsync(voiceChannel);
            if(skippedTrack is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("Can't skip - no more tracks left.")
                    .WithDisplayType(BotMessageStyle.Info)
                );
                return;
            }

            await ReplyAsync(
                skippedTrack
                .AsBotMessageBuilder()
                .WithText("Skipped:")
                .WithEmbedDescription(skippedTrack.AsTimelineString())
                .WithDisplayType(BotMessageStyle.Success)
            );

            var nowPlaying = await _musicService.GetCurrentAsync(voiceChannel);
            if (nowPlaying != null)
            {
                await ReplyAsync(
                    nowPlaying
                    .AsBotMessageBuilder()
                    .WithText("Now playing:")
                    .WithDisplayType(BotMessageStyle.Success)
                );
            }
        }

        [RequireBotToBeInAVoiceChannel(ErrorMessage = "Bot should be in a voice channel. Consider using `join` command.")]
        [Alias("Current", "Now", "Playing")]
        [Command("Track")]
        public async Task CurrentTrack()
        {
            var voiceChannel = (Context.User as IVoiceState)?.VoiceChannel;
            var track = await _musicService.GetCurrentAsync(voiceChannel);
            if(track is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("Nothing playing.")
                    .WithDisplayType(BotMessageStyle.Info)
                );
                return;
            }

            await ReplyAsync(
                (await _musicService.GetCurrentAsync(voiceChannel))
                .AsBotMessageBuilder()
                .WithText("Now playing:")
                .WithEmbedDescription(track.AsTimelineString())
                .WithDisplayType(BotMessageStyle.Success)
            );
        }

        [RequireBotToBeInAVoiceChannel(ErrorMessage = "Bot should be in a voice channel. Consider using `join` command.")]
        [RequireUserAndBotToBeInTheSameVoiceChannel(ErrorMessage = "You must be in the same channel as the bot")]
        [Command("Volume")]
        public async Task SetVolume(string volume = "100")
        {
            if (!int.TryParse(volume, out int volumeIntValue))
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("The volume must be a number between 0 and 100.")
                    .WithDisplayType(BotMessageStyle.Warning)
                );
                return;
            }

            var voiceChannel = (Context.User as IVoiceState)?.VoiceChannel;
            await _musicService.SetVolumeAsync(voiceChannel, volumeIntValue / 100f);

            await ReplyAsync(
                new BotMessageBuilder()
                .WithEmbedWithTitle(
                    // if volume is 0 or below show :mute:
                    // if between 0 and 100 show :speaker:
                    // if above 100 show :speaker: :boom:
                    $"{(volumeIntValue <= 0 ? ":mute:" : $":speaker: {(volumeIntValue > 100 ? ":boom:" : "")}")} " +
                    $"Volume is set to {volumeIntValue}.")
                .WithDisplayType(BotMessageStyle.Info)
            );
        }
    }
}
