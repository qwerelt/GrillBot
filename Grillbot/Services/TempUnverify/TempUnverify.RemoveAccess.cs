﻿using Discord;
using Discord.WebSocket;
using Grillbot.Extensions.Discord;
using Grillbot.Database.Entity;
using Grillbot.Database.Entity.UnverifyLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grillbot.Services.TempUnverify
{
    public partial class TempUnverifyService
    {
        public async Task<string> RemoveAccessAsync(List<SocketGuildUser> users, string time, string data, SocketGuild guild,
            SocketUser fromUser, bool ignoreHigherRoles = false)
        {
            var checker = Factories.GetChecker();
            var currentUnverified = GetCurrentUnverifiedUserIDs();

            foreach (var user in users)
            {
                checker.Validate(user, guild, false, null, currentUnverified);
            }

            var reason = ParseReason(data);
            var unverifyTime = ParseUnverifyTime(time);
            var unverifiedPersons = new List<TempUnverifyItem>();

            foreach (var user in users)
            {
                var person = await RemoveAccessAsync(user, unverifyTime, reason, fromUser, guild, ignoreHigherRoles, null);
                unverifiedPersons.Add(person);
            }

            unverifiedPersons.ForEach(o => o.InitTimer(ReturnAccess));
            Data.AddRange(unverifiedPersons);

            return FormatMessageToChannel(users, unverifiedPersons, reason);
        }

        private async Task<TempUnverifyItem> RemoveAccessAsync(SocketGuildUser user, int unverifyTime, string reason,
            SocketUser fromUser, SocketGuild guild, bool ignoreHigherRoles, string[] subjects)
        {
            var helper = Factories.GetHelper();

            var rolesToRemove = user.Roles
                .Where(o => !o.IsEveryone && !o.IsManaged && !string.Equals(o.Name, "muted", StringComparison.InvariantCultureIgnoreCase))
                .ToList(); // Ignore Muted roles.

            if (ignoreHigherRoles)
            {
                var botMaxRolePosition = guild.GetUser(Client.CurrentUser.Id).Roles.Max(o => o.Position);
                rolesToRemove = rolesToRemove.Where(o => o.Position < botMaxRolePosition).ToList();
            }

            if (subjects != null && subjects.Length > 0)
            {
                rolesToRemove = rolesToRemove.Where(role => !subjects.Contains(role.Name.ToLower())).ToList();
            }

            var rolesToRemoveIDs = rolesToRemove.Select(o => o.Id).ToList();
            var overrides = GetChannelOverrides(user);

            var data = new UnverifyLogSet()
            {
                Overrides = overrides,
                Roles = rolesToRemoveIDs,
                StartAt = DateTime.Now,
                TimeFor = unverifyTime.ToString(),
                Reason = reason
            };

            data.SetUser(user);
            using var repository = Factories.GetUnverifyRepository();
            await repository.LogOperationAsync(UnverifyLogOperation.Set, fromUser, guild, data).ConfigureAwait(false);

            if (subjects == null || subjects.Length == 0)
                await helper.FindAndToggleMutedRoleAsync(user, guild, true);

            await PreRemoveAccessToPublicChannels(user, guild).ConfigureAwait(false); // Set SendMessage: Deny for extra channels.
            await user.RemoveRolesAsync(rolesToRemove).ConfigureAwait(false); // Remove all roles for user.

            // Remove all extra channel permissions.
            foreach (var channelOverride in overrides)
            {
                var channel = user.Guild.GetChannel(channelOverride.ChannelIdSnowflake);

                // Where had access, now not have.
                channel?.AddPermissionOverwriteAsync(user, new OverwritePermissions(viewChannel: PermValue.Deny));
            }

            var unverify = await repository
                .AddItemAsync(rolesToRemoveIDs, user.Id, user.Guild.Id, unverifyTime, overrides, reason)
                .ConfigureAwait(false);

            var formatedPrivateMessage = GetFormatedPrivateMessage(user, unverify, reason, false);
            await user.SendPrivateMessageAsync(formatedPrivateMessage).ConfigureAwait(false);
            return unverify;
        }
    }
}
