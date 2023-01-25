using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RBM_Minotaur
{
    public class CompAbilityEffect_Terrify : CompAbilityEffect
    {

        public new CompProperties_AbilityTerrify Props
        {
            get
            {
                return (CompProperties_AbilityTerrify)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            float radius = this.Props.terrorRadius;

            base.Apply(target, dest);

            GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.terrorRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
            RBM_Utils.terrifyInArea(position, map, radius);
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(target.Cell, this.Props.terrorRadius);
        }
    }

    public class CompProperties_AbilityTerrify : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTerrify()
        {
            this.compClass = typeof(CompAbilityEffect_Terrify);
        }
        public float terrorRadius;
        public int fleeDistance;
    }
}
