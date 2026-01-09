using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class Building_SchoolDesk : Building
    {
        // Configurações padrão
        public float learningFactor = 1.0f; // 1.0 = Normal, 1.5 = 50% mais rápido
        public SkillDef skillToTeach = null; // Se não for null, ensina essa skill (ex: Tiro)

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            
            // Definições baseadas no XML (defName)
            if (this.def.defName == "PE_LessonStone") learningFactor = 0.7f;
            if (this.def.defName == "PE_TouchscreenDesk") learningFactor = 1.8f;
            if (this.def.defName == "PE_HoloScreen") learningFactor = 2.0f;
            
            // Estações de Habilidade
            if (this.def.defName == "PE_CadaverDummy") { skillToTeach = SkillDefOf.Medicine; learningFactor = 1.2f; }
            if (this.def.defName == "PE_ShootingStation") { skillToTeach = SkillDefOf.Shooting; learningFactor = 1.2f; }
            if (this.def.defName == "PE_MeleeDummy") { skillToTeach = SkillDefOf.Melee; learningFactor = 1.2f; }
        }

        public bool IsUsable()
        {
            return this.Spawned && !this.IsBrokenDown() && !this.IsForbidden(Faction.OfPlayer);
        }
    }
}