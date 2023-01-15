using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RBM_Minotaur
{
    internal class HerculeanWeapons
    {
        [HarmonyPatch(typeof(bool), nameof(EquipmentUtility.CanEquip))]
        [HarmonyPostfix]
        public static bool CanEquip_Postfix(bool __result, Thing thing, Pawn pawn)
        {
            if(__result == true)
            {
                return true;
            }

            if(thing.def.weaponClasses != null && !thing.def.weaponClasses.Contains(RBM_WeaponClassDefOf.RBME_HerculeanClass))
            {
                Log.Message("Can't equip! Its not a minotaur weapon.");
                return false;
            }

            if (pawn.story?.traits != null && pawn.genes.HasGene(RBM_GeneDefOf.RBM_Herculean))
            {
                Log.Message("I can equip this! Watch and learn bucko.");
                return true;
            }
            Log.Message("Can't equip! I'm not Herculean...");
            return false;
        }
    }
}
