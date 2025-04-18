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
    public class KarmaStuff
    {
        public static void Start()
        {
            On.Expedition.ExpeditionGame.PrepareExpedition += ChangeStartingKarma_PrepareExpedition;
            IL.Room.Loaded += EnableNaturalKarmaFlower;
            IL.RainWorldGame.GoToDeathScreen += RemovePermaDeath;

            On.SlugcatStats.SlugcatStartingKarma += GhostKarma_SlugcatStartingKarma;
            On.SSOracleBehavior.SeePlayer += PebblesIncreaseKarma; //Handles SpearOverseer too so
          
            IL.Menu.GhostEncounterScreen.GetDataFromGame += ExpedNotLimit_GhostScreen; //Expedition always pretends you have 4 Karma for this screen specifically,Probably fine to always have this
            IL.StoryGameSession.ctor += ExpedNotLimit_StoryGame; //Will limit all Karma to max 4 in Expd. But also main thing that removes Ghosts increasing Karma
        }


        public static void ExpedNotLimit_StoryGame(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("ModManager", "Expedition")))
            {
                c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                {
                    return false;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: StoryGameSession_ctor IL Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: StoryGameSession_ctorIL Failed");
            }
        }


        public static void ChangeStartingKarma_PrepareExpedition(On.Expedition.ExpeditionGame.orig_PrepareExpedition orig)
        {
            orig();
            if (ExpeditionData.slugcatPlayer == Watcher.WatcherEnums.SlugcatStatsName.Watcher)
            {
                return;
            }
            ExpeditionGame.tempKarma = WConfig.cfgKarmaStart.Value - 1;
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Red)
            {
                ExpeditionGame.tempKarma += WConfig.cfgHunterPlusKarma.Value;
            }
            int expectedMax = WConfig.cfgKarmaStartMax.Value - 1;
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Artificer && WConfig.cfgArti_MaxKarmaBool.Value)
            {
                expectedMax = WConfig.cfgArti_MaxKarma.Value - 1;
            }
            else if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint && WConfig.cfgSaint_MaxKarmaBool.Value)
            {
                expectedMax = WConfig.cfgSaint_MaxKarma.Value - 1;
            }
            ExpeditionGame.tempKarma = System.Math.Min(ExpeditionGame.tempKarma, expectedMax);

            UnityEngine.Debug.Log("ChangeStartingKarma_PrepareExpedition " + ExpeditionData.slugcatPlayer.value);
        }


        public static void RemovePermaDeath(ILContext il)
        {
            ILCursor c = new(il);

            c.TryGotoNext(MoveType.Before,
               x => x.MatchCallvirt("RainWorld", "get_ExpeditionMode"));

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("DeathPersistentSaveData", "karma")))
            {
                c.EmitDelegate<System.Func<int, int>>((karma) =>
                {
                    if (karma == 0 && WConfig.cfgRemovePermaDeath.Value)
                    {
                        UnityEngine.Debug.Log("Preventing Expedition game over.");
                        return 100;
                    }
                    return karma;
                });
                //UnityEngine.Debug.Log("ExpeditionConfig: RemovePermaDeath Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionConfig: RemovePermaDeath Failed");
            }
        }

        public static void PebblesIncreaseKarma(On.SSOracleBehavior.orig_SeePlayer orig, SSOracleBehavior self)
        {
            if (Custom.rainWorld.ExpeditionMode && self.oracle.ID == Oracle.OracleID.SS)
            {
                if (WConfig.cfgMaxKarmaPebbles.Value && self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad == 55 && (self.oracle.room.game.StoryCharacter == SlugcatStats.Name.White || self.oracle.room.game.StoryCharacter == SlugcatStats.Name.Yellow || self.oracle.room.game.StoryCharacter == SlugcatStats.Name.Red || self.oracle.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Gourmand))
                {
                    self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad++;
                    self.SlugcatEnterRoomReaction();
                    self.NewAction(SSOracleBehavior.Action.General_GiveMark);
                    self.afterGiveMarkAction = SSOracleBehavior.Action.ThrowOut_ThrowOut;
                    return;
                }
                else if (self.oracle.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Spear)
                {
                    self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad++;
                    self.NewAction(SSOracleBehavior.Action.ThrowOut_ThrowOut);
                    self.afterGiveMarkAction = SSOracleBehavior.Action.ThrowOut_ThrowOut;
                    return;
                }
            }
            orig(self);
            UnityEngine.Debug.Log("Iterator is seeing Player " + self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad);

        }


        public static void EnableNaturalKarmaFlower(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Preventing natural KarmaFlower spawn")))
            {
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Expedition"));

                c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                {
                    if (WConfig.cfgKarmaFlower.Value)
                    {
                        return false;
                    }
                    return karma;
                });

                //c.Next.OpCode = OpCodes.Brtrue_S;
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Karma Flower Hook Succeeded");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Karma Flower Hook Failed");
            }
        }

        public static int GhostKarma_SlugcatStartingKarma(On.SlugcatStats.orig_SlugcatStartingKarma orig, SlugcatStats.Name slugcatNum)
        {
            if (RWCustom.Custom.rainWorld.ExpeditionMode)
            {
                if (!WConfig.cfgMaxKarmaEchos.Value)
                {
                    //Does a >= for the results so even if the result is negative it wont change
                    //How the fuck does this work
                    return -50;
                }
                if (slugcatNum == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    if (WConfig.cfgArti_MaxKarmaBool.Value)
                    {
                        return WConfig.cfgArti_MaxKarma.Value - 1;
                    }
                }
                return WConfig.cfgKarmaStartMax.Value - 1;
            }
            return orig(slugcatNum);
        }


        public static void ExpedNotLimit_GhostScreen(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("ModManager", "Expedition")))
            {
                c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                {
                    return false;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: GhostEncounterScreen IL Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: GhostEncounterScreen IL Failed");
            }
        }


    }
}