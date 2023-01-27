﻿using UnityEngine;
using Verse;
using System;

namespace RBM_Minotaur
{
    public class MinotaurSettings : ModSettings
    {
        public const bool milkableFemalesDefault = true;
        public const bool milkableMalesDefault = true;
        public const bool midasDespawnDontDestroyDefault = false;
        public const bool midasRemovesCorpseDefault = true;
        public const int midasGoldAmountDefault = 10;
        public const float TaurailFearRadiusDefault = 5.5f;
        public const float SeeRedFearRadiusDefault = 8.5f;
        public const float SeeRedFleeRadiusDefault = 10.5f;
        public const int SeeRedFearDurationDefault = 120;
        public const int lactateMilkAmountDefault = 25;


        // The settings our mod has.
        public static bool milkableFemales = milkableFemalesDefault;
        public static bool milkableMales = milkableMalesDefault;
        public static bool midasDespawnDontDestroy = midasDespawnDontDestroyDefault;
        public static bool midasRemovesCorpse = midasRemovesCorpseDefault;
        public static int midasGoldAmount = midasGoldAmountDefault;
        public static float TaurailFearRadius = TaurailFearRadiusDefault;
        public static float SeeRedFearRadius = SeeRedFearRadiusDefault;
        public static float SeeRedFleeRadius = SeeRedFleeRadiusDefault;
        public static int SeeRedFearDuration = SeeRedFearDurationDefault;
        public static int lactateMilkAmount = lactateMilkAmountDefault;

        // The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref milkableFemales, "milkableFemales", milkableFemalesDefault);
            Scribe_Values.Look(ref milkableMales, "milkableMales", milkableMalesDefault);
            Scribe_Values.Look(ref midasDespawnDontDestroy, "midasDestroysOrDespawns", midasDespawnDontDestroyDefault);
            Scribe_Values.Look(ref midasRemovesCorpse, "midasRemovesCorpse", midasRemovesCorpseDefault);
            Scribe_Values.Look(ref midasGoldAmount, "midasGoldAmount", midasGoldAmountDefault);
            Scribe_Values.Look(ref TaurailFearRadius, "TaurailFearRadius", TaurailFearRadiusDefault);
            Scribe_Values.Look(ref SeeRedFearRadius, "SeeRedFearRadius", SeeRedFearRadiusDefault);
            Scribe_Values.Look(ref SeeRedFleeRadius, "SeeRedFleeRadius", SeeRedFleeRadiusDefault);
            Scribe_Values.Look(ref SeeRedFearDuration, "SeeRedFearDuration", SeeRedFearDurationDefault);
            Scribe_Values.Look(ref lactateMilkAmount, "lactateMilkAmount", lactateMilkAmountDefault);

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

            listingStandard.Label("Lactation Ability Settings");
            listingStandard.CheckboxLabeled("Active for female pawns", ref MinotaurSettings.milkableFemales);
            listingStandard.CheckboxLabeled("Active for male pawns", ref MinotaurSettings.milkableMales);
            listingStandard.Label("Lactation milk amount: " + MinotaurSettings.lactateMilkAmount.ToString());
            listingStandard.IntAdjuster(ref MinotaurSettings.lactateMilkAmount, 5);
            listingStandard.Label(" ");

            listingStandard.Label("See Red Ability Settings");
            MinotaurSettings.SeeRedFearRadius = listingStandard.SliderLabeled("See Red ability radius: " + MinotaurSettings.SeeRedFearRadius.ToString(), MinotaurSettings.SeeRedFearRadius, 1f, 50f, 0.5f, "The radius within which pawns will flee from a pawn using the See Red ability.");
            MinotaurSettings.SeeRedFearRadius = (float)Math.Round(MinotaurSettings.SeeRedFearRadius * 2.0) / 2;
            MinotaurSettings.SeeRedFleeRadius = listingStandard.SliderLabeled("See Red flee radius: " + MinotaurSettings.SeeRedFleeRadius.ToString(), MinotaurSettings.SeeRedFleeRadius, 1f, 50f, 0.5f, "The distance which pawns will flee from a pawn using the See Red ability.");
            MinotaurSettings.SeeRedFleeRadius = (float)Math.Round(MinotaurSettings.SeeRedFleeRadius * 2.0) / 2;
            float SeeRedFearDurationSeconds = (float)Math.Round((float)MinotaurSettings.SeeRedFearDuration / 60.0f, 1);
            MinotaurSettings.SeeRedFearDuration = (int)listingStandard.SliderLabeled("See Red fear duration: " + SeeRedFearDurationSeconds.ToString() + "s (" + MinotaurSettings.SeeRedFearDuration.ToString() + " ticks)", MinotaurSettings.SeeRedFearDuration, 1f, 600f, 0.5f, "The distance which pawns will flee from a pawn using the See Red ability.");

            listingStandard.Label(" ");

            listingStandard.Label("Taurail Gun Weapon Settings");
            MinotaurSettings.TaurailFearRadius = listingStandard.SliderLabeled("Taurail Gun projectile blast radius: " + MinotaurSettings.TaurailFearRadius.ToString(), MinotaurSettings.TaurailFearRadius, 1f, 50f, 0.5f, "The radius in which pawns will flee from a Taurail Gun projectile explosion.");
            MinotaurSettings.TaurailFearRadius = (float)Math.Round(MinotaurSettings.TaurailFearRadius * 2.0) / 2;
            listingStandard.Label(" ");

            listingStandard.Label("Midaspear Weapon Settings");
            listingStandard.CheckboxLabeled("Removes corpses after killing", ref MinotaurSettings.midasRemovesCorpse, "More fun, but makes pawns killed by the Midaspear impossible to resurrect.");
            listingStandard.CheckboxLabeled("Despawns corpses rather than destroy", ref MinotaurSettings.midasDespawnDontDestroy, "Destroying a corpse is a volatile action, and can cause occasional errors. Despawning a pawn will may not trigger some on on-death events.");
            listingStandard.Label("Midaspear gold amount: " + MinotaurSettings.midasGoldAmount.ToString());
            listingStandard.IntAdjuster(ref MinotaurSettings.midasGoldAmount, 5);
            
            if (listingStandard.ButtonText("Reset to Default"))
            {
                MinotaurSettings.milkableFemales = MinotaurSettings.milkableFemalesDefault;
                MinotaurSettings.milkableMales = MinotaurSettings.milkableMalesDefault;
                MinotaurSettings.midasDespawnDontDestroy = MinotaurSettings.midasDespawnDontDestroyDefault;
                MinotaurSettings.midasRemovesCorpse = MinotaurSettings.midasRemovesCorpseDefault;
                MinotaurSettings.midasGoldAmount = MinotaurSettings.midasGoldAmountDefault;
                MinotaurSettings.TaurailFearRadius = MinotaurSettings.TaurailFearRadiusDefault;
                MinotaurSettings.SeeRedFearRadius = MinotaurSettings.SeeRedFearRadiusDefault;
                MinotaurSettings.SeeRedFleeRadius = MinotaurSettings.SeeRedFleeRadiusDefault;
                MinotaurSettings.SeeRedFearDuration = MinotaurSettings.SeeRedFearDurationDefault;
                MinotaurSettings.lactateMilkAmount = MinotaurSettings.lactateMilkAmountDefault;
            }

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