# HBot
A fork of nick99nack's/floppydisk's NetRunner, which in turn is a fork of CamK06's WinBot, modified for general use (and to become it's own thing). 

## Usage instructions

### Requirements:
- .NET 7.0
- Roboto, Impact, and xband fonts

#### Officially supported OSes:
- Windows 10 Anniversary Update or newer
- macOS 10.15 or newer
- Debian 10 or newer
- Ubuntu 18.04 or newer
- Fedora 36 or newer
- Any other Linux distribution that supports .NET 7.0

You will not recieve technical assistance if you are using an unsupported OS, or if you're using a workaround to run this on an unsupported OS.

### Building and running:
1. Clone the repository: ``https://github.com/HIDEN64/HBot.git`` and move into the project directory: ``cd HBot``
2. Build the source code: ``dotnet build -c Release``
3. Change into the build directory: ``cd bin/Release/net6.0/``
4. Run the bot: ``./HBot`` or just ``HBot`` for Windows. This will generate a blank configuration file for you.
5. Edit ``config.json``  to your liking.
6. Run the bot once more, as before. Once it has started up (It'll output "Ready" to the terminal), you should be good to go into Discord and use it.

### Post-install instructions:
For the level rank cards to work correctly, you'll need to install Roboto. Due to some annoyances with System.Drawing, you have to install it twice in a way.

1. Download the font family https://fonts.google.com/specimen/Roboto
2. Extract Roboto-Regular.ttf into the bot's working directory
3. Install Roboto-Regular.ttf on your system
