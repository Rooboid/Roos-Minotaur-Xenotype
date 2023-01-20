using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;


[HarmonyPatch]
public static class Lovin_Minotaur
{
    [HarmonyPatch(typeof(JobDriver_Lovin), "MakeNewToils")]
    [HarmonyPostfix]
    public static void MakeNewToils_Postfix(ref JobDriver_Lovin __instance, ref Verse.AI.TargetIndex ___PartnerInd)
    {
        Pawn Partner = (Pawn)((Thing)__instance.job.GetTarget(___PartnerInd));
        Log.Message("Patched Lovin' between " + __instance.pawn.Name + " and " + Partner.Name);
        
        if (Partner.genes.HasGene(RBM_GeneDefOf.RBM_Herculean))
        {
            __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(RBM_ThoughtDefOf.RBM_Crushed);
        }
    }
}

