using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using RestSharp;

using Newtonsoft.Json;

namespace HBot.Commands.Main {
    public class WeatherCommand : BaseCommandModule {
        [Command("weather")]
        [Description("Get the weather for a specific location")]
        [Usage("[location]")]
        [Category(Category.Main)]
        public async Task Weather(CommandContext Context, [RemainingText]string location) {
            if(string.IsNullOrWhiteSpace(location))
                throw new System.Exception("You must provide a location!");

            // Pull data from the API
            RestClient client = new RestClient($"http://api.weatherapi.com/v1/forecast.json?key={Bot.config.apiKeys.weatherAPI}&q={location.Replace(" ", "%20")}");
            RestRequest request = new RestRequest();
            RestResponse response = (RestResponse)client.Execute(request);
            if (!response.IsSuccessful) {
                await Context.ReplyAsync("Unable to get the weather for that location, are you sure it exists?");
                return;
            }
            dynamic data = JsonConvert.DeserializeObject(response.Content);

            // Create and send the embed
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle($"Weather for {data.location.name}, {data.location.region}");
            eb.WithColor(DiscordColor.Gold);
            eb.WithFooter($"Last updated: {data.current.last_updated}");
            eb.WithDescription($"***{data.current.condition.text}***");
            eb.WithThumbnail($"https://{((string)data.current.condition.icon).Replace("//", "")}");

            // F i e l d s
            eb.AddField("Time", $"{data.location.localtime} ({data.location.tz_id})", true);
            eb.AddField("Coordinates", $"***Longitude:*** {data.location.lon}, ***Latitude:*** {data.location.lat}", true);
            eb.AddField("Country", $"{data.location.country}", true);
            eb.AddField("Temperature", $"{data.current.temp_c}°C ({data.current.temp_f}°F)", true);
            eb.AddField("Feels like", $"{data.current.feelslike_c}°C ({data.current.feelslike_f}°F)", true);
            eb.AddField("Humidity", $"{data.current.humidity}%", true);
            eb.AddField("Precipitation", $"{data.current.precip_mm}mm ({data.current.precip_in}in)", true);
            eb.AddField("Wind speed", $"{data.current.wind_kph} km/h ({data.current.wind_mph} mph) with gusts up to {data.current.gust_kph} km/h ({data.current.gust_mph} mph)", true);
            eb.AddField("Wind direction", $"{data.current.wind_dir}", true);

            await Context.ReplyAsync("", eb.Build());
        }
    }
}