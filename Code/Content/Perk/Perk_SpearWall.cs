using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;
using System;

 
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
                return new Color(1f,0.9f,0.8f);
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
                if (ModManager.MMF && MMF.cfgDislodgeSpears.Value)
                {
                    return T.TranslateLineBreak("Perk_Already_Remix");
                }
                return T.Translate("Perk_SpearWall_Desc");
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_SpearWall_Name");
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
            IL.Player.CanIPickThisUp += Player_CanIPickThisUp;
        }

        private void Player_CanIPickThisUp(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoPrev(MoveType.After,
            x => x.MatchLdsfld("MoreSlugcats.MMF", "cfgDislodgeSpears"),
            x => x.MatchCallvirt("Configurable`1<bool>", "get_Value")))
            {
                c.EmitDelegate<Func<bool, bool>>((spear) =>
                {
                    if (spear != true)
                    {
                        if (Custom.rainWorld.ExpeditionMode)
                        {
                            if (ExpeditionGame.activeUnlocks.Contains("unl-eec-SpearWall"))
                            {
                                return true;
                            }
                        }
                    }
                    return spear;
                });
            }
            else
            {
                Debug.Log("Failed IL Spear Perk ");
            }
        }

        public override bool AvailableForSlugcat(SlugcatStats.Name name)
        {
            if (ModManager.MMF && MMF.cfgDislodgeSpears.Value)
            {
                return false;
            }
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
            {
                return false;
            }
            return true;
        }

    }
}