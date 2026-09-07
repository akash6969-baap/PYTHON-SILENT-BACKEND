using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AotForms
{
    public static class PipeServer
    {

        private static readonly ESP esp = new ESP();
        public static void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        using var server = new NamedPipeServerStream("esp_pipe", PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        server.WaitForConnection();

                        using var reader = new StreamReader(server, Encoding.UTF8);
                        string? command = reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(command)) continue;

                        // ─── PHASE 1: adbhook command ──────────────────────────────────────────
                        // Python sends "adbhook:0xA9870BC" after DLL inject + version select.
                        // This triggers ADB setup and starts all worker threads.
                        // Until this command arrives, ALL other commands are ignored.
                        if (command.StartsWith("adbhook:"))
                        {
                            if (Config.AdbHookDone)
                            {
                                Console.WriteLine("[PipeServer] adbhook already done — ignoring duplicate.");
                                continue;
                            }

                            string rawBase = command.Substring("adbhook:".Length).Trim();
                            uint initBase;
                            bool parseOk = rawBase.StartsWith("0x") || rawBase.StartsWith("0X")
                                ? uint.TryParse(rawBase.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out initBase)
                                : uint.TryParse(rawBase, out initBase);

                            if (!parseOk || initBase == 0)
                            {
                                Console.WriteLine($"[PipeServer] adbhook — invalid InitBase: {rawBase}");
                                continue;
                            }

                            Offsets.InitBase = initBase;
                            Console.WriteLine($"[PipeServer] InitBase set to 0x{initBase:X} — starting ADB hook...");

                            // Run ADB setup + thread launch asynchronously
                            Task.Run(async () =>
                            {
                                try
                                {
                                    var adb = new Adb(Config.EmulatorAdbPath);
                                    await adb.Kill();
                                    if (!await adb.Start())
                                    {
                                        Console.WriteLine("[PipeServer] ADB failed to start.");
                                        return;
                                    }

                                    var moduleAddr = await adb.FindModule("com.dts.freefireth", "libil2cpp.so");
                                    if (moduleAddr == 0)
                                    {
                                        Console.WriteLine("[PipeServer] libil2cpp.so not found — go to lobby and try again.");
                                        return;
                                    }

                                    Offsets.Il2Cpp = moduleAddr;
                                    Console.WriteLine($"[PipeServer] Il2Cpp = 0x{moduleAddr:X} — launching all threads...");

                                    var esp = new ESP();
                                    await esp.Start();

                                    new Thread(Data.Work)          { IsBackground = true }.Start();
                                    new Thread(AimbotAi.Work)      { IsBackground = true }.Start();
                                    new Thread(AimbotV2.Work)      { IsBackground = true }.Start();
                                    new Thread(AimbotDrag.Work)    { IsBackground = true }.Start();
                                    new Thread(AimbotRageMain.Work){ IsBackground = true }.Start();
                                    new Thread(EnemyPull360.Start) { IsBackground = true }.Start();
                                    new Thread(FlyHack80x.Work)    { IsBackground = true }.Start();
                                    new Thread(FLYHACKX40.Work)    { IsBackground = true }.Start();
                                    new Thread(FlyRage.Work)       { IsBackground = true }.Start();
                                    new Thread(SilentAim.Work)     { IsBackground = true }.Start();
                                    new Thread(() => RapidSpin.Activate()) { IsBackground = true }.Start();

                                    Config.AdbHookDone = true;
                                    Console.WriteLine("[PipeServer] ✅ All threads started — ready!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"[PipeServer] adbhook error: {ex.Message}");
                                }
                            });
                            continue;
                        }

                        // ─── PHASE GATE ────────────────────────────────────────────────────────
                        // Ignore ALL other commands until ADB hook is complete
                        if (!Config.AdbHookDone)
                        {
                            Console.WriteLine($"[PipeServer] Ignoring '{command}' — adbhook not done yet.");
                            continue;
                        }
                        // ──────────────────────────────────────────────────────────────────────

                        switch (command.Trim())
                        {
                            case "autoswitch":
                                Config.AutoSwitch = true;
                                break;
                            case "autoswitchoff":
                                Config.AutoSwitch = false;
                                break;

                            case "downplayeron":
                                Config.DownPlayer = true;
                                DownPlayer.Work();
                                break;
                            case "downplayeroff":
                                Config.DownPlayer = false;
                                DownPlayer.Work();
                                break;

                            case "pullenemyon":
                                Config.EnemyPullEnabled = true;
                                break;
                            case "pullenemyoff":
                                Config.EnemyPullEnabled = false;
                                break;


                            case "ignoreknockedon":
                                Config.IgnoreKnocked = true;
                                break;
                            case "ignoreknockedoff":
                                Config.IgnoreKnocked = false;
                                break;

                            case "flyhackx80on":
                                Config.FlyHack80x = true;
                                break;
                            case "flyhackx80off":
                                Config.FlyHack80x = false;
                                break;

                            case "flyrageon":
                                Config.FlyRage = true;
                                break;
                            case "flyrageoff":
                                Config.FlyRage = false;
                                break;

                            case "speedon":
                                Config.SpeedHack = true;
                                esp.speedon();
                                break;

                            case "speedoff":
                                Config.SpeedHack = false;
                                esp.speedoff();
                                break;

                            case "speedload":
                                esp.speedon();
                                break;



                            case "underkillon":
                                Config.ClimbDownEnabled = true;
                                Config.undergroundkill = true;
                                DownPlayer.Work();
                                break;
                            case "underkilloff":
                                Config.ClimbDownEnabled = false;
                                Config.undergroundkill = false;
                                DownPlayer.Work();
                                break;

                            case "baseon":
                                Config.sperm = true;
                                break;
                            case "baseoff":
                                Config.sperm = false;
                                break;

                            case "fastreloadon":
                                Config.FastReload = true;
                                break;
                            case "fastreloadoff":
                                Config.FastReload = false;
                                break;

                            case "aimbotvisible":
                                Config.AimbotVisible = true;
                                break;
                            case "aimbotvisibleoff":
                                Config.AimbotVisible = false;
                                break;
                            case "aimbotdragon":
                                Config.AimbotDrag = true;
                                break;
                            case "aimbotdragoff":
                                Config.AimbotDrag = false;
                                break;
                            case "aimbotai":
                                Config.Aimbotai = true;
                                break;
                            case "aimbotaioff":
                                Config.Aimbotai = false;
                                break;
                            case "enablefunction":
                                Config.enableAimBot = true;
                                break;
                            case "enablefunctionoff":
                                Config.enableAimBot = false;
                                break;
                            case "aimbotrage":
                                Config.AimBotRageMax = true;
                                break;
                            case "aimbotrageoff":
                                Config.AimBotRageMax = false;
                                break;

                            case "aimbotragemainson":
                                Config.AimBotRageMainId = true;
                                break;
                            case "aimbotragemainoff":
                                Config.AimBotRageMainId = false;
                                break;
                            case "silentaim":
                                Config.SilentAim = true;
                                Config.SilentAimbody = false;
                                break;
                            case "silentaimoff":
                                Config.SilentAim = false;
                                break;
                            case "silentaimbody":
                                Config.SilentAimbody = true;
                                Config.SilentAim = false;
                                break;
                            case "silentaimbodyoff":
                                Config.SilentAimbody = false;
                                break;
                            case "undergroundon":
                                Config.UndergroundEnabled = true;
                                DownPlayer.Work();
                                break;
                            case "undergroundoff":
                                Config.UndergroundEnabled = false;
                                DownPlayer.Work();
                                break;
                            case "drawfov":
                                Config.FOVEnabled = true;
                                break;
                            case "drawfovoff":
                                Config.FOVEnabled = false;
                                break;
                            case "espline":
                                Config.ESPLine = true;
                                break;
                            case "esplineoff":
                                Config.ESPLine = false;
                                break;
                            case "espdistanceon":
                                Config.ESPDistance = true;
                                break;
                            case "espdistanceoff":
                                Config.ESPDistance = false;
                                break;
                            case "espbox":
                                Config.ESPBox = true;
                                break;
                            case "espboxoff":
                                Config.ESPBox = false;
                                break;
                            case "esphealth":
                                Config.ESPHealth = true;
                                break;
                            case "esphealthoff":
                                Config.ESPHealth = false;
                                break;
                            case "espname":
                                Config.ESPName = true;
                                break;
                            case "espnameoff":
                                Config.ESPName = false;
                                break;
                            case "espskeleton":
                                Config.ESPSkeleton = true;
                                break;
                            case "espskeletonoff":
                                Config.ESPSkeleton = false;
                                break;
                            case "espaimtrack":
                                Config.AimTrackLine = true;
                                break;
                            case "espaimtrackoff":
                                Config.AimTrackLine = false;
                                break;
                            case "mapon":
                                Config.minimap = true;
                                break;
                            case "mapoff":
                                Config.minimap = false;
                                break;
                            case "streammode":
                                Config.StreamMode = true;
                                break;
                            case "streammodeoff":
                                Config.StreamMode = false;
                                break;
                            case "norecoil":
                                Config.NoRecoil = true;
                                break;
                            case "norecoiloff":
                                Config.NoRecoil = false;
                                break;

                            case "flyhackx40on":
                                Config.FLYHACKX400 = true;
                                break;
                            case "flyhackx40off":
                                Config.FLYHACKX400 = false;
                                break;

                            case "unlimitedammoon":
                                Config.UnlimitedAmmo = true;
                                break;
                            case "unlimitedammooff":
                                Config.UnlimitedAmmo = false;
                                break;

                            case "rapidfireon":
                                Config.RapidFire = true;
                                break;
                            case "rapidfireoff":
                                Config.RapidFire = false;
                                break;

                            case "speedinton":
                                Config.speedint = true;
                                break;
                            case "speedintoff":
                                Config.speedint = false;
                                break;

                            case "espweaponon":
                                Config.espweapon = true;
                                break;
                            case "espweaponoff":
                                Config.espweapon = false;
                                break;

                            case "spinboton":
                                Config.spinbot = true;
                                RapidSpin.Activate();
                                break;
                            case "spinbotoff":
                                Config.spinbot = false;
                                RapidSpin.Deactivate();
                                break;

                            default:
                                if (command.StartsWith("silentaim_mode:"))
                                {
                                    string indexStr = command.Split(':')[1];
                                    if (int.TryParse(indexStr, out int modeIndex))
                                    {
                                        Config.AimBotType = (AimBotType)modeIndex;
                                        Console.WriteLine($"SilentAim mode set to index: {modeIndex}");
                                    }
                                }
                                else if (command.StartsWith("aimfov:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, out float fov))
                                    {
                                        Config.AimFov = fov;
                                        Console.WriteLine($"Aim FOV set to: {fov}");
                                    }
                                }
                                else if (command.StartsWith("rapidfirespeed:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, out float speed))
                                    {
                                        Config.RapidFireSpeed = speed;
                                        Console.WriteLine($"RapidFire Speed set to: {speed}");
                                    }
                                }
                                else if (command.StartsWith("spinspeed:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float spd))
                                    {
                                        Config.SpinSpeed = spd;
                                        Console.WriteLine($"SpinBot Speed set to: {spd}");
                                    }
                                }
                                else if (command.StartsWith("aimtargetpart:"))
                                {
                                    string part = command.Split(':')[1].ToUpper();
                                    Config.AimTargetPart = part;
                                    Console.WriteLine($"AimTargetPart set to: {part}");
                                }
                                else if (command.StartsWith("aimsmooth:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float smooth))
                                    {
                                        Config.AimBotSmooth = smooth;
                                        Console.WriteLine($"AimBotSmooth set to: {smooth}");
                                    }
                                }
                                else if (command.StartsWith("aimdistance:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (int.TryParse(value, out int dist))
                                    {
                                        Config.AimBotMaxDistance = dist;
                                        Console.WriteLine($"AimBotMaxDistance set to: {dist}");
                                    }
                                }
                                else if (command.StartsWith("dragthreshold:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float thr))
                                    {
                                        Config.DragThreshold = thr;
                                        Console.WriteLine($"DragThreshold set to: {thr}");
                                    }
                                }
                                else if (command.StartsWith("smoothness:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (int.TryParse(value, out int ms))
                                    {
                                        Config.Smoothness = ms;
                                        Console.WriteLine($"Smoothness set to: {ms}ms");
                                    }
                                }
                                else if (command.StartsWith("pullstrength:"))
                                {
                                    string value = command.Split(':')[1];
                                    if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float strength))
                                    {
                                        Config.EnemyPullStrength = Math.Clamp(strength, 0.01f, 1.0f);
                                        Console.WriteLine($"EnemyPullStrength set to: {strength}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Unknown command: {command}");
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Pipe error: {ex.Message}");
                    }
                }
            });
        }
    }
}