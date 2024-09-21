using HarmonyLib;
using RimWorld;
using System;
using Verse;


namespace Core_Mod
{
    [StaticConstructorOnStartup]
    public static class Core_Mod
    {
        static Core_Mod()
        {
            Log.Message("Core_Mod Static class loaded");

            Harmony harmony = new Harmony("rimworld.mod.rooboid.minotaur");
            harmony.PatchAll();
        }
    }


    [HarmonyPatch]
    public static class Lovin_MTB_Patch
    {

        [HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.GetLovinMtbHours))]
        [HarmonyPostfix]
        public static void GetLovinMtbHours_Postfix(ref float __result, Pawn pawn)
        {
            if (pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))
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
