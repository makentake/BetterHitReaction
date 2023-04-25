using BepInEx;
using BepInEx.Configuration;

namespace BetterHitReaction
{
    [BepInPlugin("com.takenmake.betterhitreaction", "Better Hit Reaction", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<float> factor;

        private void Awake()
        {
            // Plugin startup logic
            factor = Config.Bind("General", "factor", 0.5f, "The factor to multiply the effect's strength by.");

            new HitStaminaPatch().Enable();

            Logger.LogInfo("Plugin com.takenmake.betterhitreaction is loaded!");
        }

        public static float GetFactor()
        {
            return factor.Value;
        }
    }
}
