using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender),
    null)]
internal static class LovePartnerRelationUtility_HasAnyExLovePartnerOfTheSameGender
{
    public static void Postfix(ref bool __result)
    {
        __result = false;
    }
}