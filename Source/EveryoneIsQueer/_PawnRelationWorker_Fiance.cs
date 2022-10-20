using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer;

public class _PawnRelationWorker_Fiance : PawnRelationWorker_Fiance
{
    public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
    {
        var num = 1f;
        num *= GetOldAgeFactor(generated);
        num *= GetOldAgeFactor(other);
        return EveryoneIsQueer_Mod.LovePartnerRelationGenerationChance(generated, other, request, false) *
               BaseGenerationChanceFactor(generated, other, request) * num;
    }

    private float GetOldAgeFactor(Pawn pawn)
    {
        return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
    }
}