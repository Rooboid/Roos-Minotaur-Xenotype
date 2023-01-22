using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;




namespace RBM_Minotaur
{
    [StaticConstructorOnStartup]
    public static class RBM_Minotaur
    {
        static RBM_Minotaur()
        {
            Log.Message("RBM_Minotaur Static class loaded");

            Harmony harmony = new Harmony("rimworld.mod.rooboid.minotaur");
            harmony.PatchAll();
        }
    }


    [HarmonyPatch]
    public static class Lovin_MTB_Patch
    {

        [HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.GetLovinMtbHours))]
        [HarmonyPostfix]
        public static void GetLovinMtbHours_Postfix(ref float __result, Pawn pawn, Pawn partner)
        {
            if (pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat) || partner.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))
            {
                Log.Message("Lovin MTB Adjusted. Was: " + (__result).ToString());
                __result = __result / 4;
                Log.Message("Is now: " + (__result).ToString());
            }
            else
            {
                Log.Message("Lovin MTB not adjusted. Not in heat.");
            }
        }
    }

    public class Gene_RBM_EstrousCycle : Gene
    {
        public override void Tick()
        {
            base.Tick();
            if (this.pawn.IsHashIntervalTick(1440))
            {
                if(!this.pawn.Spawned)
                {
                    return;
                }

                if ((GenLocalDate.Season(this.pawn.Tile) == Season.Spring) && !this.pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))
                {
                    this.pawn.health.AddHediff(RBM_HediffDefOf.EstrousHeat);
                    Log.Message(this.pawn.Name + " is in heat.");
                }
                else if (!(GenLocalDate.Season(this.pawn.Tile) == Season.Spring) && this.pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))
                {
                    Hediff HeatHediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(RBM_HediffDefOf.EstrousHeat);
                    this.pawn.health.RemoveHediff(HeatHediff);
                    Log.Message(this.pawn.Name + " is no longer in heat.");
                }
            }
        }
    }
}





