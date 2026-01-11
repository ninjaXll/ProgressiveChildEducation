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
            if (pawn.ageTracker != null && !pawn.ageTracker.Adult)
            {
                __result = true;
            }
        }
    }
    
    // AQUI TINHA O CÓDIGO DO "SetPriority". 
    // CERTIFIQUE-SE DE QUE ELE FOI APAGADO E NÃO EXISTE MAIS NADA AQUI EMBAIXO!
}