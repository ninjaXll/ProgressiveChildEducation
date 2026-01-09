using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ProgressiveChildEducation
{
    public class Building_SchoolDesk : Building
    {
        // Se quisermos que a mesa tenha um dono específico no futuro,
        // podemos usar o CompAssignableToPawn. Por enquanto, deixamos
        // a lógica base para garantir que ela seja reconhecida como estrutura.
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }

        // Função útil para a UI ou Jobs saberem se a mesa está funcional
        public bool IsUsable()
        {
            return this.Spawned && !this.IsBrokenDown() && !this.IsForbidden(Faction.OfPlayer);
        }

        // Opcional: Desenhar algo extra quando selecionado (ex: raio conectando ao professor)
        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            
            // Exemplo: Se tivermos a referência da Turma salva aqui, 
            // poderíamos desenhar uma linha até o professor.
        }
    }
}