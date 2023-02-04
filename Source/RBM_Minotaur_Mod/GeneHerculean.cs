using HarmonyLib;
using RimWorld;
using Verse;
using System;


[HarmonyPatch]
public static class Herculean_Patches
{

    //Postfix to prevent non-Herculean pawns from equiping Herculean Weapons or Equipment

    [HarmonyPatch(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal} )]
    [HarmonyPostfix]
    public static bool CanEquip_Postfix(bool __result, Thing thing, Pawn pawn, ref string cantReason, bool checkBonded = true)
    {
        if (RBM_Minotaur.MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (Equip Postfix) public static bool CanEquip_Postfix(bool __result, Thing thing, Pawn pawn, ref string cantReason, bool checkBonded = true)"); }
        // Check Weapon
        if (thing.def.weaponClasses != null)
        {
            if (!thing.def.weaponClasses.Contains(RBM_DefOf.RBM_HerculeanClass))
            {
                return __result;
            }
        }

        // Check Apparel
        if (thing.def.apparel?.tags != null) 
        {
            if (!thing.def.apparel.tags.Contains("HerculeanApparel"))
            {
                return __result;
            }
        }

        // Check Pawn
        if (pawn.genes != null)
        {
            if (pawn.genes.HasGene(RBM_DefOf.RBM_Herculean))
            {
                return true;
            }
        }
        cantReason = "Pawn is not Herculean";
        return false;
        
    }


    //Adds the 'crushed' hediff to pawns that have lovin' with Herculean pawns

    [HarmonyPatch(typeof(JobDriver_Lovin), "MakeNewToils")]
    [HarmonyPostfix]
    public static void MakeNewToils_Postfix(ref JobDriver_Lovin __instance, ref Verse.AI.TargetIndex ___PartnerInd)
    {
        if (RBM_Minotaur.MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (Equip Postfix (crushed)) public static void MakeNewToils_Postfix(ref JobDriver_Lovin __instance, ref Verse.AI.TargetIndex ___PartnerInd)"); }
        Pawn Partner = (Pawn)((Thing)__instance.job.GetTarget(___PartnerInd));
        if (Partner.genes.HasGene(RBM_DefOf.RBM_Herculean))
        {
            if (__instance.pawn.story?.traits?.HasTrait(TraitDefOf.Masochist) == true)  //Give a positive version to masochists
            {
                __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(RBM_DefOf.RBM_CrushedMasochist);
            }
            else
            {
                __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(RBM_DefOf.RBM_Crushed);
            }

        }
    }
}
