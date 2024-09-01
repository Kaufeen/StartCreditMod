using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;
using System.Data.SqlTypes;

namespace StartCreditMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class TutorialModBase : BaseUnityPlugin
    {
        private const string modGUID = "Kaufeen.StartCreditMod";
        private const string modName = "Start Credit Mod";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static TutorialModBase Instance;

        internal ManualLogSource mls;

        public static ConfigFile config;
        public static ConfigEntry<int> setMoney { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Start Credit Mod is loading");

            config = ((BaseUnityPlugin)this).Config;
            setMoney = config.Bind<int>(
                "Settings",
                "Money",
                60,
                "How much money do you even need?"
                );


            harmony.PatchAll(typeof(TutorialModBase));
            harmony.PatchAll(typeof(TimeOfDay_Awake_Patch));

            mls.LogInfo("Start Credit Mod is loaded");
        }

        // Harmony patch class
        [HarmonyPatch(typeof(TimeOfDay))]
        [HarmonyPatch("Awake")]
        internal class TimeOfDay_Awake_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(TimeOfDay __instance)
            {
                __instance.quotaVariables.startingCredits = setMoney.Value;
            }
        }
    }

}
