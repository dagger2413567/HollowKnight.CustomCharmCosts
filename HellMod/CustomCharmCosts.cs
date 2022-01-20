using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Modding;
using UnityEngine;

namespace CustomCharmCosts
{
    [UsedImplicitly]
    public class CustomCharmCosts : Mod, ITogglableMod, IGlobalSettings<GlobalModSettings>
    {
        public CustomCharmCosts() : base("Custom Charm Costs") { }

        private GlobalModSettings _settings = new();

        public void OnLoadGlobal(GlobalModSettings s) => _settings = s;

        public GlobalModSettings OnSaveGlobal() => _settings;

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public override void Initialize()
        {
            Log("Initializing");

            ModHooks.GetPlayerIntHook += OnInt;
            ModHooks.NewGameHook += OnNewGame;
            ModHooks.SavegameLoadHook += OnSaveLoaded;
        }

        private int OnInt(string intName, int orig) => intName switch
        {
            // Gathering, Wayfayers, Quick Slash, SprintMaster
            "charmCost_01" => 0,
            "charmCost_02" => 0,
            "charmCost_20" => 1,
            "charmCost_21" => 3,
            "charmCost_32" => 1,
            "charmCost_37" => 0,

            _ => orig
        };

        public void Unload()
        {
            ModHooks.GetPlayerIntHook -= OnInt;
            ModHooks.NewGameHook -= OnNewGame;
            ModHooks.SavegameLoadHook -= OnSaveLoaded;
        }
    }
}
