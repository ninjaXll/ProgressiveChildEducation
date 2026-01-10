using HarmonyLib;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    [HarmonyPatch(typeof(Need_Learning), "LearningInterval")]
    public static class Patch_Learning
    {
        public static void Postfix(Pawn ___pawn)
        {
            // PROTEÇÃO TOTAL: Se qualquer coisa for nula, cancela imediatamente
            if (___pawn == null || ___pawn.CurJob == null || ___pawn.CurJob.def == null) return;
            if (___pawn.health == null || ___pawn.health.hediffSet == null) return;

            try 
            {
                string jobName = ___pawn.CurJob.def.defName;

                if (jobName == "LessonTaking" || jobName == "Lessongiving")
                {
                    var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false);
                    if (hediffDef != null)
                    {
                        var hediff = ___pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                        // Verifica se hediff existe e não está cheio
                        if (hediff != null && hediff.Severity < 1.0f)
                        {
                            hediff.Severity += 0.0003f;
                        }
                    }
                }
            }
            catch
            {
                // Se der erro, ignora silenciosamente para não quebrar o Cronograma
            }
        }
    }
}