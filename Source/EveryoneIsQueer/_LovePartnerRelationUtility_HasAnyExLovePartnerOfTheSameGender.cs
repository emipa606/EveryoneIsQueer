using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer;

[HarmonyPatch(typeof(LovePartnerRelationUtility), "HasAnyExLovePartnerOfTheSameGender", null)]
internal static class _LovePartnerRelationUtility_HasAnyExLovePartnerOfTheSameGender
{
    [HarmonyPostfix]
    public static void HasAnyExLovePartnerOfTheSameGender(ref bool __result)
    {
        __result = false;
    }
}