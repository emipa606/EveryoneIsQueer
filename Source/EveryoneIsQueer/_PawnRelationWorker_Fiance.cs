using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer
{
	public class _PawnRelationWorker_Fiance :  PawnRelationWorker_Fiance
	{
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return EveryoneIsQueer_Mod.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}
		
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}
	}
}
