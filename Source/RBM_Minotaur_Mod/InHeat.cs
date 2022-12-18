using System;
using Verse;
using Verse.AI;
using HarmonyLib;
using RimWorld;



namespace RBM_Minotaur_Mod
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


    [DefOf]
    public static class RBM_HediffDefOf
    {
        public static HediffDef EstrousHeat;
    }

    [DefOf]
    public static class RBM_GeneDefOf
    {
        public static GeneDef RBM_EstrousCycle;
    }

    public class ThinkNode_ConditionalPawnInHeatSeason : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)                                //Thinknode is satifified if the pawn:
        {
            bool geneFlag = false, seasonFlag = false, hediffFlag = true;
            if (pawn.RaceProps.Humanlike == true)
            {
                if (pawn.genes.HasGene(RBM_GeneDefOf.RBM_EstrousCycle) == true)
                {
                    geneFlag = true;
                    if ((GenLocalDate.Season(pawn.Tile) == Season.Spring))
                    {
                        seasonFlag = true;
                        if (!(pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat)))
                        {
                            hediffFlag = false;
                        }
                    }
                    
                }

            Log.Message("Humanlike Pawn Checked. Has gene: " + geneFlag.ToString() + " is correct season: " + seasonFlag.ToString() + " is already in heat: " + hediffFlag.ToString());
            }
            return (                                                                 
                pawn.RaceProps.Humanlike ||                                        //is not human
                pawn.genes.HasGene(RBM_GeneDefOf.RBM_EstrousCycle) ||              //Does not have the Estrous Cycle Gene
                (GenLocalDate.Season(pawn.Tile) == Season.Spring) ||               //is not on a tile where the season is currently Spring
                !(pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))      //is already in heat
            );
        }

    }

    public class JobGiver_PawnInHeatSeason : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Log.Message(pawn.Name + "is in heat.");
            pawn.health.AddHediff(RBM_HediffDefOf.EstrousHeat);
            return (Job)null;
        }
    }

}

