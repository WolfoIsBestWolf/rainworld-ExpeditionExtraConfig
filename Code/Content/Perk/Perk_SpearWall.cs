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
    public class Perk_SpearWall : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-eec-SpearWall";
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
                return Color.gray;
            }
        }
        public override string SpriteName
        {
            get
            {
                return ItemSymbol.SpriteNameForItem(AbstractPhysicalObject.AbstractObjectType.Spear, 0);
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
                return "Gives the player the ability to dislodge spears that are embedded in walls\nUnpickable if enabled in Remix.";
            }
        }
        public override string DisplayName
        {
            get
            {
                return "Dislodge Spears";
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
    }
}