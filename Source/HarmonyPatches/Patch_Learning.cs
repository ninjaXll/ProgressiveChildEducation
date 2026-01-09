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
            // Verificação de segurança
            if (___pawn == null || ___pawn.CurJob == null || ___pawn.CurJob.def == null) return;

            // CORREÇÃO: Pegamos o nome do Job como texto para evitar erro de referência
            string jobName = ___pawn.CurJob.def.defName;

            // "LessonTaking" é o nome interno da tarefa de estudar
            if (jobName == "LessonTaking" || jobName == "Lessongiving")
            {
                var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false);
                if (hediffDef == null) return;

                var hediff = ___pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                
                if (hediff != null && hediff.Severity < 1.0f)
                {
                    hediff.Severity += 0.0003f;
                }
            }
        }
    }
}