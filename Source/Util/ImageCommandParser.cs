using System;
using System.Net;
using System.Linq;

using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace WinBot.Util
{
    class ImageCommandParser
    {
        public static ImageArgs ParseArgs(CommandContext Context, string input)
        {
            ImageArgs args = new ImageArgs();

            // No arguments, No URL
            if(Context.Message.Attachments.Count > 0) {     
                args.url = Context.Message.Attachments[0].Url;
            }
            // Reply
            else if(Context.Message.ReferencedMessage != null) {

                if(Context.Message.ReferencedMessage.Attachments.Count > 0)
                    args.url = Context.Message.ReferencedMessage.Attachments[0].Url;
                else if(Uri.IsWellFormedUriString(Context.Message.ReferencedMessage.Content, UriKind.Absolute))
                    args.url = Context.Message.ReferencedMessage.Content;
            }
            // No arguments, Image URL
            else if(Uri.IsWellFormedUriString(input, UriKind.Absolute)) {
                args.url = input;
                return args;
            }
            // Arguments, Image URL
            else if(!string.IsNullOrWhiteSpace(input) && input.Split(' ').Count() > 1 && !input.Split(' ')[0].StartsWith("-")) {

                string[] splitArgs = input.Split(' ');
                for(int i = 0; i < splitArgs.Length; i++) {
                    
                    // If the current word is a URL
                    if(Uri.IsWellFormedUriString(splitArgs[i], UriKind.Absolute))
                        args.url = splitArgs[i];
                }
            }
            // Recent message
            else {
                var messages = Context.Channel.GetMessagesAsync(30).Result;
                foreach(DiscordMessage msg in messages) {
                    
                    if(msg.Attachments.Count > 0) {
                        args.url = msg.Attachments[0].Url;
                        break;
                    }
                    else if(Uri.IsWellFormedUriString(msg.Content, UriKind.Absolute)) {
                        args.url = msg.Content;
                        break;
                    }
                }
                if(args.url == null)
                    throw new Exception("Invalid or no image! The image has to have been sent in the past 30 messages!");
            }
            args.url = args.url.Split('?')[0];

            if(!string.IsNullOrWhiteSpace(input)) {

                // Handle arguments
                string[] argsStr = input.Split(' ');
                for(int i = 0; i < argsStr.Length; i++) {
                    if(argsStr[i].StartsWith("-")) {

                        // Parse the argument name and value
                        string argName = argsStr[i].Replace("-", "").ToLower();
                        int.TryParse(argName.Split("=").LastOrDefault(), out int argVal);
                        argName = argName.Split("=")[0];

                        // Set the appropriate value
                        if(argName == "layers")
                            args.layers = argVal;
                        else if(argName == "size")
                            args.size = argVal;
                        else if(argName == "scale")
                            args.scale = argVal;
                    }
                }
            }

            // Verify the image
            WebClient client = new WebClient();
            client.OpenRead(args.url);
            Int64 fileSize = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
            if (!client.ResponseHeaders["Content-Type"].Contains("image") || client.ResponseHeaders["Content-Type"].Contains("svg"))
                throw new Exception("Invalid or no image!");
            if(fileSize > 16777216)
                throw new Exception("Your image must be below 16MB in size!");
            args.extension = client.ResponseHeaders["Content-Type"].Split("image/").Last();

            return args;
        }
    }

    class ImageArgs
    {
        public string url { get; set; } = null;
        public string extension { get; set; }
        public int layers { get; set; } = 1;
        public int size { get; set; } = 1;
        public int scale { get; set; } = 1;
    }
}