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

        var generationChanceAgeFactor = getGenerationChanceAgeFactor(generated);
        var generationChanceAgeFactor2 = getGenerationChanceAgeFactor(other);
        var generationChanceAgeGapFactor = getGenerationChanceAgeGapFactor(generated, other, ex);
        var num3 = 1f;
        if (generated.GetRelations(other).Any(x => x.familyByBloodRelation))
        {
            num3 = 0.01f;
        }

        return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3;
    }

    private static float getGenerationChanceAgeFactor(Pawn p)
    {
        var value = GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat);
        return Mathf.Clamp(value, 0f, 1f);
    }

    private static float getGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
    {
        var num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
        if (ex)
        {
            var num2 = minPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
            if (num2 >= 0f)
            {
                num = Mathf.Min(num, num2);
            }

            var num3 = minPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
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

    private static float minPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
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
        switch (num2)
        {
            case < 0f:
            case < 14f:
                return -1f;
        }

        if (num3 <= 14f)
        {
            return 0f;
        }

        return num3 - 14f;
    }
}