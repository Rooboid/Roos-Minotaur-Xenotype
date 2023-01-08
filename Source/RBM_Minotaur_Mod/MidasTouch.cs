using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace RBM_Minotaur_Mod
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
        public int filthCount = 4;
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

            if (!base.Pawn.Spawned)
            {
                return;
            }

            if (this.Props.destroyItems)
            {
                base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
                base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
            }

            Thing thing = ThingMaker.MakeThing(ThingDefOf.Gold, null);
            thing.stackCount = Props.goldAmount;
            GenSpawn.Spawn(thing, base.Pawn.Position, base.Pawn.Map, WipeMode.VanishOrMoveAside);
            //thing.Position = base.Pawn.Corpse.Position;

            if (this.Props.mote != null || this.Props.fleck != null)
            {
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
            //Pawn.Ideo?.Notify_MemberDied(Pawn);
            //Pawn.Ideo?.Notify_MemberCorpseDestroyed(Pawn);

            if (this.Props.destroyBody)
            {

                base.Pawn.Corpse.DeSpawn(DestroyMode.Vanish);

                Pawn.Corpse.InnerPawn = null;
                if (Pawn.ownership != null)
                {
                    Pawn.ownership.UnclaimAll();
                }

                if (Pawn.equipment != null)
                {
                    Pawn.equipment.DestroyAllEquipment();
                }

                Pawn.inventory.DestroyAll();
                if (Pawn.apparel != null)
                {
                    Pawn.apparel.DestroyAll();
                }
            }
            //Pawn.Ideo?.RecacheColonistBelieverCount();
            //Pawn.Ideo?.RecachePrecepts();
            Pawn.Ideo?.Notify_MemberCorpseDestroyed(Pawn);

        }

    }
}
