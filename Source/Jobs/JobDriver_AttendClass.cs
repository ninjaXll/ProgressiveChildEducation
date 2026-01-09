using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class JobDriver_AttendClass : JobDriver
    {
        private const TargetIndex DeskIndex = TargetIndex.A;
        private const TargetIndex TeacherIndex = TargetIndex.B;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            // Reserva a mesa para ninguém sentar no colo dele
            return pawn.Reserve(job.GetTarget(DeskIndex), job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            // 1. Vai até a mesa
            yield return Toils_Goto.GotoThing(DeskIndex, PathEndMode.InteractionCell);

            // 2. Ação de Estudar (Loop contínuo)
            Toil study = ToilMaker.MakeToil("MakeNewToils");
            study.defaultCompleteMode = ToilCompleteMode.Never; // Só acaba quando o horário acabar
            study.tickAction = () =>
            {
                // A cada tick (fração de segundo), ganha XP
                // OBS: Ajuste o divisor (2500f) para balancear a velocidade
                Pawn actor = this.pawn;
                var hediff = actor.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("PE_EducationLevel"));
                
                if (hediff != null && hediff.Severity < 1.0f)
                {
                    // Ganho: 0.001 a cada 250 ticks (~4 segundos)
                    if (Find.TickManager.TicksGame % 250 == 0)
                    {
                        hediff.Severity += 0.0005f; 
                        
                        // Efeito visual de aprendizado (ícone subindo)
                        MoteMaker.ThrowText(actor.DrawPos, actor.Map, "+XP", UnityEngine.Color.cyan);
                    }
                }

                // Condição de Saída: Se a aula acabou
                // Precisamos checar o SchoolManager novamente ou confiar no WorkGiver para cancelar,
                // mas é mais seguro o JobDriver saber parar.
                if (!IsClassInSession(actor))
                {
                    this.EndJobWith(JobCondition.Succeeded);
                }
            };
            
            yield return study;
        }

        private bool IsClassInSession(Pawn p)
        {
            // Verifica se ainda está no horário da turma
            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(p));
            if (turma == null) return false;
            int hour = GenLocalDate.HourOfDay(p.Map);
            return hour >= turma.startHour && hour < (turma.startHour + turma.duration);
        }
    }
}