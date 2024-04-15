using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using BetterSpectate.Patches;

namespace BetterSpectate
{
#if VanillaCompatible
    [ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, true)]
#else
    [ContentWarningPlugin(GUID, VER, false)]
#endif
    [BepInPlugin(GUID, NAME, VER)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "funfoxrr.BetterSpectate";
        public const string NAME = "BetterSpectate";
        public const string VER = "1.0.0";

        private readonly Harmony harmony = new Harmony(GUID);

        public static Plugin Instance;

        public static ManualLogSource Log;

        public static string assemblyLocation;
        public static AssetBundle assets;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Log = BepInEx.Logging.Logger.CreateLogSource(GUID);

            assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            assets = AssetBundle.LoadFromFile(Path.Combine(assemblyLocation, "funfoxrr_betterspectate"));
            if (assets == null)
            {
                Log.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }
            Log.LogInfo("Hey wsg bbgirl");

            Patch();
        }
        void Patch()
        {
            harmony.PatchAll(typeof(SpectatePatch));
        }
    }
}
