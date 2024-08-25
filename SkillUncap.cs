using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkillUncap;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInIncompatibility("org.bepinex.plugins.valheim_plus")]
public class SkillUncap : BaseUnityPlugin
{
    private const string ModName = "Smart Skills";
    private const string ModVersion = "1.0.2";
    private const string ModGUID = "org.bepinex.plugins.smartskills";

    public void Awake()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: ModGUID);
    }

    [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
    private static class IncreaseSneakDamage
    {
        private static void Prefix(Character __instance, HitData hit)
        {
            if (hit.GetAttacker() is Player player && __instance.GetBaseAI() is { m_alertedMessage: "" } baseAi && !baseAi.HaveTarget())
            {
                hit.m_backstabBonus *= 1 + player.GetSkillFactor(Skills.SkillType.Sneak) * 0.5f;
                player.RaiseSkill(Skills.SkillType.Sneak, 20f);
            }
        }
    }
}

