using RBM_Minotaur;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using Verse.Noise;

namespace RBM_Minotaur
{

    [StaticConstructorOnStartup]
    public class CompAbilityEffect_SpawnStack : CompAbilityEffect
    {
        public new CompProperties_AbilitySpawnStack Props
        {
            get
            {
                return (CompProperties_AbilitySpawnStack)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            base.Apply(target, dest);


            Thing thing = ThingMaker.MakeThing(Props.spawnedThingDef);
            thing.stackCount = Props.spawnedThingAmount;

            GenSpawn.Spawn(thing, this.parent.pawn.Position, this.parent.pawn.Map);
        }
    }

    public class CompProperties_AbilitySpawnStack : CompProperties_AbilityEffect
    {
        public CompProperties_AbilitySpawnStack()
        {
            this.compClass = typeof(CompAbilityEffect_SpawnStack);
        }

        public ThingDef spawnedThingDef;
        public int spawnedThingAmount = 25;
            
    }
    
}
