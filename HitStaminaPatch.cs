using System.Reflection;
using EFT;
using Aki.Reflection.Patching;
using UnityEngine;

namespace BetterHitReaction
{
    public class HitStaminaPatch : ModulePatch
    {   
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("ApplyHitDebuff", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        static bool Prefix(ref Player __instance, float ___float_44, int ___int_5, float damage, float staminaBurnRate, EBodyPart bodyPartType, EDamageType damageType)
        {
            if (damageType.IsEnemyDamage())
            {
                __instance.IncreaseAwareness(20f);
            }

            if (__instance.HealthController.IsAlive && (!__instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers) || damage > 4f) && !__instance.IsAI)
            {
                if (__instance.Speaker != null)
                {
                    __instance.Speaker.Play(EPhraseTrigger.OnBeingHurt, __instance.HealthStatus, true, null);
                }
                else
                {
                    Debug.LogError("Player Speaker is null");
                }
            }

            if (!damageType.IsWeaponInduced())
            {
                return false;
            }

            ___float_44 = ((___int_5 == Time.frameCount) ? (___float_44 + staminaBurnRate) : staminaBurnRate);
            float num = Mathf.InverseLerp(55f, 10f, ___float_44);

            if (num < 1f)
            {
                Logger.LogInfo($"num before: {num}, num after: {num*Plugin.GetFactor()}");
                __instance.UpdateSpeedLimit(num*Plugin.GetFactor(), Player.ESpeedLimit.Shot, 0.66f);
            }

            ___int_5 = Time.frameCount;

            Logger.LogInfo($"staminaBurnRate before: {staminaBurnRate}, staminaBurnRate after: {staminaBurnRate*Plugin.GetFactor()}");
            __instance.Physical.BulletHit(staminaBurnRate*Plugin.GetFactor());

            if ((bodyPartType == EBodyPart.LeftLeg || bodyPartType == EBodyPart.RightLeg) && !__instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers))
            {
                __instance.Physical.Sprint(false);
            }

            return false;
        }
    }
}
