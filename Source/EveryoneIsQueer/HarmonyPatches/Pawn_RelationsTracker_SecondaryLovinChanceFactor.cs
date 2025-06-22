using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryLovinChanceFactor), null)]
internal static class Pawn_RelationsTracker_SecondaryLovinChanceFactor
{
    private static FieldInfo pawnFieldInfo;

    public static void Postfix(Pawn_RelationsTracker __instance, Pawn otherPawn,
        ref float __result)
    {
        var pawn = __instance.getPawn();
        if (pawn.def != otherPawn.def || pawn == otherPawn)
        {
            __result = 0f;
            return;
        }

        var ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
        var ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
        if (ageBiologicalYearsFloat2 < 16f)
        {
            __result = 0f;
            return;
        }

        var min = Mathf.Max(16f, ageBiologicalYearsFloat - 30f);
        var lower = Mathf.Max(20f, ageBiologicalYearsFloat, ageBiologicalYearsFloat - 10f);
        var num = GenMath.FlatHill(0.15f, min, lower, ageBiologicalYearsFloat + 7f, ageBiologicalYearsFloat + 30f,
            0.15f, ageBiologicalYearsFloat2);
        var num2 = 1f;
        num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Talking));
        num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
        num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
        var num3 = 0;
        if (otherPawn.RaceProps.Humanlike)
        {
            num3 = otherPawn.story.traits.DegreeOfTrait(DefDatabase<TraitDef>.GetNamedSilentFail("Beauty"));
        }

        var num4 = 1f;
        switch (num3)
        {
            case < 0:
                num4 = 0.3f;
                break;
            case > 0:
                num4 = 2.3f;
                break;
        }

        var num5 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat);
        var num6 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat2);
        __result = num * num2 * num5 * num6 * num4;
    }

    private static Pawn getPawn(this Pawn_RelationsTracker _this)
    {
        if (pawnFieldInfo != null)
        {
            return (Pawn)pawnFieldInfo.GetValue(_this);
        }

        pawnFieldInfo = typeof(Pawn_RelationsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic);
        if (pawnFieldInfo == null)
        {
            Log.ErrorOnce("Unable to reflect Pawn_RelationsTracker.pawn!", 305432421);
        }

        return (Pawn)pawnFieldInfo?.GetValue(_this);
    }
}