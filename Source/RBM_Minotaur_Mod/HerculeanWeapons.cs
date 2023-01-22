using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Security.Cryptography;


[HarmonyPatch]
public static class CanEquip_Minotaur
{
    [HarmonyPatch(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal} )]
    [HarmonyPostfix]
    public static bool CanEquip_Postfix(bool __result, Thing thing, Pawn pawn, ref string cantReason, bool checkBonded = true)
    {
        if (thing.def.weaponClasses != null)
        {
            if (!thing.def.weaponClasses.Contains(RBM_WeaponClassDefOf.RBM_HerculeanClass))
            {
                Log.Message("Weapon is not Herculean");
                return __result;
            }
        }

        if (thing.def.apparel?.tags != null) 
        {
            if (!thing.def.apparel.tags.Contains("HerculeanApparel"))
            {
                Log.Message("Apparel is not Herculean");
                return __result;
            }
        }

        if (pawn.genes != null)
        {
            if (pawn.genes.HasGene(RBM_GeneDefOf.RBM_Herculean))
            {
                Log.Message("Pawn is Herculean");
                //cantReason = "Pawn is Herculean";
                return true;
            }
        }

        Log.Message("Pawn is not Herculean");
        cantReason = "Pawn is not Herculean";
        return false;
        
    }
}

