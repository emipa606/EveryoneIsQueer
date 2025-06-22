using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender),
    null)]
internal static class LovePartnerRelationUtility_HasAnyLovePartnerOfTheSameGender
{
    public static void Postfix(ref bool __result)
    {
        __result = false;
    }
}