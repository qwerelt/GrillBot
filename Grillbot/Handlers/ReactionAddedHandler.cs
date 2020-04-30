﻿using Discord;
using Discord.WebSocket;
using Grillbot.Services;
using Grillbot.Services.Initiable;
using Grillbot.Services.Statistics;
using System;
using System.Threading.Tasks;

namespace Grillbot.Handlers
{
    public class ReactionAddedHandler : IDisposable, IInitiable
    {
        private DiscordSocketClient Client { get; }
        private EmoteStats EmoteStats { get; }
        private InternalStatistics InternalStatistics { get; }
        private PaginationService PaginationService { get; }

        public ReactionAddedHandler(DiscordSocketClient client, EmoteStats emoteStats, InternalStatistics internalStatistics,
            PaginationService paginationService)
        {
            Client = client;
            EmoteStats = emoteStats;
            InternalStatistics = internalStatistics;
            PaginationService = paginationService;
        }

        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            InternalStatistics.IncrementEvent("ReactionAdded");
            await EmoteStats.IncrementFromReaction(reaction);
            await PaginationService.HandleReactionAsync(reaction);
        }

        public void Dispose()
        {
            Client.ReactionAdded -= OnReactionAddedAsync;
        }

        public void Init()
        {
            Client.ReactionAdded += OnReactionAddedAsync;
        }

        public async Task InitAsync() { }
    }
}