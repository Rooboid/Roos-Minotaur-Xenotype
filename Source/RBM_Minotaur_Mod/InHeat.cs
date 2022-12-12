using System;
using Verse;
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

    /*
     
    public class CompInitialHediff : ThingComp
    {
        public override void CompTickRare()
        {
            base.CompTickRare();
        }
    }
    */
}
