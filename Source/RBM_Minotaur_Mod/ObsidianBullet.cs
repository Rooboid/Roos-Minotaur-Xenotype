using RBM_Minotaur_Mod;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RBM_Minotaur
{

    [StaticConstructorOnStartup]
    public class Projectile_ObsidianBullet : Projectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            Map map = this.launcher.Map;
            base.Impact(hitThing);
            IntVec3 position = base.Position;
            ThingDef def = this.def;
            float explosionRadius = this.def.projectile.explosionRadius;
            GenExplosion.DoExplosion(position, map, explosionRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
            if (map != null)
            {
                List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    if (mapPawns[i].RaceProps.Humanlike)
                    {
                        if (position.InHorDistOf(mapPawns[i].Position, explosionRadius))
                        {
                            mapPawns[i].health.AddHediff(RBM_HediffDefOf.HeDiffTerrified);
                        }
                    }
                }
            }
        }
    }
}
