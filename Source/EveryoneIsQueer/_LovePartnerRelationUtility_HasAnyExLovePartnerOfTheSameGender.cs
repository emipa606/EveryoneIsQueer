using HarmonyLib;
using RimWorld;
using Verse;

namespace EveryoneIsQueer;

[HarmonyPatch(typeof(LovePartnerRelationUtility), "HasAnyExLovePartnerOfTheSameGender", null)]
internal static class _LovePartnerRelationUtility_HasAnyExLovePartnerOfTheSameGender
{
    [HarmonyPostfix]
    public static void HasAnyExLovePartnerOfTheSameGender(Pawn pawn, ref bool __result)
    {
        __result = false;
    }
}