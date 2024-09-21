using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Roos_Faun_Xenotype
{

    [HarmonyPatch(typeof(PawnRenderNode_Fur))]
    [HarmonyPatch("GraphicFor")]
    static class PawnRenderer_Prefix
    {
        static bool Prefix(Pawn pawn, ref Graphic __result)
        {
            if (!pawn.RaceProps.Humanlike)
            {
                return true;
            }

            if (pawn.genes.HasActiveGene(RBM_DefOf.RBM_UnguligradeLegs))
            {
                Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(pawn.story.furDef.GetFurBodyGraphicPath(pawn), ShaderDatabase.CutoutSkinOverlay, Vector2.one, pawn.story.HairColor, Color.white, null, pawn.story.bodyType.bodyNakedGraphicPath);
                __result = graphic;
                return false;
            }
            return true;
        }
    }
}
