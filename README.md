# Task Force Admiral Rich Presence

[![Discord](https://img.shields.io/badge/Discord-Rich_Presence-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.com)
[![Game](https://img.shields.io/badge/Game-Task_Force_Admiral-blue?style=for-the-badge)](https://www.taskforceadmiral.com/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)

A modern WinForms application that monitors **Task Force Admiral** gameplay and displays real-time battle events via **Discord Rich Presence**.

## ⚓ Features

### 📊 Real-Time Battle Events
- **Live Log Monitoring** - Reads `Plankton.log` in real-time
- **Color-Coded Events** - Ships hit (red), aircraft destroyed (green), radio chatter (yellow)
- **Session Statistics** - Track total hits taken and enemy destroyed
- **Auto-Scrolling Feed** - Never miss an event

### 🎮 Discord Integration
- **Dynamic Status** - Shows current battle action
- **Ship Under Attack** - `🚨 USS Yorktown (CV-5) under attack!`
- **Radio Messages** - `📻 This is Sugar 0-1, engaging bandits!`
- **Session Stats** - `💥 12 hits taken | ✈️ 8 enemy destroyed`

### 🔄 Auto-Updates
- **GitHub Integration** - Checks for latest releases
- **Release Notes** - View changelog directly in app
- **One-Click Download** - Opens browser to latest release

### 💾 Persistent State
- **Session Recovery** - Load your last battle stats
- **rpc.json** - Saves all event data and statistics
- **No Data Loss** - Resume tracking after restart

## 🚀 Installation

### Requirements
- Windows 10/11
- .NET 6.0 or higher
- [Task Force Admiral](https://www.taskforceadmiral.com/) (with Plankton.log access)
- Discord running

### Download
1. Go to [Releases](https://github.com/Smiley-Devv/TaskForceAdmiralRichPresence/releases)
2. Download the latest `TaskForceAdmiralRPC.zip`
3. Extract to a folder
4. Run `TaskForceAdmiralRPC.exe`

### From Source
```bash
git clone https://github.com/Smiley-Devv/TaskForceAdmiralRichPresence.git
cd TaskForceAdmiralRichPresence
dotnet build
dotnet run
```

## 📖 How to Use

### First Launch
1. **Start Discord** - Make sure Discord is running
2. **Launch the App** - Double-click `TaskForceAdmiralRPC.exe`
3. **Load Previous Session** (optional) - Click "Yes" to restore stats

### During Gameplay
1. **Start Task Force Admiral**
2. **Begin a Scenario** - The app automatically detects `Plankton.log`
3. **Watch Events** - Battle events appear in real-time
4. **Check Discord** - Your profile shows live battle status!

### Log File Location
The app looks for `Plankton.log` in these locations:
- `<Game Install>/bin/Plankton.log`
- `Documents/My Games/TaskForceAdmiral/logs/Plankton.log`
- App directory

## 🎨 Event Types

| Icon | Event Type | Description | Discord Display |
|------|------------|-------------|-----------------|
| 💥 | Ship Hit (Bomb) | Your ship takes a bomb hit | `🚨 USS Yorktown under attack!` |
| 🌊 | Ship Hit (Torpedo) | Your ship takes a torpedo | `🚨 Ship hit by torpedo!` |
| ✈️ | Aircraft Destroyed | Enemy aircraft shot down | Session kill count increases |
| 📻 | Radio Chatter | Pilot communications | `Radio: Target in sight!` |
| 📊 | Battle Report | Action report generated | `Session: X hits, Y kills` |
| 📅 | Date Marker | In-game date/time | Timestamp reference |

## 📁 File Structure

```
TaskForceAdmiralRPC/
├── Form1.cs                    # Main application logic
├── Form1.Designer.cs           # UI layout
├── LogFileMonitor.cs           # Plankton.log parser
├── GitHubUpdateChecker.cs      # Update checker
├── ModernDialog.cs             # Startup dialog
├── SaveSnapshot.cs             # Data models
└── RpcState.cs                 # State persistence
```

## ⚙️ Configuration

### Discord Application ID
Default: `1285061587509579826`

To use your own:
1. Go to [Discord Developer Portal](https://discord.com/developers/applications)
2. Create a new application
3. Copy the Application ID
4. Update in `Form1.cs`:
```csharp
client = new DiscordRpcClient("YOUR_APPLICATION_ID");
```

### RPC State File
Located at: `%AppData%/TaskForceAdmiralLiveRPC/rpc.json`

Example contents:
```json
{
  "WhenUpdated": "2026-01-31T15:30:00",
  "LastEvent": "USS Yorktown (CV-5) hit by a bomb",
  "EventType": "ShipHitBomb",
  "TotalHits": 12,
  "TotalKills": 8,
  "LastAction": "USS Yorktown hit by bomb!",
  "ShipName": "USS Yorktown (CV-5)",
  "GameDate": "May 8 1942"
}
```

## 🐛 Troubleshooting

### "Log file not found"
- Make sure Task Force Admiral is running
- Check that `Plankton.log` exists in your game folder
- Look in `bin/` subfolder of game installation

### Discord RPC not working
1. Restart Discord
2. Click "Reconnect RPC" button
3. Check Application ID is correct
4. Ensure Discord is set to show game activity

### No events appearing
1. Start a scenario in-game
2. Make sure battle events are occurring
3. Check the status bar for errors
4. Verify log file is being written

### App won't start
- Install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- Run as Administrator
- Check Windows Defender isn't blocking it

## 🤝 Contributing

Contributions are welcome! Here's how:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Ideas for Contributions
- [ ] Additional event parsing (damage types, ship classes)
- [ ] Sound notifications for critical hits
- [ ] Statistics graphs/charts
- [ ] Export battle reports
- [ ] Multi-language support
- [ ] Custom Discord status templates
- [ ] Integration with OBS/streaming tools

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Credits

- **Task Force Admiral** by Drydock Dreams Foundation
- **DiscordRPC Library** by Lachee
- **Icon/Graphics** - Community contributions

## 📞 Support

- 🐛 [Report Issues](https://github.com/Smiley-Devv/TaskForceAdmiralRichPresence/issues)
- 💬 [Discussions](https://github.com/Smiley-Devv/TaskForceAdmiralRichPresence/discussions)

## 🔗 Links

- [Task Force Admiral Official Site](https://www.taskforceadmiral.com/)
- [Discord Rich Presence Documentation](https://discord.com/developers/docs/rich-presence/overview)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)

---

**Made with ❤️ for Task Force Admiral players**

⚓ Fair winds and following seas! ⚓
