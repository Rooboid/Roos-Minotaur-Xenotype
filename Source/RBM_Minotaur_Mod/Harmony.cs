using RBM_Minotaur;
using HarmonyLib;
using Verse;

namespace RBM_Minotaur
{
    [StaticConstructorOnStartup]
    public static class RBM_Minotaur
    {
        static RBM_Minotaur()
        {
            Log.Message("RBM_Minotaur Static class loaded");
            Harmony harmony = new Harmony("rimworld.mod.rooboid.minotaur");
            harmony.PatchAll();
        }
    }
}
