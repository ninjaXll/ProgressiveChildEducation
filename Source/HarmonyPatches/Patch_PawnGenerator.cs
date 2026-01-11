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
            // PROTEÇÃO 1: Peão nulo ou não humano
            if (__result == null || __result.RaceProps == null || !__result.RaceProps.Humanlike) return;

            // PROTEÇÃO 2 (Nova): Só aplica em Colonos.
            // Evita bugs com visitantes, inimigos e pawns temporários que não precisam de educação.
            if (!__result.IsColonist) return;

            // PROTEÇÃO 3: Dados inválidos
            if (__result.health == null || __result.ageTracker == null) return;

            try
            {
                var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false);
                if (hediffDef == null) return;
                
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
                // Ignora erros silenciosamente na geração
            }
        }
    }
}