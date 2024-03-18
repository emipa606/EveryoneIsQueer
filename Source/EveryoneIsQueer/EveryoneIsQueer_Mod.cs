using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace EveryoneIsQueer;

/// <summary>
///     Mod to make everyone equally likely to fall in love irrespective of gender.
/// </summary>
public class EveryoneIsQueer_Mod : Mod
{
    public EveryoneIsQueer_Mod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("io.github.mgraly.everythingisqueer");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    // LovePartnerRelationUtility.LovePartnerRelationGenerationChance, but with the gender and sexuality code removed.
    public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, bool ex)
    {
        if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
        {
            return 0f;
        }

        if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
        {
            return 0f;
        }

        var num = 1f;
        if (ex)
        {
            var num2 = 0;
            var directRelations = other.relations.DirectRelations;
            foreach (var directPawnRelation in directRelations)
            {
                if (LovePartnerRelationUtility.IsExLovePartnerRelation(directPawnRelation.def))
                {
                    num2++;
                }
            }

            num = Mathf.Pow(0.2f, num2);
        }
        else if (LovePartnerRelationUtility.HasAnyLovePartner(other))
        {
            return 0f;
        }

        var generationChanceAgeFactor = GetGenerationChanceAgeFactor(generated);
        var generationChanceAgeFactor2 = GetGenerationChanceAgeFactor(other);
        var generationChanceAgeGapFactor = GetGenerationChanceAgeGapFactor(generated, other, ex);
        var num3 = 1f;
        if (generated.GetRelations(other).Any(x => x.familyByBloodRelation))
        {
            num3 = 0.01f;
        }

        //var num4 = request.FixedMelanin.HasValue
        //    ? ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin)
        //    : PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);

        return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3;
        //* num4;
    }

    private static float GetGenerationChanceAgeFactor(Pawn p)
    {
        var value = GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat);
        return Mathf.Clamp(value, 0f, 1f);
    }

    private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
    {
        var num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
        if (ex)
        {
            var num2 = MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
            if (num2 >= 0f)
            {
                num = Mathf.Min(num, num2);
            }

            var num3 = MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
            if (num3 >= 0f)
            {
                num = Mathf.Min(num, num3);
            }
        }

        if (num > 40f)
        {
            return 0f;
        }

        var value = GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num);
        return Mathf.Clamp(value, 0.001f, 1f);
    }

    private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
    {
        var num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
        if (num < 0f)
        {
            Log.Warning("at < 0");
            return 0f;
        }

        var num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat,
            p2.ageTracker.AgeChronologicalYearsFloat, num);
        var num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
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