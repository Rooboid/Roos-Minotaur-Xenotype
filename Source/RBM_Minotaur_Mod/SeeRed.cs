using Verse;
using RimWorld;
using System.Collections.Generic;


namespace RBM_Minotaur_Mod
{
    [StaticConstructorOnStartup]
    public class CompAbilityEffect_Terrify : CompAbilityEffect
    {

        public new CompProperties_AbilityTerrify Props
        {
            get
            {
                return (CompProperties_AbilityTerrify)this.props;
            }
        }

        public bool ShouldHaveInspectString
        {
            get
            {
                return ModsConfig.BiotechActive;
            }
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            base.Apply(target, dest);
            GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.terrorRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
            if (pawn.Map != null)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    if (mapPawns[i].RaceProps.Humanlike && mapPawns[i] != pawn)
                    {
                        if (pawn.Position.InHorDistOf(mapPawns[i].Position, this.Props.terrorRadius))
                        {
                            mapPawns[i].health.AddHediff(RBM_HediffDefOf.HeDiffTerrified);
                        }
                    }
                }
            }
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
    }
}
