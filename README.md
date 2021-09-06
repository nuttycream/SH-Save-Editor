# <img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/SpaceHaven%20Save%20Editor/Assets/icon.ico" width="25" height="25"/> Space Haven Save Editor

Edit your [SpaceHaven](https://bugbyte.fi/spacehaven/) game save! Based on [Steam Guide](https://steamcommunity.com/sharedfiles/filedetails/?id=2120100221)

### [![Github All Releases](https://img.shields.io/github/downloads/nuttyCream/SH-Save-Editor/total)]()
## Features
- Edit each character's stats including health, attributes, skills, traits
- Clone a character (expiremental)
- Add money to player's bank
- Add items in storages
- Edit research values

## Usage for Windows
- **Note: Requires [.NET 5.0 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0/runtime). Alternatively, you can download the portable version labeled win-x64-portable(~80mb)**
- Click on [Releases](https://github.com/nuttycream/SH-Save-Editor/releases), download win-x64.zip. 
- Unzip anywhere then click and run program
- Click File->Open File or Ctrl+O, it should open a file explorer window
- Navigate to stored game save location:
`..steamapps\common\SpaceHaven\savegames\(SAVE NAME)\save`
- Select file named "game"
- Save file once done.

## For Linux
- Click on [Releases](https://github.com/nuttycream/SH-Save-Editor/releases), download linux-x64.
- Extract to a folder, example shse/SpaceHavenSaveEditor <- this is the File you want to make executable
- Open a terminal window and enter `chmod +x /SpaceHaven Save Editor`, you should now be able to run the application with `./SpaceHaven Save Editor`

## For Mac (proper .app coming soon)
- Click on [Releases](https://github.com/nuttycream/SH-Save-Editor/releases), download win-x64-portable.
- You'll need [Wine](https://wiki.winehq.org/Download) to run the program

## Screenshots
<img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/Screenies/Game%20Menu.png"/>
<img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/Screenies/Character%20Menu.png"/>
<img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/Screenies/Storage%20Menu.png"/>

## Build
- Clone or download git repo
- Open `Space Haven Save Editor.sln` in Visual Studio or Rider
- Run and build project

## Acknowledgements
- Thanks to $P00KY DA $CARY for his original [Steam Guide](https://steamcommunity.com/sharedfiles/filedetails/?id=2120100221)
- Developed in C# and AXAML on the Avalonia .NET Framework
- You can find an alternative version as a webapp on https://spacehaven-editor.com/

## License
This project is open source and available under the [MIT License](LICENSE). 
