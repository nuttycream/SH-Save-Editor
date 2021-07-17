# <img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/SpaceHaven%20Save%20Editor/Resources/icon.ico" width="25" height="25"/> Space Haven Save Editor

Edit your Space Haven game save! Based on [Steam Guide](https://steamcommunity.com/sharedfiles/filedetails/?id=2120100221)

## Features
- Edit each character's stats including health, attributes, skills, traits
- Add money to player's bank
- Add items to storages
- Add tools to tool storages
- Edit research values (coming soon)

## Usage
- **Note: Install [.NET 5.0 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0/runtime) to use framework dependent version.**
- Click on Releases, download self contained zip (much bigger) __OR__ framework dependent zip. 
- Unzip anywhere then click and run program
- Click open file, it should open a file explorer window
- Navigate to stored game save location:
`..steamapps\common\SpaceHaven\savegames\(SAVE NAME)\save`
- Select file named "game"
- Click create back-up and start editing!
- Save file once done.

## For Linux and Mac (i think)
- **Note: I tested this on Fedora 33, I don't have a mac but it should be about the same?**
- You'll need [Wine](https://wiki.winehq.org/Download) to run the program
- Download self contained zip and extract to a folder
- Open a terminal window inside the folder and enter `wine SpaceHaven Save Editor.exe`

## Screenshots
<img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/Screenies/File%20Menu.png"/>
<img src="https://github.com/nuttycream/SH-Save-Editor/blob/master/Screenies/Character%20Menu.png"/>

## Build
- Clone or download git repo
- Open `Space Haven Save Editor.sln` in Visual Studio. (Requires .NET 5.0)
- Run and build project

## Acknowledgements
- Thanks to $P00KY DA $CARY for his original [Steam Guide](https://steamcommunity.com/sharedfiles/filedetails/?id=2120100221)
- Developed in C# and XAML on the .NET Core WPF UI Framework
- Used [MaterialDesignInXAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) for buttons, icons, ui etc.

## License
This project is open source and available under the [MIT License](LICENSE). 
