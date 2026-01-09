using HarmonyLib;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    [HarmonyPatch(typeof(WorkGiver_Researcher), "ShouldSkip")]
    public static class Patch_Research
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (__result) return; // Se já foi pulado, ignora.

            // Se não for adulto, força o skip (bloqueia a tarefa)
            if (!pawn.ageTracker.Adult)
            {
                __result = true;
            }
        }
    }

    // Opcional: Bloqueio visual na UI de prioridades
    [HarmonyPatch(typeof(Pawn_WorkSettings), "SetPriority")]
    public static class Patch_WorkSettings_ForbidResearch
    {
        public static bool Prefix(Pawn ___pawn, WorkTypeDef w, int priority)
        {
            if (w.defName == "Research" && priority > 0 && !___pawn.ageTracker.Adult)
            {
                Messages.Message("PE_ChildCannotResearch".Translate(), ___pawn, MessageTypeDefOf.RejectInput, false);
                return false; 
            }
            return true;
        }
    }
}