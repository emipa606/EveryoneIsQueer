using HarmonyLib;
using RimWorld;
using Verse;

namespace EveryoneIsQueer.HarmonyPatches;

[HarmonyPatch(typeof(RelationsUtility), nameof(RelationsUtility.RomanceEligible))]
internal static class RelationsUtility_RomanceEligible
{
    public static bool Prefix(Pawn pawn, bool initiator, bool forOpinionExplanation,
        ref AcceptanceReport __result)
    {
        if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
        {
            __result = false;
            return false;
        }

        if (pawn.IsPrisoner)
        {
            if (!initiator || forOpinionExplanation)
            {
                __result = AcceptanceReport.WasRejected;
                return false;
            }

            __result = "CantRomanceInitiateMessagePrisoner".Translate(pawn).CapitalizeFirst();
            return false;
        }

        if (pawn.Downed && !forOpinionExplanation)
        {
            __result = initiator
                ? "CantRomanceInitiateMessageDowned".Translate(pawn).CapitalizeFirst()
                : "CantRomanceTargetDowned".Translate();
            return false;
        }

        if (initiator && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
        {
            if (!forOpinionExplanation)
            {
                __result = "CantRomanceInitiateMessageTalk".Translate(pawn).CapitalizeFirst();
                return false;
            }

            __result = AcceptanceReport.WasRejected;
            return false;
        }

        if (pawn.Drafted && !forOpinionExplanation)
        {
            __result = initiator
                ? "CantRomanceInitiateMessageDrafted".Translate(pawn).CapitalizeFirst()
                : "CantRomanceTargetDrafted".Translate();
            return false;
        }

        if (initiator && pawn.IsSlave)
        {
            if (!forOpinionExplanation)
            {
                __result = "CantRomanceInitiateMessageSlave".Translate(pawn).CapitalizeFirst();
                return false;
            }

            __result = AcceptanceReport.WasRejected;
            return false;
        }

        if (pawn.MentalState != null)
        {
            __result = initiator && !forOpinionExplanation
                ? "CantRomanceInitiateMessageMentalState".Translate(pawn).CapitalizeFirst()
                : "CantRomanceTargetMentalState".Translate();
            return false;
        }

        __result = true;
        return false;
    }
}