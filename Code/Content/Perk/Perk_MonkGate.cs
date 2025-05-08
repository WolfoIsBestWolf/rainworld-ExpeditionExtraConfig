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
                return WConfig.cfgNewPerksForceUnlock.Value;
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
                if (ModManager.MMF && MMF.cfgGlobalMonkGates.Value)
                {
                    return T.TranslateLineBreak("Perk_Already_Remix");
                }
                return T.Translate("Perk_MonkGate_Desc");
                return "Gates will remain open permanently after passing through them once.\nUnpickable if enabled in Remix.";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_MonkGate_Name");
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
            if (ModManager.MMF && MMF.cfgGlobalMonkGates.Value)
            {
                return false;
            }
            return true;
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