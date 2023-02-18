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
    [BepInPlugin("wolfo.ExpeditionExtraConfig", "ExpeditionExtraConfig", "1.0.0")]
    public class ExpeditionExtraConfig : BaseUnityPlugin
    {
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += AddConfigHook;
            On.RainWorldGame.ctor += AddILHooks; //Update this shit to be two way
            On.RainWorld.PostModsInit += OnHooks; //This is after config is loaded
        }


        public void OnHooks(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self);
            Debug.Log("ExpeditionExtraConfig: On Hooks being added");

            On.SlugcatStats.SlugcatStartingKarma += GhostKarma_SlugcatStartingKarma;
            On.GhostWorldPresence.SpawnGhost += GhostCharacterEncounterMechanics;
            On.SSOracleBehavior.SeePlayer += PebblesIncreaseKarma; //Handles SpearOverseer too so

            On.Player.ctor += StartWithStomachPearl; //Handles Pearl and Ballin
            On.SaveState.ctor += ChangeStartingStats;
            On.Expedition.ExpeditionGame.PrepareExpedition += ChangeStartingKarma_PrepareExpedition;

            if (EECSettings.cfgRivuletShortCycles.Value)
            {
                On.Expedition.CycleScoreChallenge.Generate += Riv.RivuletFriendlyCycleScore;
            }
            if (EECSettings.cfgMoreRegions.Value)
            {
                On.Menu.ExpeditionMenu.ExpeditionSetup += ExpeditionMenu_ExpeditionSetup;
                On.Expedition.PearlDeliveryChallenge.Generate += AllowMoreRegions_PearlDelivery;
                On.Expedition.ChallengeTools.ValidRegionPearl += EquipvelantRegions_ValidRegionPearl;
                On.Expedition.PearlDeliveryChallenge.RegionPoints += Score.PearlDeliveryChallenge_RegionPoints;
                //On.SlugcatStats.getSlugcatStoryRegions += SlugcatStats_getSlugcatStoryRegions; //Big Jellyfish aren't counted and there's not really any other unique enemy to parse in MS
            }

            Score.OnEnable();
            
           
            On.Expedition.ChallengeTools.GenerateCreatureScores += NoTerrorLongLegs;
            
            On.Expedition.ExpeditionGame.GetRegionWeight += ExpeditionGame_GetRegionWeight; //More Garbage Wastes!!!!
            On.Expedition.AchievementChallenge.ValidForThisSlugcat += AchievementChallenge_ValidForThisSlugcat;
            //On.MoreSlugcats.MSCRoomSpecificScript.LC_FINAL.TriggerFadeToEnding += ArtificerEarlyExpeditionEnd;
        }

        private void ExpeditionMenu_ExpeditionSetup(On.Menu.ExpeditionMenu.orig_ExpeditionSetup orig, Menu.ExpeditionMenu self)
        {
            orig(self);
            //Debug.Log("ExpeditionExtraConfig: ExpeditionMenu_ExpeditionSetup");
            ChallengeTools.echoScores[(int)MoreSlugcatsEnums.GhostID.MS] = 90;
            ChallengeTools.PearlRegionBlackList.Remove("LM");

            IL.Expedition.EchoChallenge.Generate -= EchoChallengeAddMSEcho;
            IL.Expedition.EchoChallenge.Generate += EchoChallengeAddMSEcho;
        }

        private void EchoChallengeAddMSEcho(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "get_Item"),
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Add")
            ))
            {
                c.Index -= 3;
                c.EmitDelegate<Func<List<string>, List<string>>>((list) =>
                {
                    if (Custom.rainWorld.ExpeditionMode && ExpeditionData.challengeDifficulty > 0.75f && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                    {
                        if (!list.Contains("MS"))
                        {
                            list.Add("MS");
                        }
                    }
                    return list;
                });
                Debug.Log("ExpeditionExtraConfig: Allow MS Echo Saint IL Success");
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: Allow MS Echo Saint IL Failed");
            }
        }

        private void NoTerrorLongLegs(On.Expedition.ChallengeTools.orig_GenerateCreatureScores orig, ref Dictionary<string, int> dict)
        {
            orig(ref dict);
            if (dict.ContainsKey("TerrorLongLegs"))
            {
                if (EECSettings.cfgBetterBlacklist.Value)
                {
                    //Debug.Log("ExpeditionExtraConfig: Remove Mother Long Legs");
                    dict.Remove("TerrorLongLegs", out _);
                }
                else
                {
                    dict["TerrorLongLegs"] = 50; //Bro really gave Mommy Long Legs same as Daddy Long Legs
                }
            }
        }

        public bool GhostCharacterEncounterMechanics(On.GhostWorldPresence.orig_SpawnGhost orig, GhostWorldPresence.GhostID ghostID, int karma, int karmaCap, int ghostPreviouslyEncountered, bool playingAsRed)
        {
            if (Custom.rainWorld.ExpeditionMode && Custom.rainWorld.progression.currentSaveState.cycleNumber > 0)
            {
                if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    Debug.Log("Artificer only when max Karma and previous encounter");
                    if (karma >= karmaCap)
                    {
                        return ghostPreviouslyEncountered < 2;
                    }
                }
                if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    Debug.Log("Saint regardless of Karma and previous encounter");
                    return ghostPreviouslyEncountered < 2;
                }
            }
            return orig(ghostID, karma, karmaCap, ghostPreviouslyEncountered, playingAsRed);
        }

        public void PebblesIncreaseKarma(On.SSOracleBehavior.orig_SeePlayer orig, SSOracleBehavior self)
        {
            Debug.Log("Iterator is seeing Player");
            if(self.oracle.room.game.rainWorld.ExpeditionMode && self.oracle.ID == Oracle.OracleID.SS)
            {
                if (EECSettings.cfgPebblesIncreaseKarma.Value && self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad == 55 && (self.oracle.room.game.StoryCharacter == SlugcatStats.Name.White || self.oracle.room.game.StoryCharacter == SlugcatStats.Name.Yellow || self.oracle.room.game.StoryCharacter == SlugcatStats.Name.Red || self.oracle.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Gourmand))
                {
                    self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad++;
                    self.SlugcatEnterRoomReaction();
                    self.NewAction(SSOracleBehavior.Action.General_GiveMark);
                    self.afterGiveMarkAction = SSOracleBehavior.Action.ThrowOut_ThrowOut;
                    return;
                }
                else if (self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad == -55 && self.oracle.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Spear)
                {
                    self.NewAction(SSOracleBehavior.Action.ThrowOut_KillOnSight);
                    self.afterGiveMarkAction = SSOracleBehavior.Action.ThrowOut_KillOnSight;
                    return;
                }
            }
            orig(self);
        }

        public Challenge AllowMoreRegions_PearlDelivery(On.Expedition.PearlDeliveryChallenge.orig_Generate orig, PearlDeliveryChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && ChallengeTools.PearlRegionBlackList.Contains("MS"))
            {
                ChallengeTools.PearlRegionBlackList.Remove("MS");
                //Debug.Log("Remove MS from Blacklist");
            }
            else if (!ChallengeTools.PearlRegionBlackList.Contains("MS"))
            {
                ChallengeTools.PearlRegionBlackList.Add("MS");
                //Debug.Log("ReAdd MS from Blacklist");
            }
            return orig(self);
        }


        public void ArtificerEarlyExpeditionEnd(On.MoreSlugcats.MSCRoomSpecificScript.LC_FINAL.orig_TriggerFadeToEnding orig, MSCRoomSpecificScript.LC_FINAL self)
        {
            if(self.room.game.rainWorld.ExpeditionMode)
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

        public bool EquipvelantRegions_ValidRegionPearl(On.Expedition.ChallengeTools.orig_ValidRegionPearl orig, string region, DataPearl.AbstractDataPearl.DataPearlType type)
        {
            string b = type.value.Substring(0, 2);
            if (region == "MS" && type.value == "DM" || region == "LM" && b == "SL" || region == "SS" && type.value == "PebblesPearl")
            {
                return true;
            }
            return orig(region, type);
        }

        public void StartWithStomachPearl(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            //Debug.Log("Player_ctor");
            if (world.game.rainWorld.ExpeditionMode && world.game.rainWorld.progression.currentSaveState.cycleNumber == 0 && self.abstractCreature.Room.name == ExpeditionData.startingDen && world.rainCycle.CycleProgression <= 2f)
            {
                if (EECSettings.cfgStomachPearl.Value && self.SlugCatClass == SlugcatStats.Name.Red)
                {
                    world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, DataPearl.AbstractDataPearl.DataPearlType.Red_stomach);
                }
                else if (self.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    if (EECSettings.cfgStomachPearl.Value)
                    {
                        world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Rivulet_stomach);
                    }

                    if (EECSettings.cfgRivuletBall.Value && self.abstractCreature.Room.realizedRoom.shelterDoor != null)
                    {
                        Debug.Log("Rivulet is ballin");
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
            Expedition.ExpeditionGame.tempKarma = EECSettings.cfgKarmaStart.Value-1;
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Red)
            {
                ExpeditionGame.tempKarma += EECSettings.cfgHunterPlusKarma.Value;
            }
            Math.Min((ExpeditionGame.tempKarma - 1), (EECSettings.cfgKarmaCapStart.Value - 1));
            //Debug.Log("ChangeStartingKarma_PrepareExpedition " + ExpeditionData.slugcatPlayer.value);
        }

        public void ChangeStartingStats(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self, saveStateNumber, progression);
            Debug.Log("SaveState_ctor");
            if (Custom.rainWorld.ExpeditionMode)
            {
                self.deathPersistentSaveData.karmaCap = EECSettings.cfgKarmaCapStart.Value - 1; 
                //self.deathPersistentSaveData.karma = ExpeditionGame.tempKarma; //Temp Karma used to circumvent setting Karma each start
                if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && EECSettings.cfgRivuletShortCycles.Value)
                {
                    self.miscWorldSaveData.pebblesEnergyTaken = false;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Artificer && EECSettings.cfgArtificerRobo.Value)
                {
                    self.hasRobo = true;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Spear && EECSettings.cfgSpearOverseer.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = -55;
                }
                else if (EECSettings.cfgPebblesIncreaseKarma.Value && (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Gourmand || saveStateNumber == SlugcatStats.Name.Red || saveStateNumber == SlugcatStats.Name.White || saveStateNumber == SlugcatStats.Name.Yellow))
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 55;
                }
            }
        }


        public int GhostKarma_SlugcatStartingKarma(On.SlugcatStats.orig_SlugcatStartingKarma orig, SlugcatStats.Name slugcatNum)
        {
            if (RWCustom.Custom.rainWorld.ExpeditionMode)
            {
                if (!EECSettings.cfgGhostsIncreaseKarma.Value)
                {
                    //Does a >= for the results so even if the result is negative it wont change
                    return -50;
                }
                /*if (ModManager.MSC && slugcatNum == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    return 4;
                }
                if (ModManager.MSC && slugcatNum == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    return 4;
                }*/
                return EECSettings.cfgKarmaCapStart.Value - 1;
            }
            return orig(slugcatNum);
        }


        public static void AddILHooks(On.RainWorldGame.orig_ctor orig, RainWorldGame game, ProcessManager manager)
        {
            Debug.Log("ExpeditionExtraConfig: IL Hooks being added");
            if (EECSettings.cfgKarmaFlower.Value)
            {
                IL.Room.Loaded += EnableNaturalKarmaFlower;
            }
            /*if (EECSettings.cfgArtificerRobo.Value)
            {
                IL.GateKarmaGlyph.ctor += ArtificerGateWrongSymbol;
            }*/
            if (EECSettings.cfgRivuletShortCycles.Value)
            {
                IL.RainCycle.ctor += Riv.RivuletShelterFailure;
                On.RainCycle.GetDesiredCycleLength += Riv.RivuletRainCycle;
                On.Expedition.ExpeditionGame.IsMSCRoomScript += Riv.RivuletAllowPebblesBall;
            }
            if (EECSettings.cfgSaintAscendPoints.Value)
            {
                IL.Player.ClassMechanicsSaint += SaintAscendMurder;
            }
          
            IL.Menu.GhostEncounterScreen.GetDataFromGame += GhostEncounterScreen_GetDataFromGame; //Expedition always pretends you have 4 Karma for this screen specifically,Probably fine to always have this
            IL.StoryGameSession.ctor += StoryGameSession_ctor; //Will limit all Karma to max 4 in Expd. But also main thing that removes Ghosts increasing Karma

            //IL.World.SpawnGhost += World_SpawnGhost; //Ghost Encountering Mechanic hook

            orig(game, manager);
            On.RainWorldGame.ctor -= AddILHooks; //Seems fine to only ever run this once
        }


        public static void SaintAscendMurder(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchIsinst("Creature"),
            x => x.MatchCallOrCallvirt("Creature", "Die")
            ))
            {
                //c.Index -= 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<PhysicalObject, Player, PhysicalObject>>((creature, player) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (creature is Creature && !(creature as Creature).dead)
                        {
                            player.SessionRecord.AddKill((creature as Creature));
                        }
                    }
                    return creature;
                });
                Debug.Log("ExpeditionExtraConfig: Saint Murder Points Success"); //could be improved
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: Saint Murder Points Fail");
            }
        }


        public static void GhostEncounterScreen_GetDataFromGame(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"));

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdflda("Menu.KarmaLadderScreen/SleepDeathScreenDataPackage", "karma")))
            {
                c.Index -= 1;
                c.RemoveRange(8);
                Debug.Log("ExpeditionExtraConfig: GhostEncounterScreen IL Success");
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: GhostEncounterScreen IL Failed");
            }

        }

        public static void StoryGameSession_ctor(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"));

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("StoryGameSession", "saveState")))
            {
                c.Index -= 1;
                c.RemoveRange(16);
                Debug.Log("ExpeditionExtraConfig: StoryGameSession_ctor IL Success");
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: StoryGameSession_ctorIL Failed");
            }
        }

        public static void EnableNaturalKarmaFlower(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Preventing natural KarmaFlower spawn")))
            {
                c.TryGotoPrev(MoveType.Before,
                x => x.MatchLdsfld("ModManager", "Expedition"));
                c.Index += 1;
                c.Next.OpCode = OpCodes.Brtrue_S;
                Debug.Log("ExpeditionExtraConfig: Karma Flower Hook Succeeded");
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: Karma Flower Hook Failed");
            }
        }

        public int ExpeditionGame_GetRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            if (region == "GW" && (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Artificer || ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Spear))
            {
                return 5;
            }
            return orig(region);
        }

        public bool AchievementChallenge_ValidForThisSlugcat(On.Expedition.AchievementChallenge.orig_ValidForThisSlugcat orig, Expedition.AchievementChallenge self, SlugcatStats.Name slugcat)
        {
            if ((ModManager.MSC && slugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint) || (!ModManager.MSC && slugcat == SlugcatStats.Name.Yellow && self.ID == WinState.EndgameID.Scholar))
            {
                return false;
            }
            return orig(self, slugcat);
        }


        public void AddConfigHook(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            Debug.Log("ExpeditionExtraConfig: Wolfo Mod Loaded");
            MachineConnector.SetRegisteredOI("ExpeditionExtraConfig", EECSettings.instance);

            //OnHooks(); //Why would doing this here make it work better
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
                Debug.Log("ExpeditionExtraConfig: Karma Gate IL Success");
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: Karma Gate IL Failed");
            }
        }
    }
}
