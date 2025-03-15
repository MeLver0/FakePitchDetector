using System.Reflection;
using System.Text.Json;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using CSSharpUtils.Extensions;
using CSSharpUtils.Utils;

namespace FakePitchDetector
{
    public class FakePitchDetectorConfig : BasePluginConfig
    {
        [JsonPropertyName("ChatPrefix")] public string ChatPrefix { get; set; } = "[{Red}FakePitchFix{Default}]";

        [JsonPropertyName("WarningInterval")] public float WarningInterval { get; set; } = 90.0f;
        [JsonPropertyName("ConfigVersion")] public override int Version { get; set; } = 1;
    }

    public class FakePitchDetector : BasePlugin, IPluginConfig<FakePitchDetectorConfig>
    {
        public override string ModuleName => "Fake Pitch Exploit Fix";
        public override string ModuleVersion => "1.0.4";
        public override string ModuleAuthor => "MeL";

        public FakePitchDetectorConfig Config { get; set; } = new();

        public required MemoryFunctionVoid<CCSPlayer_MovementServices, IntPtr> RunCommand;
        private readonly Dictionary<uint, float> _pitchBlockWarnings = new();

        public override void Load(bool hotReload)
        {
            base.Load(hotReload);

            Console.WriteLine("[FakePitchFix] Hooking run command");

            RunCommand = new(GameData.GetSignature("RunCommand"));
            RunCommand.Hook(OnRunCommand, HookMode.Pre);
        }

        public void OnConfigParsed(FakePitchDetectorConfig config)
        {
            Config = config;
            config.Update();
        }

        private HookResult OnRunCommand(DynamicHook h)
        {
            var player = h.GetParam<CCSPlayer_MovementServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();
            if (!player.IsPlayer())
                return HookResult.Continue;

            var userCmd = new CUserCmd(h.GetParam<IntPtr>(1));
            var viewAngles = userCmd.GetViewAngles();

            if (viewAngles is null || viewAngles.IsValid())
                return HookResult.Continue;

            // Проверяем pitch угол
            if (Math.Abs(viewAngles.X) > 89.0f)
            {
                viewAngles.X = Math.Clamp(viewAngles.X, -89.0f, 89.0f);
                viewAngles.Fix();

                if (_pitchBlockWarnings.TryGetValue(player!.Index, out var lastWarningTime) &&
                    !(lastWarningTime + Config.WarningInterval <= Server.CurrentTime))
                    return HookResult.Changed;


                Server.PrintToChatAll($"{ChatUtils.FormatMessage(Config.ChatPrefix)} Уебан {ChatColors.Red}{player.PlayerName}{ChatColors.Default} пытается использовать fake pitch!");
                _pitchBlockWarnings[player.Index] = Server.CurrentTime;

                return HookResult.Changed;
            }

            return HookResult.Continue;
        }

        public override void Unload(bool hotReload)
        {
            base.Unload(hotReload);
            RunCommand.Unhook(OnRunCommand, HookMode.Pre);
        }
    }
}