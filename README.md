# NetRunner
A fork of CamK06's WinBot.

## Usage/Build instructions
1. Clone the repository: ``https://github.com/nickandfloppy/NetRunner.git`` and move into the project directory: ``cd NetRunner``
2. Build the source code: ``dotnet build -c Release``
3. Change into the build directory: ``cd bin/Release/net6.0/``
4. Run the bot: ``./WinBot`` or just ``WinBot`` for Windows. This will generate a blank configuration file for you.
5. Edit the ``config.json``  file and update the options [described below](#configuration-options) as required
6. Run the bot once more, as before. Once it has started up (It'll output "Ready" to the terminal), you should be good to go into Discord and use it.

## Configuration Options
- ``token``: Your bot token (get one from the [developer portal](https://discord.com/developers/applications))
- ``prefix``: The bot command prefix
- ``status``: The game the bot should be "Playing"
- ``ids.hostGuild``: The ID of the guild where the log channel resides
- ``ids.targetGuild``: The guild where the bot will be used
- ``ids.logChannel``: The ID of the channel where bot logs will be sent (in the host guild)
- ``ids.mutedRole``: The ID of the role given to users when they are muted
- ``ids.rssChannel``: The ID of the channel where RSS messages from WWRSS are sent (0 to disable)
- ``apiKeys.wikihowAPIKey``: Your WikiHow API key (get one from [RapidAPI](https://rapidapi.com/hargrimm/api/wikihow))
- ``apiKeys.catAPIKey``: Your TheCatAPI API key (get one [here](https://thecatapi.com/signup))
- ``apiKeys.weatherAPI``: Your WeatherAPI API key (get one [here](https://www.weatherapi.com/signup.aspx))
- ``minecraftServers``: An array of Minecraft servers. An example is provided in the default config and options are described below:

### Minecraft Server Options
- ``guildID``: The ID of the guild the server belongs to (one per guild)
- ``address``: The public address of the Minecraft server
- ``dynmap``: The Dynmap address of the Minecraft server, blank to disable
- ``versions``: A string listing whatever versions the server supports
- ``crackedInfo``: A string detailing whether the server supports cracked accounts or not

## Post-install instructions
For the level rank cards to work you'll need to install Roboto. Due to some annoyances with System.Drawing you have to install it twice in a way.
Steps:

1. Download the font family https://fonts.google.com/specimen/Roboto
2. Extract Roboto-Regular.ttf into the bot's working directory
3. Install Roboto-Regular.ttf on your system

Note: This bot has been modified for use on 64-bit Windows and may not work out of the box. Some modifications to [``WinBot.csproj``](WinBot.csproj) maybe required.

## Contributing

For contribution guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md)

## Licensing

This project is under the [MIT Licence](https://choosealicense.com/licenses/mit/)
