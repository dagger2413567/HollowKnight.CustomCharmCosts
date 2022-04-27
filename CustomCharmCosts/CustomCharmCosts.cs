using JetBrains.Annotations;
using Modding;
using System.Reflection;

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

        public override int LoadPriority() => 10;

        public override void Initialize()
        {
            Log("Initializing");

            ModHooks.GetPlayerIntHook += OnInt;
        }

        private int OnInt(string intName, int orig) => intName switch
        {
            // Gathering, Wayfayers, Soul Catcher, Soul Eater, Dashmaster, Quick Slash, SprintMaster, Grimmchild, Heart/Greed/Strength
            "charmCost_1" => _settings.GSwarmCost,
            "charmCost_2" => _settings.WayfairerCost,
            "charmCost_7" => PlayerData.instance.GetBool("gotCharm_34") ? 0 :_settings.QuickFocusCost,
            "charmCost_20" => _settings.CatcherCost,
            "charmCost_21" => _settings.EaterCost,
            "charmCost_23" => PlayerData.instance.fragileHealth_unbreakable ? _settings.UHeartCost : 2,
            "charmCost_24" => PlayerData.instance.fragileGreed_unbreakable ? _settings.UGreedCost : 2,
            "charmCost_25" => PlayerData.instance.fragileStrength_unbreakable ? _settings.UStrengthCost : 3,
            "charmCost_31" => _settings.DashmasterCost,
            "charmCost_32" => _settings.QuickCost,
            "charmCost_34" => PlayerData.instance.GetBool("gotCharm_7") ? 0 : _settings.DeepFocusCost,
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
