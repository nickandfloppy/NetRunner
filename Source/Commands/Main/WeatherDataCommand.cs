using System;
using System.Net;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.Main
{
    public class WeatherDataCommand : BaseCommandModule
    {
        [Command("weatherdata")]
        [Description("Pulls data from floppy's weather station")]
        [Usage("(info)")]
        [Category(Category.Main)]
        public async Task WeatherData(CommandContext Context, string command = null)
        {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            if (command == null) {
                string json = "";
                string time = "";

                // Try to grab the json string from the API
                try {
                    using (WebClient client = new WebClient()) {
                        json = client.DownloadString("http://api.weather.diskfloppy.me/v1/current_conditions");
                    }
                    time = DateTime.Now.ToString();
                } catch {
                    try {
                        using (WebClient client = new WebClient()) {
                            json = client.DownloadString("https://weather.diskfloppy.me/data/weatherData.json");
                            time = client.DownloadString("https://weather.diskfloppy.me/data/got.txt");
                        }
                    } catch {
                        eb.WithTitle("Unable to retrieve weather data!");
                        eb.WithColor(new DiscordColor("#ED4245"));
                        return;
                    }
                }
                if (json != "") {
                    dynamic output = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object
                    string outsideTempC = Convert.ToString(Math.Round(Convert.ToDouble((output.data.conditions[0].temp - 32) / 1.8), 1));
                    string outsideTempF = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].temp), 1));
                    string windSpeedMPH = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].wind_speed_last * 1.151), 2));
                    double windSpeedKTS = Math.Round(Convert.ToDouble(output.data.conditions[0].wind_speed_last), 2);
                    string dewPointC = Convert.ToString(Math.Round(Convert.ToDouble((output.data.conditions[0].dew_point - 32 ) / 1.8), 1));
                    string dewPointF = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].dew_point), 1));
                    int winDirDegInt = Convert.ToInt32(output.data.conditions[0].wind_dir_last);
                    string windDirDeg = Convert.ToString(winDirDegInt);
                    string windDir = "";
                    string outsideHum = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].hum), 1));
                    string rainRateIn = Convert.ToString(Math.Round(Convert.ToDouble((output.data.conditions[0].rain_rate_last) * 0.2) / 25.4, 1));
                    string rainRateMM = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rain_rate_last * 0.2), 1));

                    string dailyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_daily) * 0.2, 1));
                    string monthlyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_monthly) * 0.2, 1));
                    string yearlyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_yearly) * 0.2, 1));
                    

                    // Determine wind direction (NESW)
                    if (349 <= winDirDegInt || winDirDegInt <= 11) windDir = "N";
                    else if (12 <= winDirDegInt && winDirDegInt <= 33) windDir = "NNE";
                    else if (34 <= winDirDegInt && winDirDegInt <= 56) windDir = "NE";
                    else if (57 <= winDirDegInt && winDirDegInt <= 78) windDir = "ENE";
                    else if (79 <= winDirDegInt && winDirDegInt <= 101) windDir = "E";
                    else if (102 <= winDirDegInt && winDirDegInt <= 123) windDir = "ESE";
                    else if (124 <= winDirDegInt && winDirDegInt <= 146) windDir = "SE";
                    else if (147 <= winDirDegInt && winDirDegInt <= 168) windDir = "SSE";
                    else if (169 <= winDirDegInt && winDirDegInt <= 191) windDir = "S";
                       else if (192 <= winDirDegInt && winDirDegInt <= 213) windDir = "SSW";
                       else if (214 <= winDirDegInt && winDirDegInt <= 236) windDir = "SW";
                       else if (237 <= winDirDegInt && winDirDegInt <= 258) windDir = "WSW";
                       else if (259 <= winDirDegInt && winDirDegInt <= 281) windDir = "W";
                       else if (282 <= winDirDegInt && winDirDegInt <= 303) windDir = "WNW";
                       else if (304 <= winDirDegInt && winDirDegInt <= 326) windDir = "NW";
                       else if (327 <= winDirDegInt && winDirDegInt <= 348) windDir = "NNW";
                    
                    string beaufort = "";
                    
                    if (windSpeedKTS < 1) beaufort = "0 (Calm)";
                    else if (1 <= windSpeedKTS && windSpeedKTS <= 3) beaufort = "1 (Light air)";
                    else if (4 <= windSpeedKTS && windSpeedKTS <= 6) beaufort = "2 (Light breeze)";
                    else if (7 <= windSpeedKTS && windSpeedKTS <= 10) beaufort = "3 (Gentle breeze)";
                    else if (11 <= windSpeedKTS && windSpeedKTS <= 16) beaufort = "4 (Moderate breeze)";
                    else if (17 <= windSpeedKTS && windSpeedKTS <= 21) beaufort = "5 (Fresh breeze)";
                    else if (22 <= windSpeedKTS && windSpeedKTS <= 27) beaufort = "6 (Strong breeze)";
                    else if (28 <= windSpeedKTS && windSpeedKTS <= 33) beaufort = "7 (High wind, moderate gale, near gale)";
                    else if (34 <= windSpeedKTS && windSpeedKTS <= 40) beaufort = "8 (Gale, fresh gale)";
                    else if (41 <= windSpeedKTS && windSpeedKTS <= 47) beaufort = "9 (Strong/severe gale)";
                    else if (48 <= windSpeedKTS && windSpeedKTS <= 55) beaufort = "10 (Storm, whole gale)";
                    else if (56 <= windSpeedKTS && windSpeedKTS <= 63) beaufort = "11 (Violent storm)";
                    else if (64 <= windSpeedKTS) beaufort = "12 (Hurricane force)";

                    string dateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                    // Send the bracelettes in an embed
                    eb.WithTitle("floppy's Weather Data");
                    eb.WithColor(DiscordColor.Gold);
                    eb.WithFooter($"Data Retrieved: {time} · Use argument \"longterm\" for Long-Term data!");
                    // F i e l d s
                    eb.AddField("Outside Temp", $"{outsideTempC}°C ({outsideTempF}°F)", true);
                    eb.AddField("Outside Humidity", $"{outsideHum}%", true);
                    eb.AddField("Dew Point", $"{dewPointC}°C ({dewPointF}°F)", true);
                    eb.AddField("Precipitation", $"{rainRateMM} mm/hr ({rainRateIn} in/hr)", true);
                    if (Convert.ToInt32(windSpeedKTS) == 0) eb.AddField("Wind Direction", "No wind", true);
                    else eb.AddField("Wind Direction/Speed", $"{windSpeedMPH} mph ({windDirDeg}°, {windDir}, {Convert.ToString(windSpeedKTS)} knots)", true);
                    eb.AddField("Beaufort Scale", beaufort, true);

                }
            } else if (command == "info") {
                eb.WithTitle("Station Info");
                eb.WithColor(DiscordColor.Gold);
                eb.AddField("Model", "Davis Vantage / WeatherLink Live", true);
                eb.AddField("API Version", "v1", true);
                eb.AddField("Data Format", "JSON", true);
                eb.AddField("Sensors", "Wind Speed, Wind Direction, Humidity, Inside Temp, Outside Temp, Precipitation", false);
            } else if (command == "longterm") {
                string json = "";
                string time = "";

                // Try to grab the json string from the API
                try {
                    using (WebClient client = new WebClient()) {
                        json = client.DownloadString("http://api.weather.diskfloppy.me/v1/current_conditions");
                    }
                    time = "Now! (Live data)";
                } catch {
                    try {
                        using (WebClient client = new WebClient()) {
                            json = client.DownloadString("http://weather.diskfloppy.me/weatherData.json");
                            time = client.DownloadString("http://weather.diskfloppy.me/got.txt");
                        }
                    } catch {
                        eb.WithTitle("Unable to retrieve weather data!");
                        eb.WithColor(new DiscordColor("#ED4245"));
                        return;
                    }
                }
                if (json != "") {
                    dynamic output = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object
                    string dailyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_daily) * 0.2, 1));
                    string monthlyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_monthly) * 0.2, 1));
                    string yearlyRain = Convert.ToString(Math.Round(Convert.ToDouble(output.data.conditions[0].rainfall_year) * 0.2, 1));
                
                    eb.WithTitle("Long-Term Data");
                    eb.WithColor(DiscordColor.Gold);
                    eb.AddField("Past Day of Rainfall", $"{dailyRain} mm", false);
                    eb.AddField("Past Month of Rainfall", $"{monthlyRain} mm", false);
                    eb.AddField("Past Year of Rainfall", $"{yearlyRain} mm", false);
                    eb.WithFooter($"Data Retrieved: {time}");
                }
            }
            await Context.ReplyAsync("", eb.Build());
        }
    }
}
