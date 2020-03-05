using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using HarmonyLib;
using Verse;
using UnityEngine;


namespace EveryoneIsQueer
{
	/// <summary>
	/// Mod to make everyone equally likely to fall in love irrespective of gender.
	/// </summary>
	public class EveryoneIsQueer_Mod : Mod
	{

		public EveryoneIsQueer_Mod(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("io.github.mgraly.everythingisqueer");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
		
		// LovePartnerRelationUtility.LovePartnerRelationGenerationChance, but with the gender and sexuality code removed.
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			float num = 1f;
			if (ex)
			{
				int num2 = 0;
				List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
					{
						num2++;
					}
				}
				num = Mathf.Pow(0.2f, (float)num2);
			}
			else if (LovePartnerRelationUtility.HasAnyLovePartner(other))
			{
				return 0f;
			}
			float generationChanceAgeFactor = GetGenerationChanceAgeFactor(generated);
			float generationChanceAgeFactor2 = GetGenerationChanceAgeFactor(other);
			float generationChanceAgeGapFactor = GetGenerationChanceAgeGapFactor(generated, other, ex);
			float num3 = 1f;
			if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
			{
				num3 = 0.01f;
			}
			float num4;
			if (request.FixedMelanin.HasValue)
			{
				num4 = ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin);
			}
			else
			{
				num4 = PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);
			}
			return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num4;
		}
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			float value = GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat);
			return Mathf.Clamp(value, 0f, 1f);
		}

		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			if (num > 40f)
			{
				return 0f;
			}
			float value = GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num);
			return Mathf.Clamp(value, 0.001f, 1f);
		}

		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			if (num < 0f)
			{
				Log.Warning("at < 0", false);
				return 0f;
			}
			float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
			float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
			if (num2 < 0f)
			{
				return -1f;
			}
			if (num2 < 14f)
			{
				return -1f;
			}
			if (num3 <= 14f)
			{
				return 0f;
			}
			return num3 - 14f;
		}
	}
}
