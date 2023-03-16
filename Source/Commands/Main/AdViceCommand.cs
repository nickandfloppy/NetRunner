using System;
using System.Threading.Tasks;
using System.Net.Http;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class AdviceCommand : BaseCommandModule {
        [Command("advice")]
        [Description("You can't go to a therapist so you rely on a Discord bot for life advice... why???")]
        [Category(Category.Main)]
        public async Task Advice(CommandContext ctx) {
            using (var client = new HttpClient()) {
                HttpResponseMessage response = await client.GetAsync("http://api.adviceslip.com/advice");
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) {
                    try {
                        JsonConvert.DeserializeObject(responseString);
                    }
                    catch (Exception) {
                        await ctx.RespondAsync("An API error occurred.");
                        return;
                    }

                    var advice = JsonConvert.DeserializeObject<dynamic>(responseString);
                    await ctx.RespondAsync(advice.slip.advice.ToString());
                }
                else {
                    Console.Error.WriteLine($"REST call failed: {response.StatusCode}");
                }
            }
        }
    }
}