using Discord.Commands;
using Grillbot.Extensions.Discord;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Discord;
using Grillbot.Extensions;
using Grillbot.Services;
using Grillbot.Attributes;
using Grillbot.Models.Embed.PaginatedEmbed;
using Grillbot.Services.UserManagement;

namespace Grillbot.Modules
{
    [Group("birthday")]
    [Name("Narozeniny")]
    [ModuleID("BirthdaysModule")]
    public class BirthdaysModule : BotModuleBase
    {
        public BirthdaysModule(IServiceProvider provider, PaginationService paginationService) : base(paginationService, provider)
        {
        }

        [Command("")]
        public async Task GetTodayBirthdayAsync()
        {
            using var service = GetService<BirthdayService>();
            var birthdayUsers = await service.Service.GetUsersWithTodayBirthdayAsync(Context.Guild);

            if (birthdayUsers.Count == 0)
            {
                await ReplyAsync("Dnes nemá nikdo narozeniny.");
                return;
            }

            var pages = new List<PaginatedEmbedPage>();

            foreach (var user in birthdayUsers)
            {
                var page = new PaginatedEmbedPage($"**{user.User.GetFullName()}**", thumbnail: user.User.GetUserAvatarUrl());

                if(user.Birthday.Value.Year > 1)
                    page.AddField(new EmbedFieldBuilder().WithName("Věk").WithValue(user.Birthday.Value.ComputeDateAge()));

                pages.Add(page);
            }

            var embed = new PaginatedEmbed()
            {
                Pages = pages,
                ResponseFor = Context.User,
                Title = "Dnes má narozeniny"
            };

            await SendPaginatedEmbedAsync(embed);
        }

        [Command("add")]
        [Summary("Přidání svého data narození.")]
        [Remarks("Parametr date je váš datum narození, můžete jej zadat ve formátu dd/MM/yyyy, nebo dd/MM.\nPokud zadáte i rok, tak bude zobrazován i váš věk.")]
        public async Task AddBirthdayAsync(string date)
        {
            try
            {
                using var service = GetService<BirthdayService>();

                if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    await service.Service.SetBirthdayAsync(Context.Guild, Context.User, dateTime, true);
                }
                else if (DateTime.TryParseExact(date, "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    await service.Service.SetBirthdayAsync(Context.Guild, Context.User, dateTime, false);
                }
                else
                {
                    throw new ValidationException("Neplatný formát data a času. Povolené jsou pouze `dd/MM/yyyy`, nebo `dd/MM`");
                }

                await ReplyAsync("Datum narození bylo úspěšně přidáno.");
            }
            catch (ValidationException ex)
            {
                await ReplyAsync(ex.Message);
            }

            await Context.Message.DeleteAsync(new RequestOptions() { AuditLogReason = $"Birthday create for {Context.User.GetShortName()}" });
        }

        [Command("remove")]
        [Summary("Odebrání data narození.")]
        public async Task RemoveAsync()
        {
            try
            {
                using var service = GetService<BirthdayService>();
                await service.Service.ClearBirthdayAsync(Context.Guild, Context.User);
                await ReplyAsync("Datum narození bylo odebráno.");
            }
            catch (ValidationException ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("have?")]
        [Summary("Mám uložené narozeniny?")]
        public async Task Have()
        {
            using var service = GetService<BirthdayService>();

            var have = await service.Service.HaveUserBirthday(Context.Guild, Context.User);
            await ReplyAsync($"{Context.User.Mention} {(have ? "máš" : "nemáš")} uložené narozeniny.");
        }
    }
}
