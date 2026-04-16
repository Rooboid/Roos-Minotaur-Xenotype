using RimWorld;
using Verse;

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

        //Main 'See Red' ability method - terrifies pawns in an area and enrages the user.
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (See Red) Apply(LocalTargetInfo target, LocalTargetInfo dest)"); }
            Pawn pawn = this.parent.pawn;
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            float radius = MinotaurSettings.SeeRedFearRadius;

            base.Apply(target, dest);

            //pawn.health.AddHediff(RBM_DefOf.HeDiffSeeRed);
            GenExplosion.DoExplosion(
                center: target.Cell,
                map: map,
                radius: radius,
                damType: DamageDefOf.Smoke,
                instigator: pawn
            );
            RBM_Utils.TerrifyInArea(position, map, radius, pawn);
        }

        //Draws radius circle
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            if (MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (See Red) public override void DrawEffectPreview(LocalTargetInfo target)"); }
            GenDraw.DrawRadiusRing(target.Cell, MinotaurSettings.SeeRedFearRadius);
        }

        //Allows AI to use ability only if 35% to pain threshold
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            float? currentPain = this.parent?.pawn?.health?.hediffSet?.PainTotal;
            float allowedPain = this.parent.pawn.GetStatValue(StatDefOf.PainShockThreshold) * 0.35f;
            if (MinotaurSettings.debugMessages) { Log.Message("AI wants to use see red - pain is " + currentPain.ToString() + " / " + allowedPain.ToString()); }

            if (currentPain < allowedPain)
            {
                return false;
            }

            var cellsAround = GenRadial.RadialCellsAround(parent.pawn.Position, MinotaurSettings.SeeRedFearRadius - 2, true);
            foreach (var cell in cellsAround)
            {
                if (cell.GetFirstPawn(parent.pawn.Map)?.HostileTo(parent.pawn) == true) // if cell contains hostile pawn, return true
                {
                    //Log.Message("checked cell " + cell + " and found enemies - AI can use");
                    return true;

                }
                //Log.Message("checked cell " + cell + " No enemies.");
            }
            //Log.Message(parent.pawn.Name + " found no enemies - AI CANNOT use see red");
            return false;
        }
    }

    public class CompProperties_AbilityTerrify : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTerrify()
        {
            this.compClass = typeof(CompAbilityEffect_Terrify);
        }
    }
}
