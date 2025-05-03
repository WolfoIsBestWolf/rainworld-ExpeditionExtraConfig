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
    public class Perk_ExplosiveSpear : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-ExplosiveSpear";
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
                return ItemSymbol.ColorForItem(AbstractPhysicalObject.AbstractObjectType.Spear, 1);
            }
        }
        public override string SpriteName
        {
            get
            {
                return ItemSymbol.SpriteNameForItem(AbstractPhysicalObject.AbstractObjectType.Spear, 1);
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
                return "Start the expedition with a Explosive Spear";
            }
        }
        public override string DisplayName
        {
            get
            {
                return "Explosive Spear";
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