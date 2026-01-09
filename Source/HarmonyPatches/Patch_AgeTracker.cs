using HarmonyLib;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "BirthdayBiological")]
    public static class Patch_AgeTracker
    {
        public static void Postfix(Pawn_AgeTracker __instance, Pawn ___pawn)
        {
            if (__instance.Adult)
            {
                var hediffDef = DefDatabase<HediffDef>.GetNamed("PE_EducationLevel");
                var education = ___pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);

                if (education != null && education.Severity < 1.0f)
                {
                    education.Severity = 1.0f;
                    Messages.Message("PE_BecameAdultEducationUnlocked".Translate(___pawn.LabelShort), ___pawn, MessageTypeDefOf.PositiveEvent);
                }
                else if (education == null)
                {
                    var newEd = ___pawn.health.AddHediff(hediffDef);
                    newEd.Severity = 1.0f;
                }
            }
        }
    }
}