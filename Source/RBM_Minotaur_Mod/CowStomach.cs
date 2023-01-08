using HarmonyLib;
using RimWorld;
using Verse;
using System;


[HarmonyPatch]
public static class WillEat_Minotaur
{
    

    [HarmonyPatch(typeof(FoodUtility), nameof(FoodUtility.WillEat_NewTemp), new Type[] { typeof(Pawn), typeof(ThingDef), typeof(Pawn), typeof(bool), typeof(bool) })]
    [HarmonyPostfix]
    public static bool WillEat_NewTemp_Postfix(bool __result, Pawn p, ThingDef food, Pawn getter, bool careIfNotAcceptableForTitle, bool allowVenerated)
    {
        Log.Message("We checked edible. Pawn: " + p.Name + " Food: " + food.defName);
        if (food == ThingDefOf.Hay)
        {
            if (p.genes.HasGene(RBM_GeneDefOf.RBM_RuminantStomach))
            {
                return true;
            }
            return false;
            
        }
        return __result;
    }

    [HarmonyPatch( typeof(Thing), nameof(Thing.Ingested) )]
    [HarmonyPostfix]
    public static float Ingested_Postfix(float __result, Pawn ingester, Thing __instance )
    {
        if( ingester != null && ingester.RaceProps.Humanlike && __instance.def.defName == "Hay" && !(ingester.genes.HasGene(RBM_GeneDefOf.RBM_RuminantStomach)))
        {
            Log.Message("Pawn " + ingester.Name + " ate hay but doesnt have a Ruminant Stomach. No nutrition was gained.");
            return 0;
        }
        return __result;
    }
}

