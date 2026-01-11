using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class WorkGiver_AttendClass : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ThingDef.Named("PE_InfantDesk"));

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            // --- PROTEÇÃO DE SEGURANÇA (NOVA) ---
            // Impede erros vermelhos se o SchoolManager ainda não tiver carregado (comum ao carregar saves)
            if (SchoolManager.Current == null || SchoolManager.Current.classes == null) return false;
            // ------------------------------------

            if (!t.Spawned || t.IsForbidden(pawn) || !pawn.CanReserve(t)) return false;

            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(pawn));
            if (turma == null) return false;

            int hour = GenLocalDate.HourOfDay(pawn.Map);
            if (hour < turma.startHour || hour >= (turma.startHour + turma.duration)) return false;
            
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            // Proteção extra aqui também, só para garantir
            if (SchoolManager.Current == null || SchoolManager.Current.classes == null) return null;

            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(pawn));
            return JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("PE_AttendClass"), t, turma?.teacher);
        }
    }
}