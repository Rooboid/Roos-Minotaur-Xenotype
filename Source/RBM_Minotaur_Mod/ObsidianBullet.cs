using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RBM_Minotaur
{

    [StaticConstructorOnStartup]
    public class Projectile_ObsidianBullet : Projectile_Explosive
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {

            Map map = base.Map;
            base.Impact(hitThing);
            IntVec3 position = base.Position;
            float explosionRadius = this.def.projectile.explosionRadius;
            DamageDef damageDef = this.def.projectile.damageDef;
            int damageAmount = this.DamageAmount;
            float armorPenetration = this.ArmorPenetration;

            GenExplosion.DoExplosion(position, map, explosionRadius, damageDef, null, damageAmount, armorPenetration, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
            if (map != null)
            {
                List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    if (mapPawns[i].RaceProps.Humanlike && mapPawns[i].Downed == false && mapPawns[i].InMentalState == false)
                    {
                        if (position.InHorDistOf(mapPawns[i].Position, explosionRadius))
                        {
                            LocalTargetInfo t = new LocalTargetInfo(RBM_Utils.genFleeTile(mapPawns[i].Position, position, 10));
                            Job job = new Job(JobDefOf.FleeAndCower, t);
                            mapPawns[i].mindState.mentalStateHandler.TryStartMentalState(RBM_MentalStateDefOf.RBM_TerrifiedFlee, "scared by something nearby", true, false, null, true);
                            mapPawns[i].jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                    }
                }
            }
        }
    }
}
