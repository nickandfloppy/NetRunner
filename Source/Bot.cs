using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.IO.Compression;						
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

using Newtonsoft.Json;

using Serilog;
using Microsoft.Extensions.Logging;

using HBot.Util;
using HBot.Misc;
using HBot.Commands;
using static HBot.Util.ResourceManager;

using ImageMagick;

namespace HBot {
    class Bot {
        public const string VERSION = "1.5.0";

        public static void Main(string[] args) => new Bot().RunBot().GetAwaiter().GetResult();

        // DSharpPlus
        public static DiscordClient client;
        public static CommandsNextExtension commands;
        public static Stopwatch sw = Stopwatch.StartNew();

        // Bot
        public static BotConfig config;

        public async Task RunBot() {
            // Logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.DiscordSink()
                .CreateLogger();
            ILoggerFactory logFactory = new LoggerFactory().AddSerilog();
            Log.Information($"HBot {VERSION}");
            Log.Information($"Starting bot...");

            VerifyIntegrity();
            LoadConfigs();
            
            // Set up the Discord client
            client = new DiscordClient(new DiscordConfiguration() {
                Token = config.token,
                TokenType = TokenType.Bot,
                LoggerFactory = logFactory,
                Intents = DiscordIntents.All
            });
            commands = client.UseCommandsNext(new CommandsNextConfiguration() {
                StringPrefixes = new string[] { config.prefix },
                EnableDefaultHelp = false,
                EnableDms = true,
                UseDefaultCommandHandler = false
            });
            client.UseInteractivity(new InteractivityConfiguration() {
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(60)
            });
            HookEvents();
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            await client.ConnectAsync();

            await Task.Delay(-1);
        }

        async Task Ready(DiscordClient client, ReadyEventArgs e) {
            // Set guilds
            Global.targetGuild = await client.GetGuildAsync(config.ids.targetGuild);

            // Set channels
            if(config.ids.logChannel != 0)
                Global.logChannel = await client.GetChannelAsync(config.ids.logChannel);
            if(Global.logChannel == null)
                Log.Error("Shitcord is failing to return a valid log channel, or no channel ID is set in the config");

            if(config.ids.welcomeChannel != 0)
                Global.welcomeChannel = await client.GetChannelAsync(config.ids.welcomeChannel);
            if(Global.welcomeChannel == null)
                Log.Error("Shitcord is failing to return a valid welcome channel, or no channel ID is set in the config");

            if(config.ids.reportsChannel != 0)
                Global.reportsChannel = await client.GetChannelAsync(config.ids.reportsChannel);
            if(Global.reportsChannel == null)
                Log.Error("Shitcord is failing to return a valid reports channel, or no channel ID is set in the config");

            // Set misc stuff

            // Start misc systems
            UserData.Init();
            Leveling.Init();
            TempManager.Init();
            DailyReportSystem.Init();
            MagickNET.Initialize(); 

            if(Bot.config.ids.rssChannel != 0)
                await RSS.Init();

            await client.UpdateStatusAsync(new DiscordActivity() { Name = config.status });
            Log.Information("Ready");
        }

        void HookEvents() {
            // Bot
            client.Ready += Ready;
            client.MessageCreated += CommandHandler.HandleMessage;
            client.MessageUpdated += (DiscordClient client, MessageUpdateEventArgs e) => {
                if(DateTime.Now.Subtract(e.Message.Timestamp.DateTime).TotalMinutes < 1 && DateTime.Now.Subtract(e.Message.Timestamp.DateTime).TotalSeconds > 2)
                    CommandHandler.HandleCommand(e.Message, e.Author);
                return Task.CompletedTask;
        };

            client.GuildMemberAdded += async (DiscordClient client, GuildMemberAddEventArgs e) => {
                if(!Global.mutedUsers.Contains(e.Member.Id))
                    await Global.welcomeChannel.SendMessageAsync($"Welcome, {e.Member.Mention} to the shithole! Make sure you read the rules before chatting. You can find them here: https://hiden.pw/discord/rules.");
                else {
                    await Global.welcomeChannel.SendMessageAsync($"Welcome, {e.Member.Mention} to the shithole! Unfortunately it seems as if you have failed to read the rules, have fun in the box! This is what you get for trying to be a ding-dong :P.");
                    await e.Member.GrantRoleAsync(Global.mutedRole, "succ");
                }
            };

            // Commands
            commands.CommandErrored += CommandHandler.HandleError;

            EventLogging.Init();
        }

        void VerifyIntegrity() {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Verifying integrity of bot files...");

            // Verify directories
            if(!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
            if(!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            if(!Directory.Exists("Resources"))
                Directory.CreateDirectory("Resources");
            // Extrememly awful way to do this, but I guess it'll work for now
            if(!Directory.Exists("Resources/Lyrics"))
                Directory.CreateDirectory("Resources/Lyrics");
            if(!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");

            // Verify configs & similar files
            if(!ResourceExists("config", ResourceType.Config)) {

                // Create a blank config
                config = new BotConfig();
                config.token = "TOKEN";
                config.status = " ";
                config.prefix = ".";
                config.ids = new IDConfig();
                config.apiKeys = new APIConfig();
                config.minecraftServers = new MCServer[]{new MCServer()};

                // Write the config and quit
                File.WriteAllText(GetResourcePath("config", ResourceType.Config), JsonConvert.SerializeObject(config, Formatting.Indented));
                Log.Fatal("No configuration file found. A template config has been written to config.json");
                Environment.Exit(-1);
        }
            if(!ResourceExists("blacklist", ResourceType.JsonData))
                File.WriteAllText(GetResourcePath("blacklist", ResourceType.JsonData), "[]");
            if(!ResourceExists("mute", ResourceType.JsonData))
                File.WriteAllText(GetResourcePath("mute", ResourceType.JsonData), "[]");

            // Verify and download resources
            Log.Information("Verifying resources...");
            WebClient webClient = new WebClient();
            string resourcesJson = webClient.DownloadString("https://raw.githubusercontent.com/HIDEN64/HBot/master/Resources/resources.json");
            string[] resources = JsonConvert.DeserializeObject<string[]>(resourcesJson);
            foreach(string resource in resources) {
                if(!ResourceExists(resource, ResourceType.Resource)) {
                    webClient.DownloadFile($"https://raw.githubusercontent.com/HIDEN64/HBot/master/Resources/{resource}", GetResourcePath(resource, ResourceType.Resource));
                    Log.Information("Downloaded " + resource + "");
                }
            }
// This is awful awful awful awful awful AWFUL to do this on every startup
            // but I'm lazy and it's the only way I can think of right now to make the bot
            // update lyrics on startup lol
            foreach(string file in Directory.GetFiles("Resources/Lyrics"))
                File.Delete(file);
            ZipFile.ExtractToDirectory("Resources/Lyrics.zip", "Resources/");
        }

        void LoadConfigs() {
            // Main bot config
            config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(GetResourcePath("config", ResourceType.Config)));
            if(config == null) {
                Log.Fatal("Failed to load configuration!");
                Environment.Exit(-1);
            }
            Global.blacklistedUsers = JsonConvert.DeserializeObject<List<ulong>>(File.ReadAllText(GetResourcePath("blacklist", ResourceType.JsonData)));
            Global.mutedUsers = JsonConvert.DeserializeObject<List<ulong>>(File.ReadAllText(GetResourcePath("mute", ResourceType.JsonData)));

        }
    }

    class BotConfig {
        public string token { get; set; }
        public string prefix { get; set; }
        public string status { get; set; }
        public IDConfig ids { get; set; }
        public APIConfig apiKeys { get; set; }
        public MCServer[] minecraftServers { get; set; }
    }
    
    class IDConfig {
        public ulong targetGuild { get; set; } = 0; // Where muted role etc are
        public ulong logChannel { get; set; } = 0;
        public ulong mutedRole { get; set; } = 0;
        public ulong welcomeChannel { get; set; } = 0;
        public ulong reportsChannel { get; set; } = 0;
        public ulong rssChannel { get; set; } = 0;

    }

    class APIConfig {
        public string wikihowAPIKey { get; set; } = "";
        public string catAPIKey { get; set; } = "";
        public string weatherAPI { get; set; } = "";
    }

    class Global {
        public static List<List<string>> reminders = new List<List<string>>();
        public static DiscordGuild targetGuild;
        public static DiscordChannel logChannel = null;
        
        // Moderation
        public static List<ulong> blacklistedUsers = new List<ulong>();
        public static List<ulong> mutedUsers = new List<ulong>();
        public static DiscordRole mutedRole;
        public static DiscordChannel welcomeChannel = null;
        public static DiscordChannel reportsChannel = null;

    }
}
