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
    public class Perk_Pups : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-Pups";
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
                return new Color(0.9f, 1f, 0.8f);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "MotherB";
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
                return T.Translate("Perk_Mother_Desc");
                return "Start the expedition with a family of 2 Slugpups";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_Mother_Name");
                return "Mother";
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
            if (!ModManager.MSC)
            {
                return false;
            }
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.White && WConfig.cfgSurvivor_StartWithPups.Value)
            {
                return false;
            }     
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Gourmand && WConfig.cfgGourmand_StartWithPups.Value)
            {
                return false;
            }
            return true;

        }

    }
}