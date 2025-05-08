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
    public class Perk_EnergyCell : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-EnergyCell";
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
                return ItemSymbol.ColorForItem(MoreSlugcatsEnums.AbstractObjectType.EnergyCell, 0);
            }
        }
        public override string SpriteName
        {
            get
            {
                return ItemSymbol.SpriteNameForItem(MoreSlugcatsEnums.AbstractObjectType.EnergyCell, 0);
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
                return T.Translate("Perk_EnergyCell_Desc");
                return "Start the expedition with a Rarefaction Cell";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_EnergyCell_Name");
                return "Rarefaction Cell";
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
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && WConfig.cfgRivuletBall.Value)
            {
                return false;
            }
            return true;
        }
    }
}