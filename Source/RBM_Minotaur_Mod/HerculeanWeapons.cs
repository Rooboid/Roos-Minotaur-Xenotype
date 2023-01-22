using HarmonyLib;
using RimWorld;
using Verse;
using System;


[HarmonyPatch]
public static class CanEquip_Minotaur
{
    [HarmonyPatch(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal} )]
    [HarmonyPostfix]
    public static bool CanEquip_Postfix(bool __result, Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
    {
        cantReason = null;

        if( (thing.def.weaponClasses != null || !thing.def.weaponClasses.Contains(RBM_WeaponClassDefOf.RBM_HerculeanClass) ) && (thing.def.apparel.tags != null || !thing.def.apparel.tags.Contains("HerculeanApparel")))
        {
            return __result;
        }

        if (pawn.story?.traits != null && !pawn.genes.HasGene(RBM_GeneDefOf.RBM_Herculean))
        {
            cantReason = "Pawn is not Herculean";
            return false;
        }
        return true;
    }
}

