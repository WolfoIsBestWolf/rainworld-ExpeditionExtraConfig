using BepInEx;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Collections.Generic;
using MoreSlugcats;
using Expedition;
using RWCustom;

namespace ExpeditionExtraConfig
{
    public class Artificer
    {
 
        public static void Start()
        {
            //On.MoreSlugcats.MSCRoomSpecificScript.LC_FINAL.TriggerFadeToEnding += ArtificerEarlyExpeditionEnd;
        }



        public void ArtificerEarlyExpeditionEnd(On.MoreSlugcats.MSCRoomSpecificScript.LC_FINAL.orig_TriggerFadeToEnding orig, MSCRoomSpecificScript.LC_FINAL self)
        {
            if (self.room.game.rainWorld.ExpeditionMode)
            {
                if (self.endingTriggerTime == 0)
                {
                    new FadeOut(self.room, Color.black, 290f, false);
                    self.player.controller = new Player.NullController();
                }
                else if (self.endingTriggerTime == 300)
                {
                    ExpeditionGame.voidSeaFinish = true;
                    ExpeditionGame.expeditionComplete = true;
                    return;
                }
                self.endingTriggerTime++;
                return;
            }
            orig(self);
        }


 

        /*private void ArtificerNeedCorpseForDoor(On.RegionGate.orig_customKarmaGateRequirements orig, RegionGate self)
       {
           orig(self);
           if (EECSettings.cfgArtificerNeedCorpse.Value && self.room.game.rainWorld.ExpeditionMode && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
           {
               UnityEngine.Debug.Log("ExpeditionExtraConfig: Arti Gate");
               
                //Could probably still like check if there is a corpse but I assume it actually increases your Karma temporarily
            
           }
       }*/
    }
}