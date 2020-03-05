using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer
{
	/// <summary>
	/// Override behavior for Marriage proposals to make both genders equally likely to initiate.
	/// </summary>
	public class _InteractionWorker_MarriageProposal : InteractionWorker_MarriageProposal
	{
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			DirectPawnRelation directRelation = initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient);
			if (directRelation == null) {
				return 0f;
			}
			Pawn spouse = recipient.GetSpouse();
			Pawn spouse2 = initiator.GetSpouse();
			if ((spouse != null && !spouse.Dead) || (spouse2 != null && !spouse2.Dead)) {
				return 0f;
			}
			float num = 0.4f;
			int ticksGame = Find.TickManager.TicksGame;
			float value = (float)(ticksGame - directRelation.startTicks) / 60000f;
			num *= Mathf.InverseLerp(0f, 60f, value);
			num *= Mathf.InverseLerp(0f, 60f, (float)initiator.relations.OpinionOf(recipient));
			if (recipient.relations.OpinionOf(initiator) < 0) {
				num *= 0.3f;
			}
			return num;
		}
	}
}
