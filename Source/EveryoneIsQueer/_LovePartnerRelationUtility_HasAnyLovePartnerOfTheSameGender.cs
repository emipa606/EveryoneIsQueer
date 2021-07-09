using HarmonyLib;
using RimWorld;
using Verse;

namespace EveryoneIsQueer
{
    [HarmonyPatch(typeof(LovePartnerRelationUtility), "HasAnyLovePartnerOfTheSameGender", null)]
    internal static class _LovePartnerRelationUtility_HasAnyLovePartnerOfTheSameGender
    {
        [HarmonyPostfix]
        public static void HasAnyLovePartnerOfTheSameGender(Pawn pawn, ref bool __result)
        {
            __result = false;
        }
    }
}