using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class JobDriver_TeachClass : JobDriver
    {
        private const TargetIndex SpotIndex = TargetIndex.A; 

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true; 
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(SpotIndex, PathEndMode.InteractionCell);

            Toil teach = ToilMaker.MakeToil("Teach");
            teach.defaultCompleteMode = ToilCompleteMode.Never;
            teach.tickAction = () =>
            {
                if (Find.TickManager.TicksGame % 200 == 0)
                {
                    pawn.skills.Learn(SkillDefOf.Social, 10f);
                }
                if (!IsClassInSession(pawn)) this.EndJobWith(JobCondition.Succeeded);
            };
            yield return teach;
        }

        private bool IsClassInSession(Pawn p)
        {
            var turma = SchoolManager.Current.classes.Find(c => c.teacher == p);
            if (turma == null) return false;
            int hour = GenLocalDate.HourOfDay(p.Map);
            return hour >= turma.startHour && hour < (turma.startHour + turma.duration);
        }
    }
}