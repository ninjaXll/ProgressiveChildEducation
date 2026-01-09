using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class JobDriver_AttendClass : JobDriver
    {
        private const TargetIndex DeskIndex = TargetIndex.A;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(DeskIndex), job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(DeskIndex, PathEndMode.InteractionCell);

            Toil study = ToilMaker.MakeToil("Study");
            study.defaultCompleteMode = ToilCompleteMode.Never;
            study.tickAction = () =>
            {
                Pawn actor = this.pawn;
                Building_SchoolDesk desk = this.job.GetTarget(DeskIndex).Thing as Building_SchoolDesk;

                // A cada ~4 segundos
                if (Find.TickManager.TicksGame % 250 == 0)
                {
                    // 1. Ganho de Educação (Geral)
                    var hediff = actor.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false));
                    if (hediff != null && hediff.Severity < 1.0f)
                    {
                        float factor = desk != null ? desk.learningFactor : 1.0f;
                        hediff.Severity += 0.0005f * factor;
                        
                        // Texto visual apenas ocasionalmente
                        if (Find.TickManager.TicksGame % 1000 == 0)
                             MoteMaker.ThrowText(actor.DrawPos, actor.Map, $"+XP ({factor:P0})", UnityEngine.Color.cyan);
                    }

                    // 2. Ganho de Habilidade Específica (Se a mesa for especial)
                    if (desk != null && desk.skillToTeach != null)
                    {
                        actor.skills.Learn(desk.skillToTeach, 15f); // Ganha XP na skill
                    }
                }

                if (!IsClassInSession(actor)) this.EndJobWith(JobCondition.Succeeded);
            };
            yield return study;
        }

        private bool IsClassInSession(Pawn p)
        {
            var turma = SchoolManager.Current.classes.Find(c => c.students.Contains(p));
            if (turma == null) return false;
            int hour = GenLocalDate.HourOfDay(p.Map);
            return hour >= turma.startHour && hour < (turma.startHour + turma.duration);
        }
    }
}