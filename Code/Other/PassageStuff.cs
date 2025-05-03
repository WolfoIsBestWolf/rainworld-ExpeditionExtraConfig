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


            On.Menu.SleepAndDeathScreen.Singal += SleepAndDeathScreen_Singal;
        }

       

        public static WinState.EndgameID ReturnRandomEndgameScreen(WinState self)
        {
            List<WinState.EndgameID> list = new List<WinState.EndgameID>();


            for (int i = 0; i < self.endgameTrackers.Count; i++)
            {
                if (self.endgameTrackers[i].GoalFullfilled)
                {
                    list.Add(self.endgameTrackers[i].ID);
                }
            }
            list.Remove(WinState.EndgameID.Survivor);
            if (list.Count > 0)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
            return (WinState.EndgameID.Survivor);
        }

        private static void SleepAndDeathScreen_Singal(On.Menu.SleepAndDeathScreen.orig_Singal orig, Menu.SleepAndDeathScreen self, Menu.MenuObject sender, string message)
        {
    
            if (message == "EXPPASSAGE")
            {
                if (ModManager.Expedition && self.manager.rainWorld.ExpeditionMode && ExpeditionData.earnedPassages > 0 && Perk_Passages.NormalPassage)
                {
                    ExpeditionData.earnedPassages--;
                    self.PlaySound(SoundID.MENU_Passage_Button);
                    if (self.proceedWithEndgameID == null)
                    {
                        self.proceedWithEndgameID = ReturnRandomEndgameScreen(self.winState);
                        if (self.proceedWithEndgameID != null)
                        {
                            //self.endgameTokens.Passage(self.proceedWithEndgameID);
                            //self.endGameSceneCounter = 1;
                            self.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.CustomEndGameScreen);
                            return;
                        }
                    }
                    self.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.FastTravelScreen);
                    return;
                }
            }
            orig(self, sender, message);
        }

        public static void SleepAndDeathScreen_AddSubObjects(On.Menu.SleepAndDeathScreen.orig_AddSubObjects orig, Menu.SleepAndDeathScreen self)
        {
            orig(self);
            if (ModManager.Expedition && self.manager.rainWorld.ExpeditionMode)
            {
                if (!ExpeditionGame.activeUnlocks.Contains("unl-passage") && (Perk_Passages.NormalPassage || WConfig.cfgInfinitePassage.Value))
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
            else if (Perk_Passages.NormalPassage)
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