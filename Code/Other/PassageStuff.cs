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
    public class PassageStuff
    {
        public static void Start()
        {
            On.Menu.SleepAndDeathScreen.AddExpeditionPassageButton += SleepAndDeathScreen_AddExpeditionPassageButton;
            On.Menu.SleepAndDeathScreen.AddSubObjects += SleepAndDeathScreen_AddSubObjects;

            //For trying to reintroduce end game screens
            //On.Menu.SleepAndDeathScreen.GetDataFromGame += SleepAndDeathScreen_GetDataFromGame;
            //On.Menu.SleepAndDeathScreen.Singal += SleepAndDeathScreen_Singal;
        }

        public static void SleepAndDeathScreen_GetDataFromGame(On.Menu.SleepAndDeathScreen.orig_GetDataFromGame orig, Menu.SleepAndDeathScreen self, Menu.KarmaLadderScreen.SleepDeathScreenDataPackage package)
        {
            orig(self, package);
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (WConfig.cfgPassageTeleportation.Value)
                {
                    self.endgameTokens = new Menu.EndgameTokens(self, self.pages[0], new Vector2(self.LeftHandButtonsPosXAdd + self.manager.rainWorld.options.SafeScreenOffset.x + 140f, Mathf.Max(15f, self.manager.rainWorld.options.SafeScreenOffset.y)), self.container, self.karmaLadder);
                    self.pages[0].subObjects.Add(self.endgameTokens);
                }
            }
            
        }

        private static void SleepAndDeathScreen_Singal(On.Menu.SleepAndDeathScreen.orig_Singal orig, Menu.SleepAndDeathScreen self, Menu.MenuObject sender, string message)
        {
            orig(self, sender, message);
            if (message == "EXPPASSAGE")
            {
                if (ModManager.Expedition && self.manager.rainWorld.ExpeditionMode && ExpeditionData.earnedPassages == 0)
                {
                    if (self.proceedWithEndgameID == null)
                    {
                        self.proceedWithEndgameID = self.winState.GetNextEndGame();
                        if (self.proceedWithEndgameID != null)
                        {
                            self.endgameTokens.Passage(self.proceedWithEndgameID);
                            self.endGameSceneCounter = 1;
                            self.PlaySound(SoundID.MENU_Passage_Button);
                            return;
                        }
                    }
                }
            }
        }

        public static void SleepAndDeathScreen_AddSubObjects(On.Menu.SleepAndDeathScreen.orig_AddSubObjects orig, Menu.SleepAndDeathScreen self)
        {
            orig(self);
            if (ModManager.Expedition && self.manager.rainWorld.ExpeditionMode)
            {
                if (!ExpeditionGame.activeUnlocks.Contains("unl-passage") && (WConfig.cfgPassageTeleportation.Value || WConfig.cfgInfinitePassage.Value))
                {
                    ExpLog.Log("Add Expedition Passage through not normal means.");
                    self.AddExpeditionPassageButton();
                }
            }
        }

        private static void SleepAndDeathScreen_AddExpeditionPassageButton(On.Menu.SleepAndDeathScreen.orig_AddExpeditionPassageButton orig, Menu.SleepAndDeathScreen self)
        {
            //int extraPassages = 0;
            if (WConfig.cfgInfinitePassage.Value)
            {
                ExpeditionData.earnedPassages = 255;
            }
            else if (WConfig.cfgPassageTeleportation.Value)
            {
                for (int i = 0; i < self.saveState.deathPersistentSaveData.winState.endgameTrackers.Count; i++)
                {
                    if (self.saveState.deathPersistentSaveData.winState.endgameTrackers[i].GoalFullfilled && !self.saveState.deathPersistentSaveData.winState.endgameTrackers[i].consumed)
                    {
                        //extraPassages++;
                        self.saveState.deathPersistentSaveData.winState.endgameTrackers[i].consumed = true;
                        Expedition.ExpeditionData.earnedPassages++;
                    }
                }
                //ExpeditionData.earnedPassages += extraPassages;
            }
            orig(self);
            /*if (WConfig.cfgPassageTeleportation.Value)
            {
                ExpeditionData.earnedPassages -= extraPassages;
            }*/
        }
    }
}