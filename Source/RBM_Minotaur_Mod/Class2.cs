using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RBM_Minotaur_Mod

{
    // Token: 0x02000F8B RID: 3979
    public class CompProperties_AbilityPetrify : CompProperties_AbilityEffect
    {
        // Token: 0x06005D85 RID: 23941 RVA: 0x001FBF24 File Offset: 0x001FA124
        public CompProperties_AbilityPetrify()
        {
            this.compClass = typeof(CompAbilityEffect_Smokepop);
        }

        // Token: 0x040038F1 RID: 14577
        public float smokeRadius;
    }
}

{
    // Token: 0x02000F8C RID: 3980
    public class CompAbilityEffect_Petrify : CompAbilityEffect
    {
        // Token: 0x17001011 RID: 4113
        // (get) Token: 0x06005D86 RID: 23942 RVA: 0x001FBF3C File Offset: 0x001FA13C
        public new CompProperties_AbilityPetrify Props
        {
            get
            {
                return (CompProperties_AbilityPetrify)this.props;
            }
        }


        // Token: 0x06005D88 RID: 23944 RVA: 0x001FBF6C File Offset: 0x001FA16C
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.BlindSmoke), false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
        }

        // Token: 0x06005D89 RID: 23945 RVA: 0x001FBFF8 File Offset: 0x001FA1F8
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
        }
    }
}
}
