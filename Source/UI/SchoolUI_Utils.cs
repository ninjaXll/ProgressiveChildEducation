using UnityEngine;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    [StaticConstructorOnStartup]
    public static class SchoolUI_Utils
    {
        public static readonly Texture2D BarTex_Infant = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.7f, 1.0f));
        public static readonly Texture2D BarTex_Basic = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.2f));
        public static readonly Texture2D BarTex_Advanced = SolidColorMaterials.NewSolidColorTexture(new Color(1.0f, 0.8f, 0.2f));

        public static void DrawEducationBar(Rect rect, Pawn student)
        {
            if (student == null) return;
            var hediff = student.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("PE_EducationLevel", false));
            float progress = hediff != null ? hediff.Severity : 0f;

            Texture2D barTexture = BarTex_Infant;
            string stageLabel = "Iniciante";

            if (progress >= 0.7f) { barTexture = BarTex_Advanced; stageLabel = "Avançado"; }
            else if (progress >= 0.3f) { barTexture = BarTex_Basic; stageLabel = "Básico"; }

            Widgets.DrawBoxSolid(rect, new Color(0.15f, 0.15f, 0.15f));
            Rect barRect = new Rect(rect.x, rect.y, rect.width * progress, rect.height);
            GUI.DrawTexture(barRect, barTexture);
            Widgets.DrawBox(rect);
            
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect, $"{stageLabel} ({(progress * 100).ToString("F0")}%)");
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }
    }
}