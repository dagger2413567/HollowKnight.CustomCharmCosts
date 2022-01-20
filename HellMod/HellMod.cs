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

        private int OnInt(string intName, int orig)
        {
            return intName switch
            {
                // Dreamshield
                "charmCost_01" => 0,
                "charmCost_02" => 0,
                "charmCost_01" => 0,
                "charmCost_01" => 0,

                _ => orig
            };
        }

        private int OnHealthTaken(int damage)
        {
            return _settings.DoubleDeamage
                ? damage * 2
                : damage;
        }

        private bool _roundedSoul;

        private int OnSoulGain(int amount)
        {
            if (!_settings.LimitSoulGain)
                return amount;

            _roundedSoul = !_roundedSoul;

            // First hit is rounded down, second is rounded up
            var pd = PlayerData.instance;

            // If we have a shade, no soul
            // Otherwise swap between ceiling and floor to allow it to still be in 6 hits.
            amount = pd.soulLimited
                    ? 0
                    : _roundedSoul
                        ? amount / 2
                        : (int) Math.Ceiling((float) amount / 2)
                ;

            return amount;
        }

        public void Unload()
        {
            ModHooks.TakeHealthHook -= OnHealthTaken;
            ModHooks.SoulGainHook -= OnSoulGain;
            ModHooks.GetPlayerIntHook -= OnInt;
            ModHooks.NewGameHook -= OnNewGame;
            ModHooks.SavegameLoadHook -= OnSaveLoaded;
            ModHooks.HitInstanceHook -= OnHit;
        }
    }
}
