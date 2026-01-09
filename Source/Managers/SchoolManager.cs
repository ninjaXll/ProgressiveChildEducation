using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class SchoolClass : IExposable
    {
        public string className = "Nova Turma";
        public Pawn teacher;
        public List<Pawn> students = new List<Pawn>();
        public int startHour = 8;
        public int duration = 4;
        
        public void ExposeData()
        {
            Scribe_Values.Look(ref className, "className");
            Scribe_References.Look(ref teacher, "teacher");
            Scribe_Collections.Look(ref students, "students", LookMode.Reference);
            Scribe_Values.Look(ref startHour, "startHour", 8);
            Scribe_Values.Look(ref duration, "duration", 4);
        }
    }

    public class SchoolManager : GameComponent
    {
        public List<SchoolClass> classes = new List<SchoolClass>();

        public SchoolManager(Game game) {}

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref classes, "schoolClasses", LookMode.Deep);
            if (classes == null) classes = new List<SchoolClass>();
        }

        public static SchoolManager Current => Verse.Current.Game.GetComponent<SchoolManager>();
    }
}