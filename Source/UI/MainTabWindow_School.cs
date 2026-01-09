using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace ProgressiveChildEducation
{
    public class MainTabWindow_School : MainTabWindow
    {
        private Vector2 scrollPosition;
        public override Vector2 RequestedTabSize => new Vector2(1024f, 600f);

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, inRect.y, 300, 40), "Gerenciamento Escolar");
            Text.Font = GameFont.Small;

            if (Widgets.ButtonText(new Rect(inRect.width - 150, inRect.y, 140, 30), "Nova Turma"))
            {
                SchoolManager.Current.classes.Add(new SchoolClass() { className = "Turma " + (SchoolManager.Current.classes.Count + 1) });
            }

            Rect listRect = new Rect(inRect.x, inRect.y + 50, inRect.width, inRect.height - 60);
            Rect viewRect = new Rect(0, 0, listRect.width - 16, SchoolManager.Current.classes.Count * 150f);

            Widgets.BeginScrollView(listRect, ref scrollPosition, viewRect);
            float y = 0;
            var classesToRemove = new List<SchoolClass>();

            foreach (var turma in SchoolManager.Current.classes)
            {
                DrawClassRow(new Rect(0, y, viewRect.width, 140), turma, classesToRemove);
                y += 150f;
            }

            foreach (var c in classesToRemove) SchoolManager.Current.classes.Remove(c);
            Widgets.EndScrollView();
        }

        private void DrawClassRow(Rect rect, SchoolClass turma, List<SchoolClass> toRemove)
        {
            Widgets.DrawMenuSection(rect);
            Rect col1 = new Rect(rect.x + 10, rect.y + 10, 200, rect.height - 20);
            turma.className = Widgets.TextField(new Rect(col1.x, col1.y, 180, 30), turma.className);
            Widgets.Label(new Rect(col1.x, col1.y + 40, 180, 25), $"Início: {turma.startHour}h");
            turma.startHour = (int)Widgets.HorizontalSlider(new Rect(col1.x, col1.y + 65, 180, 20), turma.startHour, 0, 23);
            Widgets.Label(new Rect(col1.x, col1.y + 90, 180, 25), $"Duração: {turma.duration}h");

            Rect col2 = new Rect(col1.xMax + 10, rect.y + 10, 200, rect.height - 20);
            Widgets.Label(new Rect(col2.x, col2.y, 200, 25), "Professor:");
            string teacherLabel = turma.teacher != null ? turma.teacher.LabelShort : "Nenhum";
            if (Widgets.ButtonText(new Rect(col2.x, col2.y + 30, 180, 30), teacherLabel))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (Pawn p in Find.CurrentMap.mapPawns.FreeColonists)
                {
                    if (p.ageTracker.Adult)
                        options.Add(new FloatMenuOption(p.LabelShort, () => turma.teacher = p));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }

            Rect col3 = new Rect(col2.xMax + 10, rect.y + 10, 400, rect.height - 20);
            Widgets.Label(new Rect(col3.x, col3.y, 400, 25), $"Alunos ({turma.students.Count}):");
            if (Widgets.ButtonText(new Rect(col3.x, col3.y + 30, 120, 30), "+ Adicionar"))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (Pawn p in Find.CurrentMap.mapPawns.FreeColonists)
                {
                    if (!p.ageTracker.Adult && !turma.students.Contains(p))
                        options.Add(new FloatMenuOption(p.LabelShort, () => turma.students.Add(p)));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }

            float studentY = col3.y + 70;
            foreach(var student in turma.students)
            {
                Rect sRect = new Rect(col3.x, studentY, 300, 24);
                SchoolUI_Utils.DrawEducationBar(sRect, student);
                studentY += 26;
                if(studentY > rect.yMax - 10) break;
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 30, rect.y, 25, 25), "X")) toRemove.Add(turma);
        }
    }
}