using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RBM_Minotaur
{
    public class MinotaurSettings : ModSettings
    {
        // The three settings our mod has.
        public bool settings_milkableFemales = true;
        public bool settings_milkableMales = true;


        // The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref settings_milkableFemales, "milkableFemales");
            Scribe_Values.Look(ref settings_milkableMales, "milkableMales");

            base.ExposeData();
        }
    }

    public class RBM_Minotaur_Mod : Mod
    {
        // A reference to our settings.
        MinotaurSettings settings;


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
            listingStandard.Label("Gene Settings: ");
            listingStandard.CheckboxLabeled("Lactation Gene is active for Female Pawns", ref settings.settings_milkableFemales, "Requires Restart");
            listingStandard.CheckboxLabeled("Lactation Gene is active for Male Pawns", ref settings.settings_milkableMales, "Requires Restart");
            
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // Override SettingsCategory to show up in the list of settings.
        // Using .Translate() is optional, but does allow for localisation.

        public override string SettingsCategory()
        {
            return "Roo's Minotaur Mod";
        }
    }
}
