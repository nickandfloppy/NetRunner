using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Util;
using HBot.Commands.Attributes;

using ImageMagick;

namespace HBot.Commands.Images {
    public class ReverseCommand : BaseCommandModule {
        [Command("reverse")]
        [Description("Reverse a gif, because why not?")]
        [Usage("[gif]")]
        [Category(Category.Images)]
        public async Task Reverse(CommandContext context, [RemainingText] string input = null) {
            // Check for input URL
            string url = null;
            if (!string.IsNullOrWhiteSpace(input)) {
                url = input;
            }
            else {
                // Check for image in reply
                var message = await context.Channel.GetMessageAsync(context.Message.ReferencedMessage?.Id ?? context.Message.Id);
                if (message == null || message.Attachments.Count == 0) {
                    throw new ArgumentException("Please provide a valid image URL or reply to an image.");
                }
                url = message.Attachments.FirstOrDefault()?.Url;
                if (url == null) {
                    throw new ArgumentException("Please provide a valid image URL or reply to an image.");
                }
            }

            // Handle arguments
            ImageArgs args = ImageCommandParser.ParseArgs(context, url);
            string tempImgFile = TempManager.GetTempFile(Path.GetRandomFileName() + "-reverseDL." + args.extension, true);

            if (args.extension.ToLower() != "gif") {
                throw new Exception("Image provided is not a gif!");
            }

            // Download the image
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync(args.url);
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = File.Create(tempImgFile)) {
                    await stream.CopyToAsync(fileStream);
                }
            }

            var msg = await context.ReplyAsync("Processing...\nThis may take a while depending on the image size");

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
                        await context.Channel.SendFileAsync(imgStream, "reverse." + args.extension);
                    }
                }
            }
            catch (Exception ex) {
                await context.Channel.SendMessageAsync($"Error processing image: {ex.Message}");
            }
            finally {
                await msg.DeleteAsync();
            }
        }
    }
}