using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer;

[HarmonyPatch(typeof(RelationsUtility), "AttractedToGender")]
internal static class _RelationsUtility_AttractedToGender
{
    [HarmonyPrefix]
    public static bool CheckIfAttractedToGender(ref bool __result)
    {
        __result = true;
        return false;
    }
}