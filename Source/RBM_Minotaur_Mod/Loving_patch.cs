﻿using HarmonyLib;
using RimWorld;
using Verse;


[HarmonyPatch]
public static class Lovin_Minotaur
{
    [HarmonyPatch(typeof(JobDriver_Lovin), "MakeNewToils")]
    [HarmonyPostfix]
    public static void MakeNewToils_Postfix(ref JobDriver_Lovin __instance, ref Verse.AI.TargetIndex ___PartnerInd) //Adds the 'crushed' hediff to pawns that have lovin' with Herculean pawns
    {
        Pawn Partner = (Pawn)((Thing)__instance.job.GetTarget(___PartnerInd));
        Log.Message("Patched Lovin' between " + __instance.pawn.Name + " and " + Partner.Name);
        
        if (Partner.genes.HasGene(RBM_GeneDefOf.RBM_Herculean))
        {
            __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(RBM_ThoughtDefOf.RBM_Crushed);
        }
    }
}
