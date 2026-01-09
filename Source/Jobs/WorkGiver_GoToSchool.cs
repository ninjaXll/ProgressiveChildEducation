using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    // OBS: Certifique-se que o nome do arquivo bate com o nome da classe se possível, 
    // mas o importante é o conteúdo abaixo.
    public class WorkGiver_GoToSchool : WorkGiver_Scanner
    {
        // Define o que ele procura (Mesas Infantis)
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ThingDef.Named("PE_InfantDesk"));

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            // Validações básicas
            if (!t.Spawned || t.IsForbidden(pawn) || !pawn.CanReserve(t)) return false;

            // Verifica se o aluno está matriculado em alguma turma
            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(pawn));
            if (turma == null) return false;

            // Verifica o horário
            int hour = GenLocalDate.HourOfDay(pawn.Map);
            if (hour < turma.startHour || hour >= (turma.startHour + turma.duration)) return false;
            
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(pawn));
            // Cria o trabalho de ir para a aula
            return JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("PE_AttendClass"), t, turma?.teacher);
        }
    }
}