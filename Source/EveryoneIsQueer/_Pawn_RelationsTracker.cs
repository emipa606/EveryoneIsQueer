using HarmonyLib;
using RimWorld;
using UnityEngine;
using System;
using System.Reflection;
using Verse;

namespace EveryoneIsQueer
{
	[HarmonyPatch(typeof(Pawn_RelationsTracker), "SecondaryLovinChanceFactor", null)]
	internal static class _Pawn_RelationsTracker
	{
		internal static FieldInfo _pawn;

		[HarmonyPostfix]
		public static void SecondaryLovinChanceFactor(Pawn_RelationsTracker __instance, Pawn otherPawn, ref float __result)
		{
			Pawn pawn = __instance.GetPawn();
			if (pawn.def != otherPawn.def || pawn == otherPawn) {
				__result = 0f;
				return;
			}
			float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
			float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
			float num = 1f;
			if (ageBiologicalYearsFloat2 < 16f) {
				__result = 0f;
				return;
			}
			float min = Mathf.Max(16f, ageBiologicalYearsFloat - 30f);
			float lower = Mathf.Max(20f, ageBiologicalYearsFloat, ageBiologicalYearsFloat - 10f);
			num = GenMath.FlatHill(0.15f, min, lower, ageBiologicalYearsFloat + 7f, ageBiologicalYearsFloat + 30f, 0.15f, ageBiologicalYearsFloat2);
			float num2 = 1f;
			num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Talking));
			num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
			num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			int num3 = 0;
			if (otherPawn.RaceProps.Humanlike) {
				num3 = otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
			}
			float num4 = 1f;
			if (num3 < 0) {
				num4 = 0.3f;
			} else if (num3 > 0) {
				num4 = 2.3f;
			}
			float num5 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat);
			float num6 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat2);
			__result = num * num2 * num5 * num6 * num4;
		}
		private static Pawn GetPawn(this Pawn_RelationsTracker _this)
		{
			bool flag = _Pawn_RelationsTracker._pawn == null;
			if (flag) {
				_Pawn_RelationsTracker._pawn = typeof(Pawn_RelationsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic);
				bool flag2 = _Pawn_RelationsTracker._pawn == null;
				if (flag2) {
					Log.ErrorOnce("Unable to reflect Pawn_RelationsTracker.pawn!", 305432421);
				}
			}
			return (Pawn)_Pawn_RelationsTracker._pawn.GetValue(_this);
		}
	}
}
