using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Util;
using HBot.Commands.Attributes;

using ImageMagick;

namespace HBot.Commands.Images {
    public class ResizeCommand : BaseCommandModule {
        [Command("resize")]
        [Description("Resize an image")]
        [Usage("[image] [-scale -size]")]
        [Category(Category.Images)]
        public async Task Resize(CommandContext Context, [RemainingText]string input) {
            // Handle arguments
            ImageArgs args = ImageCommandParser.ParseArgs(Context, input);
            int seed = new System.Random().Next(1000, 99999);
            if(args.scale == 1 && args.size == 1)
                throw new System.Exception("A scale or size must be provided!");

            // Download the image
            string tempImgFile = TempManager.GetTempFile(seed+"-resizeDL."+args.extension, true);
            using (var httpClient = new HttpClient()) {
                using (var response = await httpClient.GetAsync(args.url, HttpCompletionOption.ResponseHeadersRead)) {
                    response.EnsureSuccessStatusCode();
                    using (var stream = await response.Content.ReadAsStreamAsync()) {
                        using (var fileStream = new FileStream(tempImgFile, FileMode.Create, FileAccess.Write, FileShare.None)) {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
            }

            var msg = await Context.ReplyAsync("Processing...\nThis may take a while depending on the image size");

            // R e s i z e
            MagickImage img = null;
            MagickImageCollection gif = null;
            if(args.extension.ToLower() != "gif") {
                img = new MagickImage(tempImgFile);
                if(args.size != 1) 
                    img.Resize(new MagickGeometry(args.size));
                else if(args.scale != 1)
                    img.Scale(args.scale*img.Width, args.scale*img.Height);
            }
            else {
                gif = new MagickImageCollection(tempImgFile);
                foreach(var frame in gif) {
                    if(args.size != 1) 
                        frame.Resize(new MagickGeometry(args.size));
                    else if(args.scale != 1)
                        frame.Scale(args.scale*frame.Width, args.scale*frame.Height);
                }
            }

            TempManager.RemoveTempFile(seed+"-resizeDL."+args.extension);

            // Save the image
            MemoryStream imgStream = new MemoryStream();
            if(args.extension.ToLower() != "gif")
                img.Write(imgStream);
            else
                gif.Write(imgStream);
            imgStream.Position = 0;

            // Send the image
            await msg.ModifyAsync("Uploading...\nThis may take a while depending on the image size");
            await Context.Channel.SendFileAsync(imgStream, "resized."+args.extension);
            await msg.DeleteAsync();
        }
    }
}
