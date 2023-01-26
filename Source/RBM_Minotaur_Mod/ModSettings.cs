using UnityEngine;
using Verse;

namespace RBM_Minotaur
{
    public class MinotaurSettings : ModSettings
    {
        // The settings our mod has.
        public static bool milkableFemales = true;
        public static bool milkableMales = true;
        public static bool midasDespawnDontDestroy = false;
        public static bool midasRemovesCorpse = true;
        public static int midasGoldAmount = 10;

        // The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref milkableFemales, "milkableFemales");
            Scribe_Values.Look(ref milkableMales, "milkableMales");
            Scribe_Values.Look(ref midasDespawnDontDestroy, "midasDestroysOrDespawns");
            Scribe_Values.Look(ref midasRemovesCorpse, "midasRemovesCorpse");
            Scribe_Values.Look(ref midasGoldAmount, "midasGoldAmount");

            base.ExposeData();
        }
    }


    public class RBM_Minotaur_Mod : Mod
    {
        // A reference to our settings.
        public MinotaurSettings settings;

        // A mandatory constructor which resolves the reference to our settings.
        public RBM_Minotaur_Mod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<MinotaurSettings>();
        }

        // The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Lactation Gene Settings");
            listingStandard.CheckboxLabeled("Active for female pawns", ref MinotaurSettings.milkableFemales);
            listingStandard.CheckboxLabeled("Active for male pawns", ref MinotaurSettings.milkableMales);
            listingStandard.Label(" ");
            listingStandard.Label("Midaspear Weapon Settings");
            listingStandard.CheckboxLabeled("Removes corpses after killing", ref MinotaurSettings.midasRemovesCorpse, "More fun, but makes pawns killed by the Midaspear impossible to resurrect.");
            listingStandard.CheckboxLabeled("Despawns corpses rather than destroy", ref MinotaurSettings.midasDespawnDontDestroy, "Destroying a corpse is a volatile action, and can cause occasional errors. Despawning a pawn will may not trigger some on on-death events.");
            listingStandard.Label("Midaspear gold amount: " + MinotaurSettings.midasGoldAmount.ToString() );
            listingStandard.IntAdjuster(ref MinotaurSettings.midasGoldAmount, 5);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // Override SettingsCategory to show up in the list of settings.
        public override string SettingsCategory()
        {
            return "Roo's Minotaur Xenotype";
        }
    }
}
