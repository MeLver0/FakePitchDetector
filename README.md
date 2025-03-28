# FakePitchDetector

## About
FakePitchDetector is a Counter-Strike 2 server plugin developed using CounterStrikeSharp. It detects and fixes Fake Pitch Exploit by setting the pitch value to 89 without affecting the player's gameplay experience. FakePitchDetector was developed independently from TeleportFix, AirStackFix, and GameCrashFix, allowing users to choose only the fixes they need without unnecessary overhead.

## Installation
### Requirements:
- **Metamod:Source 2.x**
- **CounterStrikeSharp v255**
- **.NET 8.0** (If your server runs a different .NET version, follow the "Configuration" section below.)

### Steps:
1. Download and extract the `addons` folder into your CS2 server directory, ensuring it merges with the existing `addons` folder.
2. Verify that your server is running the required dependencies (**Metamod:Source 2.x** and **CounterStrikeSharp v255**).
3. Start the server and check logs for any errors related to plugin loading.

## Configuration
If your CS2 server runs on a .NET version other than **.NET 8.0**, you must modify the project to match your environment:
1. Clone this repository to your local machine:
   ```sh
   git clone https://github.com/MeLver0/FakePitchDetector.git
   ```
2. Open the project in your preferred IDE (e.g., Visual Studio or JetBrains Rider).
3. Locate and open `FakePitchDetector.csproj`.
4. Modify the `<TargetFramework>` value to match your .NET version. Example:

   ```xml
   <TargetFramework>net7.0</TargetFramework>
   ```
6. Rebuild the project and deploy the updated files to your server.

## Functionality
- Detects players using Fake Pitch Exploit.
- Automatically adjusts their pitch value to **89**.
- Does not interfere with normal gameplay mechanics.

## Credits
This project was made possible thanks to the source code available on **hvh.gg**, originally written by **imi-tat0r**. 
Special thanks to **imi-tat0r** for their contributions to the CS2 plugin development community and for sharing valuable insights that helped shape this project.

## Support
For any issues, please open an issue on GitHub or reach out to the CounterStrikeSharp community for assistance.

