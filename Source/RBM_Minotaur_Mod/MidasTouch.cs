using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace RBM_Minotaur
{
    //Midas Touch Death Effect
    public class HediffCompProperties_MidasTouch : HediffCompProperties
    {
        public HediffCompProperties_MidasTouch()
        {
            this.compClass = typeof(HediffComp_MidasTouch);
        }

        public FleckDef fleck;
        public ThingDef mote;
        public int moteCount = 3;
        public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);
        public ThingDef filth;
        public int filthCount = 10;
        public SoundDef sound;
        public int goldAmount = 20;
        public bool destroyBody = true;
        public bool destroyItems = true;
    }

    public class HediffComp_MidasTouch : HediffComp
    {

        public HediffCompProperties_MidasTouch Props
        {
            get
            {
                return (HediffCompProperties_MidasTouch)this.props;
            }
        }

        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();

            if (!base.Pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.MidasTouch) || base.Pawn.health.hediffSet.GetFirstHediffOfDef(RBM_HediffDefOf.MidasTouch).Severity < 0.5)
            {
                return;
            }

            if (!base.Pawn.Spawned)
            {
                return;
            }

            if (this.Props.destroyItems)
            {
                base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
                base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
            }

            if (this.Props.mote == null && this.Props.fleck == null)
            {
                return;
            }

            Vector3 drawPos = base.Pawn.DrawPos;
            for (int i = 0; i < this.Props.moteCount; i++)
            {
                Vector2 vector = Rand.InsideUnitCircle * this.Props.moteOffsetRange.RandomInRange * (float)Rand.Sign;
                Vector3 loc = new Vector3(drawPos.x + vector.x, drawPos.y, drawPos.z + vector.y);
                if (this.Props.mote != null)
                {
                    MoteMaker.MakeStaticMote(loc, base.Pawn.Map, this.Props.mote, 1f, false);
                }
                else
                {
                    FleckMaker.Static(loc, base.Pawn.Map, this.Props.fleck, 1f);
                }
            }
            
            if (this.Props.filth != null)
            {
                FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, this.Props.filth, this.Props.filthCount, FilthSourceFlags.None, true);
            }
            if (this.Props.sound != null)
            {
                this.Props.sound.PlayOneShot(SoundInfo.InMap(base.Pawn, MaintenanceType.None));
            }
        }

        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();

            if (!base.Pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.MidasTouch) || base.Pawn.health.hediffSet.GetFirstHediffOfDef(RBM_HediffDefOf.MidasTouch).Severity < 0.5)
            {
                return;
            }

            IntVec3 pawnPos = base.Pawn.Corpse.Position;
            Map map = this.parent.pawn.Corpse.Map;

            if (this.Props.destroyBody)
            {
                base.Pawn.Corpse.DeSpawn(DestroyMode.Vanish);
            }

            Thing thing = ThingMaker.MakeThing(ThingDefOf.Gold, null);
            thing.stackCount = Props.goldAmount;

            GenSpawn.Spawn(thing, pawnPos, map, WipeMode.VanishOrMoveAside);
        }
    }
}

