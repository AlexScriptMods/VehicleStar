# VehicleStar [ALPHA]

This tool allows you to record, play, and export custom vehicle paths in YVR format.<br>
The mod is perfect for creating cinematic scenes and races.

## Features

- Record vehicle movement in real time while driving
- Save recordings as `.yvr`, allowing you to utilise Rockstar's advanced vehicle playback system
- Debug playback for quick tests of the recorded path
- Simple in-game UI

## Installation

1. Place all files from the mod in your **scripts** folder inside your GTAV root.
2. Download [CodeWalker](https://www.gta5-mods.com/tools/codewalker-gtav-interactive-3d-map), copy and paste `CodeWalker.Core.dll` and `SharpDX.Mathematics.dll` from the downloaded CodeWalker folder into the **scripts** folder.
3. Download LemonUI and place it into the **scripts** folder.
4. Open `config.xml` in `scripts` and set your export directory

## Usage

Press **Z** in-game to open the menu, then scroll and press Enter to navigate.

### 1. Recording
Use the **Start** and **Stop** buttons to record your vehicle’s path in real time while driving.

### 2. Playback
VehicleStar provides two playback systems:  

- **DEBUG Playback**  
  Allows you to quickly test the recorded path without importing or restarting the game.<br>
  **This system uses simplified playback and does not render all visual effects.**

- **YVR Playback**  
  Plays the recorded path using the exported `.yvr` file. <br>
  **This method requires importing the `.yvr` into the game files and **restarting** the game.**

#### How to import your exported `.yvr` using OpenIV:

1. Open OpenIV and navigate to your `mods/update/update.rpf/x64/data/cdimages/carrec.rpf`.  
2. Enable **Edit Mode**.  
3. Copy your exported `.yvr` file into this folder.  
4. Restart the game.

### 3. Exporting
Use the **Export** button to export the recording as `.xml` and `.yvr` files. The files will be saved in the directory specified in `config.xml`, located inside the **scripts** folder.

## Credits

- **CodeWalker** – Used for exporting to a vehicle path file.
  Official page: [CodeWalker GTA V Interactive 3D Map](https://www.gta5-mods.com/tools/codewalker-gtav-interactive-3d-map)  
