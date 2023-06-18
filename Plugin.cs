using BepInEx;
using BepInEx.Configuration;

namespace BetterHitReaction
{
    [BepInPlugin("com.takenmake.betterhitreaction", "Better Hit Reaction", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<float> factor;

        private void Awake()
        {
            // Plugin startup logic
            factor = Config.Bind("General", "Factor", 0.5f, new ConfigDescription("The factor to multiply the effect's strength by.", new AcceptableValueRange<float>(0f, 1000f)));

            new HitStaminaPatch().Enable();

            Logger.LogInfo("Plugin com.takenmake.betterhitreaction is loaded!");
        }

        public static float GetFactor()
        {
            return factor.Value;
        }
    }
}
