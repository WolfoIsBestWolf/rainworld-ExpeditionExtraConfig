using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;


 
namespace ExpeditionExtraConfig
{
    public class Perk_MonkGate : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-MonkGate";
            }
        }
        public override bool UnlockedByDefault
        {
            get
            {
                return true;
            }
        }
        public override Color Color
        {
            get
            {
                return PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Yellow);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Kill_Slugcat";
            }
        }
        public override string ManualDescription
        {
            get
            {
                return Description;
            }
        }
        public override string Description
        {
            get
            {
                return "Gates will remain open permanently after passing through them once.\nUnpickable if enabled in Remix.";
            }
        }
        public override string DisplayName
        {
            get
            {
                return "Unlock Karma Gates";
            }
        }
        public override string Group
        {
            get
            {
                return "ExpeditionExtraConfig";
            }
        }
        public override bool AvailableForSlugcat(SlugcatStats.Name name)
        {
            if (name == SlugcatStats.Name.Yellow)
            {
                return false;
            }
            return name != SlugcatStats.Name.Yellow || !MMF.cfgGlobalMonkGates.Value;
        }

        public override void ApplyHooks()
        {
            base.ApplyHooks();
            On.DeathPersistentSaveData.CanUseUnlockedGates += DeathPersistentSaveData_CanUseUnlockedGates;
        }

        private bool DeathPersistentSaveData_CanUseUnlockedGates(On.DeathPersistentSaveData.orig_CanUseUnlockedGates orig, DeathPersistentSaveData self, SlugcatStats.Name slugcat)
        {
            return orig(self,slugcat) || Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("unl-eec-MonkGate");
        }
    }
}