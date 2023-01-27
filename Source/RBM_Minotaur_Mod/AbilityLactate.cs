using RBM_Minotaur;
using RimWorld;
using System.Linq;
using System;
using Unity.Jobs;
using Verse;
using Verse.AI;
using UnityEngine;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RBM_Minotaur
{
    // Spawns a stack of an item on the Pawn's location
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
            thing.stackCount = MinotaurSettings.lactateMilkAmount;

            GenSpawn.Spawn(thing, this.parent.pawn.Position, this.parent.pawn.Map);
        }
        public override bool GizmoDisabled(out string reason)
        {
            if (this.parent.pawn.gender == Gender.Male && !MinotaurSettings.milkableMales)
            {
                reason = "Males Cannot Be Milked";
                return true;
            }

            if (this.parent.pawn.gender == Gender.Female && !MinotaurSettings.milkableFemales)
            {
                reason = "Females Cannot Be Milked";
                return true;
            }

            reason = null;
            return false;
        }
    }

    public class CompProperties_AbilitySpawnStack : CompProperties_AbilityEffect
    {
        public CompProperties_AbilitySpawnStack()
        {
            this.compClass = typeof(CompAbilityEffect_SpawnStack);
        }

        public ThingDef spawnedThingDef;

    }
}