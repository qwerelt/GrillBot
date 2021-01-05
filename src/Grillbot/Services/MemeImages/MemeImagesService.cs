using Discord;
using Discord.WebSocket;
using Grillbot.Database;
using Grillbot.Extensions;
using Grillbot.Helpers;
using Grillbot.Models.Config.Dynamic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Img = System.Drawing.Image;

namespace Grillbot.Services.MemeImages
{
    public class MemeImagesService
    {
        private IGrillBotRepository GrillBotRepository { get; }
        private Random Random { get; }

        public MemeImagesService(IGrillBotRepository grillbotRepository)
        {
            GrillBotRepository = grillbotRepository;
            Random = new Random();
        }

        public async Task<byte[]> GetRandomFileAsync(SocketGuild guild, string category)
        {
            var config = await GrillBotRepository.ConfigRepository.FindConfigAsync(guild.Id, "", category, false);
            var configData = config.GetData<MemeImagesConfig>();

            var filenamesQuery = GrillBotRepository.FilesRepository.GetFilenames().Where(o => o.StartsWith($"{category}_"));

            var filenames = await filenamesQuery
                .AsAsyncEnumerable()
                .Where(_ => configData.AllowedImageTypes.Any(type => type == Path.GetExtension(type)))
                .ToListAsync();

            if (filenames.Count == 0)
                return null;

            var filename = filenames[Random.Next(filenames.Count)];
            var file = await GrillBotRepository.FilesRepository.GetFileAsync(filename);

            return file?.Content;
        }

        public async Task<Img> CreatePeepoloveAsync(IUser forUser, PeepoloveConfig config)
        {
            using var profileImage = await UserHelper.DownloadProfilePictureAsync(forUser, config.ProfilePicSize, true);

            // Drawing canvas
            using var body = new Bitmap(config.BodyPath);
            using var graphics = Graphics.FromImage(body);

            graphics.RotateTransform(-config.Rotate);
            graphics.DrawImage(profileImage, config.ProfilePicRect);
            graphics.RotateTransform(config.Rotate);
            graphics.DrawImage(Img.FromFile(config.HandsPath), config.Screen);

            graphics.DrawImage(body, new Point(0, 0));
            return (body as Img).CropImage(config.CropRect);
        }

        public Task<Bitmap> PeepoAngryAsync(IUser forUser, PeepoAngryConfig config)
        {
            var renderer = new PeepoAngryRenderer();
            return renderer.RenderAsync(forUser, config);
        }

        public Task<Bitmap> RouskaAsync(IUser forUser, RouskaConfig config)
        {
            var renderer = new RouskaRenderer();
            return renderer.RenderAsync(forUser, config);
        }
    }
}
