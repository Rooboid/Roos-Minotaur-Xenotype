using HarmonyLib;
using RimWorld;
using Verse;
using System;


[HarmonyPatch]
public static class WillEat_Minotaur
{
    // Allows pawns with the Ruminant Stomach Gene to eat Hay
    [HarmonyPatch(typeof(FoodUtility), nameof(FoodUtility.WillEat_NewTemp), new Type[] { typeof(Pawn), typeof(ThingDef), typeof(Pawn), typeof(bool), typeof(bool) })]
    [HarmonyPostfix]
    public static bool WillEat_NewTemp_Postfix(bool __result, Pawn p, ThingDef food, Pawn getter, bool careIfNotAcceptableForTitle, bool allowVenerated)    
    {
        if (food == ThingDefOf.Hay)
        {
            if (p.genes.HasGene(RBM_DefOf.RBM_RuminantStomach))
            {
                return true;
            }
            return false;
            
        }
        return __result;
    }
    
    // Prevents pawns without the ruminant stomach gene from gaining nutrition from hay.
    [HarmonyPatch( typeof(Thing), nameof(Thing.Ingested) )]
    [HarmonyPostfix]
    public static float Ingested_Postfix(float __result, Pawn ingester, Thing __instance)  
    {
        if( ingester != null && ingester.RaceProps.Humanlike && __instance.def.defName == "Hay" && !(ingester.genes.HasGene(RBM_DefOf.RBM_RuminantStomach)))
        {
            Log.Warning("Pawn " + ingester.Name + " ate hay but doesnt have a Ruminant Stomach. No nutrition was gained - but this shouldn't happen without the pawn being forced.");
            return 0;
        }
        return __result;
    }
}

