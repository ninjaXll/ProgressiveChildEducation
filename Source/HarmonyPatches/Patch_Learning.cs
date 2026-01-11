using HarmonyLib;
using Verse;
using RimWorld;
using UnityEngine; // Necessário para o Mathf.Min

namespace ProgressiveChildEducation
{
    // O método correto é "NeedInterval"
    [HarmonyPatch(typeof(Need_Learning), "NeedInterval")]
    public static class Patch_Learning
    {
        // TRUQUE DO HARMONY: ___pawn (3 sublinhados) acessa a variável protegida sem dar erro de compilação
        public static void Postfix(Pawn ___pawn)
        {
            // PROTEÇÃO 1: Validação básica
            // Removemos o !Spawned conforme sugestão para não quebrar caravanas
            if (___pawn == null || ___pawn.Dead || !___pawn.IsColonist) return;

            try 
            {
                // PROTEÇÃO 2: Verifica se o trabalho (Job) é válido
                if (___pawn.CurJob == null || ___pawn.CurJob.def == null) return;

                string jobName = ___pawn.CurJob.def.defName;

                // LÓGICA: Só aplica XP se estiver tendo aula
                if (jobName == "LessonTaking" || jobName == "Lessongiving")
                {
                    var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false);
                    
                    // Verifica se o hediff e a saúde existem
                    if (hediffDef != null && ___pawn.health != null && ___pawn.health.hediffSet != null)
                    {
                        var hediff = ___pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                        
                        // APLICA O XP com limite máximo (Clamp)
                        if (hediff != null && hediff.Severity < 1.0f)
                        {
                            // Mathf.Min garante que nunca passe de 1.0f
                            hediff.Severity = Mathf.Min(hediff.Severity + 0.0003f, 1.0f);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Loga o erro UMA vez para você saber, mas não trava a tela (Cronograma Cinza)
                Log.ErrorOnce($"[PE] Erro no Patch_Learning: {ex}", 123456789);
            }
        }
    }
}