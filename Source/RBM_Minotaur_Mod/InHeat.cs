using HarmonyLib;
using RimWorld;
using Verse;




namespace RBM_Minotaur
{
    [HarmonyPatch]
    public static class Lovin_MTB_Patch
    {

        [HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.GetLovinMtbHours))]
        [HarmonyPostfix]
        public static void GetLovinMtbHours_Postfix(ref float __result, Pawn pawn, Pawn partner)    //Adjusts Mean TIme Between Lovin' for pawns in heat
        {
            if (pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat) || partner.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat))
            {
                Log.Message("Lovin MTB Adjusted. Was: " + (__result).ToString());
                __result = __result / 4;
                Log.Message("Is now: " + (__result).ToString());
            }
        }
    }

    public class Gene_RBM_EstrousCycle : Gene
    {
        public override void Tick() //Adds the 'In Heat' hediff to pawns with the Estrous Cycle gene in ApriMay
        {
            base.Tick();

            //Return if the hash interval is incorrect (not enough time has passed)
            if (!this.pawn.IsHashIntervalTick(1440))
            {
                return;
            }

            //Return if the pawn is not spawned to stop 
            if(!this.pawn.Spawned)
            {
                return;
            }

            float latitude = Find.WorldGrid.LongLatOf(this.pawn.Map.Tile).y;
            int absTick = GenTicks.TicksAbs;
            bool isAprimay = GenDate.Quadrum(absTick, latitude) == Quadrum.Aprimay;
            bool pawnHasHeatHediff = this.pawn.health.hediffSet.HasHediff(RBM_HediffDefOf.EstrousHeat);

            //Add Hediff if it is may
            if (isAprimay && !pawnHasHeatHediff)
            {
                this.pawn.health.AddHediff(RBM_HediffDefOf.EstrousHeat);
                Log.Message(this.pawn.Name + " is in heat.");
            }
            //Remove Hediff if it is not may
            else if ( !isAprimay && pawnHasHeatHediff ) 
            {
                Hediff HeatHediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(RBM_HediffDefOf.EstrousHeat);
                this.pawn.health.RemoveHediff(HeatHediff);
                Log.Message(this.pawn.Name + " is no longer in heat.");
            }
            
        }
    }
}





