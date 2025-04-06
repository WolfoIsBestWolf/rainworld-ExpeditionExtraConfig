using BepInEx;
using Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using RWCustom;
//using System;
using System.Collections.Generic;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;


namespace ExpeditionExtraConfig
{
    [BepInPlugin("wolfo.ExpeditionExtraConfig", "ExpeditionExtraConfig", "1.2.0")]
    public class ExpeditionExtraConfig : BaseUnityPlugin
    {
        public bool init;
        public void OnEnable()
        {
            UnityEngine.Debug.Log("ExpeditionExtraConfig");
            On.RainWorld.OnModsInit += AddConfigHook;
            On.RainWorldGame.ctor += AddILHooks; //Update this shit to be two way
            On.RainWorld.PostModsInit += OnHooks; //This is after config is loaded


        }


        public void OnHooks(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self);
            UnityEngine.Debug.Log("ExpeditionExtraConfig: On Hooks being added");

            On.SlugcatStats.SlugcatStartingKarma += GhostKarma_SlugcatStartingKarma;
            On.GhostWorldPresence.SpawnGhost += GhostCharacterEncounterMechanics;
            On.SSOracleBehavior.SeePlayer += PebblesIncreaseKarma; //Handles SpearOverseer too so
            IL.RainWorldGame.GoToDeathScreen += RemovePermaDeath;
            //Text removing in an update idk

            On.Player.ctor += StartWithStomachPearl; //Handles Pearl and Ballin
            On.SaveState.ctor += ChangeStartingStats;
            On.Expedition.ExpeditionGame.PrepareExpedition += ChangeStartingKarma_PrepareExpedition;
            //game.setupValues.slugPupsMax
            On.Expedition.ExpeditionGame.GetRegionWeight += ExpeditionGame_GetRegionWeight; //More Garbage Wastes!!!!

        
            Monk.Start();
            Artificer.Start();
            Rivulet.Start();
            Saint.Start();
            //WatcherExpedition.Start();
            Score.Start();

            Slugpup.Start();

            On.HUD.Map.ctor += Map_ctor;

            IL.Menu.PauseMenu.ctor += OtherStuff.PauseMenu_ctor;
            On.Music.MusicPlayer.GameRequestsSong += OtherStuff.AlwaysPlayMusic;

            #region KarmaJank
            IL.Menu.GhostEncounterScreen.GetDataFromGame += GhostEncounterScreen_GetDataFromGame; //Expedition always pretends you have 4 Karma for this screen specifically,Probably fine to always have this
            IL.StoryGameSession.ctor += StoryGameSession_ctor; //Will limit all Karma to max 4 in Expd. But also main thing that removes Ghosts increasing Karma
            IL.Room.Loaded += EnableNaturalKarmaFlower;
            #endregion

            On.Menu.ChallengeSelectPage.UpdateChallengeButtons += FixPointsNotBeingDisplayed;

            //Passage Test
            //IL.Menu.SleepAndDeathScreen.AddPassageButton += SleepAndDeathScreen_AddPassageButton;
            //On.Menu.SleepAndDeathScreen.GetDataFromGame += SleepAndDeathScreen_GetDataFromGame;

            On.RWCustom.Custom.Log += Custom_Log;
        }

        private void SleepAndDeathScreen_GetDataFromGame(On.Menu.SleepAndDeathScreen.orig_GetDataFromGame orig, Menu.SleepAndDeathScreen self, Menu.KarmaLadderScreen.SleepDeathScreenDataPackage package)
        {
            orig(self, package);
            if (self.IsSleepScreen || self.IsDeathScreen || self.IsStarveScreen)
            {
                self.endgameTokens = new Menu.EndgameTokens(self, self.pages[0], new Vector2(self.LeftHandButtonsPosXAdd + self.manager.rainWorld.options.SafeScreenOffset.x + 140f, Mathf.Max(15f, self.manager.rainWorld.options.SafeScreenOffset.y)), self.container, self.karmaLadder);
                self.pages[0].subObjects.Add(self.endgameTokens);
            }
        }

  

        private void SleepAndDeathScreen_AddPassageButton(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdsfld("ModManager", "Expedition")))
            {
                c.EmitDelegate<System.Func<bool, bool>>((exped) =>
                {
                    /*if (EECSettings.cfgPassageTeleportation.Value)
                    {
                        return false;
                    }*/
                    return exped;
                });
                UnityEngine.Debug.Log("ExpeditionConfig: SleepAndDeathScreen_AddPassageButton good");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionConfig: SleepAndDeathScreen_AddPassageButton Failed");
            }
        }

        private void FixPointsNotBeingDisplayed(On.Menu.ChallengeSelectPage.orig_UpdateChallengeButtons orig, Menu.ChallengeSelectPage self)
        {
            orig(self);
            if (!self.menu.Translate("POINTS: <score>").EndsWith("<score>"))
            {
                string newValue = Menu.Remix.ValueConverter.ConvertToString<int>(ExpeditionGame.CalculateScore(true));
                self.pointsLabel.text += " " + newValue;
            }
           
        }

        private void Custom_Log(On.RWCustom.Custom.orig_Log orig, string[] values)
        {
            UnityEngine.Debug.Log(string.Join(" ", values));
        }

        private void RemovePermaDeath(ILContext il)
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
                        UnityEngine.Debug.Log("Preventing Expedition game over, you're welcome.");
                        return 1;
                    }
                    return karma;
                });
                UnityEngine.Debug.Log("ExpeditionConfig: RemovePermaDeath Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionConfig: RemovePermaDeath Failed");
            }
        }

        private void Map_ctor(On.HUD.Map.orig_ctor orig, HUD.Map self, HUD.HUD hud, HUD.Map.MapData mapData)
        {
            orig(self, hud, mapData);
            if (Custom.rainWorld.ExpeditionMode)
            {
                //UnityEngine.Debug.Log(self.DiscoverResolution);
                self.DiscoverResolution = 7f * WConfig.cfgMapRevealRadius.Value;
                //UnityEngine.Debug.Log(self.DiscoverResolution);
            }

        }


        public bool GhostCharacterEncounterMechanics(On.GhostWorldPresence.orig_SpawnGhost orig, GhostWorldPresence.GhostID ghostID, int karma, int karmaCap, int ghostPreviouslyEncountered, bool playingAsRed)
        {
            if (Custom.rainWorld.ExpeditionMode && Custom.rainWorld.progression.currentSaveState.cycleNumber > 0)
            {
                /*if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    UnityEngine.Debug.Log("Artificer only when max Karma and previous encounter");
                    if (karma >= karmaCap)
                    {
                        return ghostPreviouslyEncountered < 2;
                    }
                }*/
                if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    if (WConfig.cfgSaint_Echoes.Value)
                    {
                        //Idk why this works but it just does??
                        UnityEngine.Debug.Log("Saint regardless of Karma and previous encounter");
                        return ghostPreviouslyEncountered < 2;
                    }
                }
            }
            return orig(ghostID, karma, karmaCap, ghostPreviouslyEncountered, playingAsRed);
        }

        public void PebblesIncreaseKarma(On.SSOracleBehavior.orig_SeePlayer orig, SSOracleBehavior self)
        {
            UnityEngine.Debug.Log("Iterator is seeing Player");
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
                else if (self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad == -55 && self.oracle.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Spear)
                {
                    self.NewAction(SSOracleBehavior.Action.ThrowOut_ThrowOut);
                    self.afterGiveMarkAction = SSOracleBehavior.Action.ThrowOut_ThrowOut;
                    return;
                }
            }
            orig(self);
        }




 

        public void StartWithStomachPearl(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            //UnityEngine.Debug.Log("Player_ctor");
            if (world.game.rainWorld.ExpeditionMode && world.game.rainWorld.progression.currentSaveState.cycleNumber == 0 && self.abstractCreature.Room.name == ExpeditionData.startingDen && world.rainCycle.CycleProgression <= 2f)
            {
                if (WConfig.cfgStomachPearl.Value && self.SlugCatClass == SlugcatStats.Name.Red)
                {
                    world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, DataPearl.AbstractDataPearl.DataPearlType.Red_stomach);
                }
                else if (self.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    if (WConfig.cfgStomachPearl.Value)
                    {
                        world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Rivulet_stomach);
                    }
                    if (WConfig.cfgRivuletBall.Value && self.abstractCreature.Room.realizedRoom.shelterDoor != null)
                    {
                        UnityEngine.Debug.Log("Rivulet is ballin");
                        WorldCoordinate pos3 = new WorldCoordinate(self.abstractCreature.Room.index, self.abstractCreature.Room.realizedRoom.shelterDoor.playerSpawnPos.x, self.abstractCreature.Room.realizedRoom.shelterDoor.playerSpawnPos.y, 0);
                        AbstractPhysicalObject abstractPhysicalBall = new AbstractPhysicalObject(world, MoreSlugcatsEnums.AbstractObjectType.EnergyCell, null, pos3, world.game.GetNewID());
                        self.abstractCreature.Room.entities.Add(abstractPhysicalBall);
                        abstractPhysicalBall.RealizeInRoom();
                    }
                }
            }
        }




        public void ChangeStartingKarma_PrepareExpedition(On.Expedition.ExpeditionGame.orig_PrepareExpedition orig)
        {
            orig();
           
            ExpeditionGame.tempKarma = WConfig.cfgKarmaStart.Value - 1;
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Red)
            {
                ExpeditionGame.tempKarma += WConfig.cfgHunterPlusKarma.Value;
            }
            int expectedMax = WConfig.cfgKarmaCapStart.Value - 1;
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

        public void ChangeStartingStats(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self, saveStateNumber, progression);
            UnityEngine.Debug.Log("SaveState_ctor");
            if (Custom.rainWorld.ExpeditionMode)
            { 
                self.deathPersistentSaveData.karmaCap = WConfig.cfgKarmaCapStart.Value - 1;
 

                if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    self.miscWorldSaveData.pebblesEnergyTaken = !WConfig.cfgRivuletShortCycles.Value;
                    self.deathPersistentSaveData.altEnding = true; //This just fixes some junk think it's worth keeping
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    self.hasRobo = WConfig.cfgArtificerRobo.Value;
                    if (WConfig.cfgArti_MaxKarmaBool.Value)
                    {
                        self.deathPersistentSaveData.karmaCap = WConfig.cfgArti_MaxKarma.Value - 1;
                    }
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    if (WConfig.cfgSaint_MaxKarmaBool.Value)
                    {
                        self.deathPersistentSaveData.karmaCap = WConfig.cfgSaint_MaxKarma.Value - 1;
                    }
                    self.deathPersistentSaveData.SaintEnlightMessage = true;
                    self.deathPersistentSaveData.KarmicBurstMessage = true;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                {
                    if (WConfig.cfgGourmand_StartWithPups.Value)
                    {
                        self.forcePupsNextCycle = 1;
                    }
                }
                else if (saveStateNumber == SlugcatStats.Name.Yellow)
                {
                    if (WConfig.cfgMonk_StartWithPups.Value)
                    {
                        self.forcePupsNextCycle = 1;
                    }
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Spear && WConfig.cfgSpearOverseer.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = -55;
                }
                if (WConfig.cfgMaxKarmaPebbles.Value && (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Gourmand || saveStateNumber == SlugcatStats.Name.Red || saveStateNumber == SlugcatStats.Name.White || saveStateNumber == SlugcatStats.Name.Yellow))
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 55;
                }
            }
        }


        public int GhostKarma_SlugcatStartingKarma(On.SlugcatStats.orig_SlugcatStartingKarma orig, SlugcatStats.Name slugcatNum)
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
                return WConfig.cfgKarmaCapStart.Value - 1;
            }
            return orig(slugcatNum);
        }


        public static void AddILHooks(On.RainWorldGame.orig_ctor orig, RainWorldGame game, ProcessManager manager)
        {
            //UnityEngine.Debug.Log("ExpeditionExtraConfig: IL Hooks being added");
            //Why do we do this here?
         
           

            orig(game, manager);
            On.RainWorldGame.ctor -= AddILHooks; //Seems fine to only ever run this once
        }



        public static void GhostEncounterScreen_GetDataFromGame(ILContext il)
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

        public static void StoryGameSession_ctor(ILContext il)
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

        public int ExpeditionGame_GetRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            if (region == "GW")
            {
                return 5;
            }
            else if (region == "DM")
            {
                return 6;
            }
            return orig(region);
        }




        public void AddConfigHook(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            UnityEngine.Debug.Log("ExpeditionExtraConfig: Wolfo Mod Loaded");
            MachineConnector.SetRegisteredOI("ExpeditionExtraConfig", WConfig.instance);
        }

        public static void ArtificerGateWrongSymbol(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"));

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("RegionGate/GateRequirement", "OneKarma"),
            x => x.MatchStfld("GateKarmaGlyph", "requirement")
            ))
            {
                c.Index -= 1;
                c.RemoveRange(4);
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Karma Gate IL Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Karma Gate IL Failed");
            }
        }
    }
}
