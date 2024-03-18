using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer;

[HarmonyPatch(typeof(LovePartnerRelationUtility), "HasAnyLovePartnerOfTheSameGender", null)]
internal static class _LovePartnerRelationUtility_HasAnyLovePartnerOfTheSameGender
{
    [HarmonyPostfix]
    public static void HasAnyLovePartnerOfTheSameGender(ref bool __result)
    {
        __result = false;
    }
}