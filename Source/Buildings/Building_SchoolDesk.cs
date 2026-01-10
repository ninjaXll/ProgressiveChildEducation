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
            
            // --- Mesas de Estudo ---
            if (this.def.defName == "PE_LessonStone") learningFactor = 0.7f;
            if (this.def.defName == "PE_SchoolDesk") learningFactor = 1.0f;
            if (this.def.defName == "PE_TouchscreenDesk") learningFactor = 1.5f;
            if (this.def.defName == "PE_HoloScreen") learningFactor = 2.0f;
            
            // --- Treino Físico ---
            if (this.def.defName == "PE_MeleeDummy") 
            { 
                skillToTeach = SkillDefOf.Melee; 
                learningFactor = 1.2f; 
            }

            // --- Treino de Tiro ---
            
            // Estação de Arco (Madeira - Básica)
            if (this.def.defName == "PE_ShootingStation") 
            { 
                skillToTeach = SkillDefOf.Shooting; 
                learningFactor = 1.2f; 
            }

            // Alvo Reativo (Spacer - Avançado)
            if (this.def.defName == "PE_ReactiveTarget")  
            { 
                skillToTeach = SkillDefOf.Shooting; 
                learningFactor = 1.8f; // Ensina quase o dobro da velocidade
            }

            // --- Medicina ---
            if (this.def.defName == "PE_CadaverDummy") 
            { 
                skillToTeach = SkillDefOf.Medicine; 
                learningFactor = 1.3f; 
            }
        }

        public bool IsUsable()
        {
            return this.Spawned && !this.IsBrokenDown() && !this.IsForbidden(Faction.OfPlayer);
        }
    }
}