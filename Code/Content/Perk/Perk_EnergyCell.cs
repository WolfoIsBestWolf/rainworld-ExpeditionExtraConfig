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
                return true;
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
                return "Start the expedition with a Rarefaction Cell";
            }
        }
        public override string DisplayName
        {
            get
            {
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
    }
}