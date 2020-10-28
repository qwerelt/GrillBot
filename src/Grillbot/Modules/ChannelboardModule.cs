using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;
using Grillbot.Extensions.Discord;
using Grillbot.Services.Channelboard;
using Grillbot.Models.Embed;
using Grillbot.Extensions;
using Grillbot.Attributes;

namespace Grillbot.Modules
{
    [Group("channelboard")]
    [Name("Channel leaderboards")]
    [ModuleID("ChannelboardModule")]
    public class ChannelboardModule : BotModuleBase
    {
        private ChannelStats Stats { get; }
        private ChannelboardWeb ChannelboardWeb { get; }

        public ChannelboardModule(ChannelStats channelStats, ChannelboardWeb channelboardWeb)
        {
            Stats = channelStats;
            ChannelboardWeb = channelboardWeb;
        }

        [Command("")]
        [Summary("Počet zpráv v kanálech na serveru.")]
        [Remarks("Posílá statistiku do PM.")]
        public async Task ChannelboardAsync()
        {
            var data = await Stats.GetChannelboardDataAsync(Context.Guild, Context.User, ChannelStats.ChannelboardTakeTop);

            if (data.Count == 0)
                await Context.Message.Author.SendPrivateMessageAsync("Ještě nejsou zaznamenány žádné kanály pro tento server.");

            var embed = new BotEmbed(Context.User, null, "Channel leaderboard");

            var messageBuilder = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                var channelBoardItem = data[i];

                messageBuilder
                    .Append(i + 1).Append(": ").Append(channelBoardItem.Channel.Name)
                    .Append(" - ").AppendLine(channelBoardItem.Count.FormatWithSpaces());
            }

            embed.AddField("=======================", messageBuilder.ToString(), false);
            await Context.Message.Author.SendPrivateMessageAsync(embedBuilder: embed.GetBuilder());
        }

        [Command("web")]
        [Summary("Webový leaderboard.")]
        public async Task ChannelboardWebAsync()
        {
            var url = ChannelboardWeb.GetWebUrl(Context);

            var message = $"Tady máš odkaz na channelboard serveru **{Context.Guild.Name}**: {url}.";
            await Context.Message.Author.SendPrivateMessageAsync(message).ConfigureAwait(false);
        }

        [Command("")]
        [Summary("Počet zpráv v místnosti.")]
        public async Task ChannelboardForRoomAsync(IChannel channel)
        {
            var value = await Stats.GetValueAsync(Context.Guild, channel.Id, Context.User);

            if (value == null)
            {
                await ReplyAsync("Do této místnosti nemáš dostatečná práva.");
                return;
            }

            var formatedMessageCount = value.Item2.FormatWithSpaces();
            var message = $"Aktuální počet zpráv v místnosti **#{channel.Name}** je **{formatedMessageCount}** a v příčce se drží na **{value.Item1}**. pozici.";

            await Context.Message.Author.SendPrivateMessageAsync(message);
            await Context.Message.DeleteAsync(new RequestOptions() { AuditLogReason = "Channelboard security" }).ConfigureAwait(false);
        }

        [Command("clear")]
        [Summary("Úklid starých kanálů.")]
        public async Task CleanOldChannels()
        {
            var clearedChannels = await Stats.CleanOldChannels(Context.Guild);

            if (clearedChannels == null)
            {
                await ReplyAsync("> Není co čistit.");
                return;
            }

            await ReplyChunkedAsync(clearedChannels, 10);
            await ReplyAsync("> Čištění dokončeno.").ConfigureAwait(false);
        }

        protected override void AfterExecute(CommandInfo command)
        {
            ChannelboardWeb.Dispose();

            base.AfterExecute(command);
        }
    }
}