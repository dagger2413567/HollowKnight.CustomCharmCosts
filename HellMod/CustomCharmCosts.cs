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
        }

        private int OnInt(string intName, int orig) => intName switch
        {
            // Gathering, Wayfayers, Soul Catcher, Soul Eater, Dashmaster, Quick Slash, SprintMaster, Grimmchild
            "charmCost_01" => _settings.GSwarmCost,
            "charmCost_02" => _settings.WayfairerCost,
            "charmCost_20" => _settings.CatcherCost,
            "charmCost_21" => _settings.EaterCost,
            "charmCost_31" => _settings.DashmasterCost,
            "charmCost_32" => _settings.QuickCost,
            "charmCost_37" => _settings.SprintCost,
            "charmCost_40" => _settings.GrimmCost,

            _ => orig
        };

        public void Unload()
        {
            ModHooks.GetPlayerIntHook -= OnInt;
        }
    }
}
