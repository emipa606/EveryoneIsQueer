using HarmonyLib;
using RimWorld;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(RelationsUtility), nameof(RelationsUtility.AttractedToGender))]
internal static class RelationsUtility_AttractedToGender
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}