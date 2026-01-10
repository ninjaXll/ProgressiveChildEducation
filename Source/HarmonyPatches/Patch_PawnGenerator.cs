using HarmonyLib;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new System.Type[] { typeof(PawnGenerationRequest) })]
    public static class Patch_PawnGenerator
    {
        public static void Postfix(Pawn __result)
        {
            // Se o peão não foi gerado ou não é humano, sai fora
            if (__result == null || __result.RaceProps == null || !__result.RaceProps.Humanlike) return;
            if (__result.health == null || __result.ageTracker == null) return;

            try
            {
                var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false);
                if (hediffDef == null) return;
                
                // Evita adicionar o hediff se já tiver
                if (__result.health.hediffSet.HasHediff(hediffDef)) return;

                if (__result.ageTracker.Adult)
                {
                    var education = __result.health.AddHediff(hediffDef);
                    education.Severity = 1.0f;
                }
                else
                {
                    float progress = __result.ageTracker.AgeBiologicalYearsFloat / __result.ageTracker.AdultMinAge;
                    var education = __result.health.AddHediff(hediffDef);
                    education.Severity = UnityEngine.Mathf.Clamp(progress, 0.1f, 0.7f);
                }
            }
            catch
            {
                // Ignora erros na geração para não travar o jogo
            }
        }
    }
}