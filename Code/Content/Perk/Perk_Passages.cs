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
    public class Perk_Passages : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-Passages";
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
                return new Color(1f, 0.75f, 0.1f);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "SurvivorB";
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
                    return T.TranslateLineBreak("Perk_Already_Option");
                }
                return T.Translate("Perk_Passage_Desc");
                //return "Enables the use of passages during expeditions which can be earned by completing the trackers on the sleep screen, like in campaigns.\nUnselectable if always enabled in settings.";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_Passage_Name");
                return "Classic Passages";
            }
        }
        public override string Group
        {
            get
            {
                return "ExpeditionExtraConfig";
            }
        }
        public override void ApplyHooks()
        {
            base.ApplyHooks();
        }
        public static bool NormalPassage
        {
            get
            {
                return WConfig.cfgPassageTeleportation.Value || Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("unl-eec-Passages");
            }
        }


    }
}