using RimWorld;
using Verse;

namespace EveryoneIsQueer
{
    public class _PawnRelationWorker_ExSpouse : PawnRelationWorker_ExSpouse
    {
        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            return EveryoneIsQueer_Mod.LovePartnerRelationGenerationChance(generated, other, request, true) *
                   BaseGenerationChanceFactor(generated, other, request);
        }
    }
}