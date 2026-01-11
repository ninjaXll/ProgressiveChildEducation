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
            if (__result) return; 

            // Se não for adulto, força o skip
            if (pawn.ageTracker != null && !pawn.ageTracker.Adult)
            {
                __result = true;
            }
        }
    }
    // APAGUE TUDO O QUE TIVER DAQUI PARA BAIXO!
}