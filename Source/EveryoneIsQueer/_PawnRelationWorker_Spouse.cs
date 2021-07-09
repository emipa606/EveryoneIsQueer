using RimWorld;
using Verse;

namespace EveryoneIsQueer
{
    public class _PawnRelationWorker_Spouse : PawnRelationWorker_Spouse
    {
        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            return EveryoneIsQueer_Mod.LovePartnerRelationGenerationChance(generated, other, request, false) *
                   BaseGenerationChanceFactor(generated, other, request);
        }
    }
}