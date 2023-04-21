using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Util;
using HBot.Commands.Attributes;

using ImageMagick;

namespace HBot.Commands.Images {
    public class ReverseCommand : BaseCommandModule {
        [Command("reverse")]
        [Description("Reverse a gif, because why not?")]
        [Usage("[gif]")]
        [Category(Category.Images)]
        public async Task Reverse(CommandContext Context, [RemainingText]string input) {
            // Check for input URL
            if (string.IsNullOrEmpty(input)) {
                throw new ArgumentException("Please provide a valid image URL.");
            }

            // Handle arguments
            ImageArgs args = ImageCommandParser.ParseArgs(Context, input);
            string tempImgFile = TempManager.GetTempFile(Path.GetRandomFileName() + "-reverseDL." + args.extension, true);

            if(args.extension.ToLower() != "gif")
                throw new Exception("Image provided is not a gif!");

            // Download the image
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync(args.url);
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = File.Create(tempImgFile)) {
                    await stream.CopyToAsync(fileStream);
                }
            }

            var msg = await Context.ReplyAsync("Processing...\nThis may take a while depending on the image size");

            try {
                // Add s p e e d
                using (var gif = new MagickImageCollection(tempImgFile)) {
                    gif.Reverse();
                    TempManager.RemoveTempFile(Path.GetFileName(tempImgFile));

                    // Save the image
                    using (var imgStream = new MemoryStream()) {
                        gif.Write(imgStream);
                        imgStream.Position = 0;

                        // Send the image
                        await msg.ModifyAsync("Uploading...\nThis may take a while depending on the image size");
                        await Context.Channel.SendFileAsync(imgStream, "reverse." + args.extension);
                    }
                }
            }
            catch (Exception ex) {
                await Context.Channel.SendMessageAsync($"Error processing image: {ex.Message}");
            }
            finally {
                await msg.DeleteAsync();
            }
        }
    }
}
