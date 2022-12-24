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

    public class JobGiver_PawnInHeatSeason : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.RaceProps.Humanlike && pawn.genes.HasGene(RBM_GeneDefOf.RBM_EstrousCycle) && pawn.ageTracker.Adult)
            {
                if ((GenLocalDate.Season(pawn.Tile) == Season.Spring) && !(pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat)))
                {
                    pawn.health.AddHediff(RBM_HediffDefOf.EstrousHeat);
                    Log.Message(pawn.Name + " is in heat.");
                }
                else if (!(GenLocalDate.Season(pawn.Tile) == Season.Spring) && (pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat)))
                {
                    Hediff HeatHediff = pawn.health.hediffSet.GetFirstHediffOfDef(RBM_HediffDefOf.EstrousHeat);
                    pawn.health.RemoveHediff(HeatHediff);
                    Log.Message(pawn.Name + " is no longer in heat.");
                }
            }
            return null;

        }
    }
}





