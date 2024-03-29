﻿using RimWorld;
using Verse;

namespace EveryoneIsQueer;

public class _PawnRelationWorker_ExLover : PawnRelationWorker_ExLover
{
    public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
    {
        return EveryoneIsQueer_Mod.LovePartnerRelationGenerationChance(generated, other, true) *
               BaseGenerationChanceFactor(generated, other, request);
    }
}