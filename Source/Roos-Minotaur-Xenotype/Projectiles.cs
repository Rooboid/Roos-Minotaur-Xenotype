using RimWorld;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace RBM_Minotaur
{
    // Bullet type that applies the terrified state in an area - used by Taurail Gun
    public class Projectile_ObsidianBullet : Projectile_Explosive
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (Projectile) protected override void Impact(Thing hitThing, bool blockedByShield = false)"); }
            Map map = base.Map;
            base.Impact(hitThing);
            IntVec3 position = base.Position;
            float explosionRadius = MinotaurSettings.TaurailFearRadius;
            DamageDef damageDef = this.def.projectile.damageDef;
            int damageAmount = this.DamageAmount;
            float armorPenetration = this.ArmorPenetration;

            GenExplosion.DoExplosion(
                center: position,
                map: map,
                radius: explosionRadius,
                damType: damageDef,
                instigator: null,
                damAmount: damageAmount,
                armorPenetration: armorPenetration
            );
            RBM_Utils.TerrifyInArea(position, map, explosionRadius);
        }
    }
}
