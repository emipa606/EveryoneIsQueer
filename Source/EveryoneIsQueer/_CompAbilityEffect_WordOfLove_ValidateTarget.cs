using HarmonyLib;
using RimWorld;
using Verse;

namespace EveryoneIsQueer;

[HarmonyPatch(typeof(CompAbilityEffect_WordOfLove), "ValidateTarget")]
internal static class _CompAbilityEffect_WordOfLove_ValidateTarget
{
    [HarmonyPrefix]
    public static void AddBisexual(LocalTargetInfo ___selectedTarget, out bool __state)
    {
        var pawn = ___selectedTarget.Pawn;
        __state = false;
        if (pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
        {
            return;
        }

        pawn.story.traits.allTraits.Add(new Trait(TraitDefOf.Bisexual));
        __state = true;
    }

    [HarmonyPostfix]
    public static void RemoveBisexual(LocalTargetInfo ___selectedTarget, bool __state)
    {
        var pawn = ___selectedTarget.Pawn;
        if (__state)
        {
            pawn.story.traits.allTraits.Remove(new Trait(TraitDefOf.Bisexual));
        }
    }
}