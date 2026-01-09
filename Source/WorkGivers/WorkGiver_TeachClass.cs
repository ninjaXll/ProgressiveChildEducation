using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class WorkGiver_TeachClass : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ThingDef.Named("PE_InfantDesk"));

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!t.Spawned || t.IsForbidden(pawn)) return false;

            var turma = SchoolManager.Current.classes.Find(c => c.teacher == pawn);
            if (turma == null) return false;

            int hour = GenLocalDate.HourOfDay(pawn.Map);
            if (hour < turma.startHour || hour >= (turma.startHour + turma.duration)) return false;

            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("PE_TeachClass"), t);
        }
    }
}