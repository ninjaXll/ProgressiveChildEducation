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
            if (__result == null || !__result.RaceProps.Humanlike || __result.health == null) return;

            var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel");
            
            // Lógica para Adultos (Baseado na raça)
            if (__result.ageTracker.Adult)
            {
                var education = __result.health.AddHediff(hediffDef);
                education.Severity = 1.0f;
            }
            // Lógica para Crianças Geradas (Invasores/Refugiados)
            else
            {
                float progress = __result.ageTracker.AgeBiologicalYearsFloat / __result.ageTracker.AdultMinAge;
                var education = __result.health.AddHediff(hediffDef);
                // Garante que não venham zerados se já tiverem alguma idade
                education.Severity = UnityEngine.Mathf.Clamp(progress, 0.1f, 0.7f);
            }
        }
    }
}