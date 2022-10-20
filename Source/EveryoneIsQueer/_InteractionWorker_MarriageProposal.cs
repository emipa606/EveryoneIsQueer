using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer;

/// <summary>
///     Override behavior for Marriage proposals to make both genders equally likely to initiate.
/// </summary>
public class _InteractionWorker_MarriageProposal : InteractionWorker_MarriageProposal
{
    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        var directRelation = initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient);
        if (directRelation == null)
        {
            return 0f;
        }

        var spouse = recipient.GetFirstSpouse();
        var spouse2 = initiator.GetFirstSpouse();
        if (spouse is { Dead: false } || spouse2 is { Dead: false })
        {
            return 0f;
        }

        var num = 0.4f;
        var ticksGame = Find.TickManager.TicksGame;
        var value = (ticksGame - directRelation.startTicks) / 60000f;
        num *= Mathf.InverseLerp(0f, 60f, value);
        num *= Mathf.InverseLerp(0f, 60f, initiator.relations.OpinionOf(recipient));
        if (recipient.relations.OpinionOf(initiator) < 0)
        {
            num *= 0.3f;
        }

        return num;
    }
}