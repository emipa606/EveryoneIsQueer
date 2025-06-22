using HarmonyLib;
using RimWorld;
using Verse;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(CompAbilityEffect_WordOfLove), nameof(CompAbilityEffect_WordOfLove.ValidateTarget))]
internal static class CompAbilityEffect_WordOfLove_ValidateTarget
{
    public static void Prefix(LocalTargetInfo ___selectedTarget, out bool __state)
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

    public static void Postfix(LocalTargetInfo ___selectedTarget, bool __state)
    {
        var pawn = ___selectedTarget.Pawn;
        if (__state)
        {
            pawn.story.traits.allTraits.Remove(new Trait(TraitDefOf.Bisexual));
        }
    }
}