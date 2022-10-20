using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer;

/// <summary>
///     Override behavior for Romance Attempts to ignore gender and sexuality.
/// </summary>
public class _InteractionWorker_RomanceAttempt : InteractionWorker_RomanceAttempt
{
    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        if (LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
        {
            return 0f;
        }

        var num = initiator.relations.SecondaryRomanceChanceFactor(recipient);
        if (num < 0.25f)
        {
            return 0f;
        }

        var num2 = initiator.relations.OpinionOf(recipient);
        if (num2 < 5)
        {
            return 0f;
        }

        if (recipient.relations.OpinionOf(initiator) < 5)
        {
            return 0f;
        }

        var num3 = 1f;
        var pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
        if (pawn != null)
        {
            float value = initiator.relations.OpinionOf(pawn);
            num3 = Mathf.InverseLerp(50f, -50f, value);
        }

        var num5 = Mathf.InverseLerp(0.25f, 1f, num);
        var num6 = Mathf.InverseLerp(5f, 100f, num2);
        return 1.15f * num5 * num6 * num3;
    }
}