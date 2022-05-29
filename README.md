# NetRunner
A fork of Starman0620's WinBot, modified for use in nick and floppy's corner.

## Usage/Build instructions
1. Clone the repository: [https://github.com/nickandfloppy/NetRunner.git](https://github.com/nickandfloppy/NetRunner.git) and move into the project directory: ``cd NetRunner``
2. Build the source code: ``dotnet build -c Release -r linux-x64``
3. Change into the build directory: ``cd bin/Release/net5.0/linux-x64/``
4. Run the bot: ``./WinBot`` or just ``WinBot`` for Windows. This will generate a blank configuration file for you.
5. Edit the ``config.json``  file and add your token into the token field, aswell as your client ID and log channel if you want one
6. Run the bot once more, as before. Once it has started up (It'll output "Ready" to the terminal), you should be good to go into Discord and use it.

## Post-install instructions
For the level rank cards to work you'll need to install Roboto. Due to some annoyances with System.Drawing you have to install it twice in a way.
Steps:

1. Download the font family https://fonts.google.com/specimen/Roboto
2. Extract Roboto-Regular.ttf into the bot's working directory
3. Install Roboto-Regular.ttf on your system

Note: This bot has been modified for a specific system configuration and may not work out of the box. This repo is mainly here for me to keep track of edits.
