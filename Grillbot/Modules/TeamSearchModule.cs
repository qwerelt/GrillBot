using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Grillbot.Database.Repository;
using Grillbot.Exceptions;
using Grillbot.Models.Config.AppSettings;
using Grillbot.Models.PaginatedEmbed;
using Grillbot.Services;
using Grillbot.Services.Preconditions;
using Grillbot.Services.TeamSearch;
using Microsoft.Extensions.Options;

namespace Grillbot.Modules
{
    [Group("hledam")]
    [RequirePermissions]
    [Name("Hledání týmů")]
    public class TeamSearchModule : BotModuleBase
    {
        private TeamSearchService TeamSearchService { get; }

        public TeamSearchModule(IOptions<Configuration> options, ConfigRepository configRepository, TeamSearchService teamSearchService,
            PaginationService paginationService)
            : base(options, configRepository, paginationService)
        {
            TeamSearchService = teamSearchService;
        }

        [Command("add")]
        [Summary("Přidá zprávu o hledání.")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task LookingForTeamAsync([Remainder] string messageToAdd)
        {
            try
            {
                TeamSearchService.CreateSearch(Context.Guild, Context.User, Context.Channel, Context.Message);
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            catch
            {
                await Context.Message.AddReactionAsync(new Emoji("❌"));
                throw;
            }
        }

        [Command("")]
        [Summary("Vypíše seznam hledání.")]
        public async Task TeamSearchInfoAsync()
        {
            var searches = await TeamSearchService.GetItemsAsync(Context.Channel.Id.ToString());

            if (searches.Count == 0)
                throw new BotCommandInfoException("Zatím nikdo nic nehledá.");

            var pages = new List<PaginatedEmbedPage>();
            var currentPage = new List<EmbedFieldBuilder>();

            foreach (var search in searches)
            {
                var builder = new EmbedFieldBuilder()
                    .WithName($"**{search.ID}**  - **{search.ShortUsername}** v **{search.ChannelName}**")
                    .WithValue($"\"{search.Message}\" [Jump]({search.MessageLink})");

                currentPage.Add(builder);

                if(currentPage.Count == EmbedBuilder.MaxFieldCount)
                {
                    pages.Add(new PaginatedEmbedPage(null, new List<EmbedFieldBuilder>(currentPage)));
                    currentPage.Clear();
                }
            }

            if (currentPage.Count != 0)
                pages.Add(new PaginatedEmbedPage(null, new List<EmbedFieldBuilder>(currentPage)));

            var embed = new PaginatedEmbed()
            {
                Pages = pages,
                ResponseFor = Context.User,
                Title = $"Hledání v {Context.Channel.Name}"
            };

            await SendPaginatedEmbedAsync(embed);
        }

        [Command("remove")]
        public async Task RemoveTeamSearchAsync(int searchId)
        {
            if (Context.User is SocketGuildUser user)
            {
                TeamSearchService.RemoveSearch(searchId, user);
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
        }

        [Command("cleanChannel")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task CleanChannelAsync(string channel)
        {
            var mentionedChannel = Context.Message.MentionedChannels.FirstOrDefault();

            if (mentionedChannel == null)
                throw new BotCommandInfoException("Nebyl tagnut žádný kanál.");

            await TeamSearchService.BatchCleanChannelAsync(mentionedChannel.Id, async message => await ReplyAsync(message));
            await ReplyAsync($"Čištění kanálu `{mentionedChannel.Name}` dokončeno");
        }

        [Command("massRemove")]
        public async Task MassRemoveAsync(params int[] searchIds)
        {
            await TeamSearchService.BatchCleanAsync(searchIds, async message => await ReplyAsync(message));
            await ReplyAsync("Úklid hledání dokončeno.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                TeamSearchService.Dispose();

            base.Dispose(disposing);
        }
    }
}