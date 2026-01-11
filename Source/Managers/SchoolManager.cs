using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class SchoolClass : IExposable
    {
        // Define valores padrão seguros
        public string className = "Nova Turma";
        public Pawn teacher;
        public List<Pawn> students = new List<Pawn>();
        public int startHour = 8;
        public int duration = 4;
        
        public void ExposeData()
        {
            // PROTEÇÃO 1: Valores padrão (defaultValue)
            // Se o save não tiver o nome (save antigo/novo mod), ele usa "Nova Turma" em vez de nulo.
            Scribe_Values.Look(ref className, "className", "Nova Turma");
            
            Scribe_References.Look(ref teacher, "teacher");
            
            // Salva a lista de referências
            Scribe_Collections.Look(ref students, "students", LookMode.Reference);
            
            Scribe_Values.Look(ref startHour, "startHour", 8);
            Scribe_Values.Look(ref duration, "duration", 4);

            // PROTEÇÃO 2: Garantia de Lista
            // Se a lista vier nula do save, recria uma vazia para não dar erro de "NullReference" depois.
            if (students == null) students = new List<Pawn>();
        }
    }

    public class SchoolManager : GameComponent
    {
        public List<SchoolClass> classes = new List<SchoolClass>();

        public SchoolManager(Game game) {}

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref classes, "schoolClasses", LookMode.Deep);
            
            // PROTEÇÃO 3: Garantia da Lista Principal
            if (classes == null) classes = new List<SchoolClass>();
        }

        // Acesso seguro ao componente
        public static SchoolManager Current => Verse.Current.Game?.GetComponent<SchoolManager>();
    }
}