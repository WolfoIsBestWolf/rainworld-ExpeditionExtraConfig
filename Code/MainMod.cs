using BepInEx;
using Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using RWCustom;
using System;
using System.Collections.Generic;
using UnityEngine;
using Watcher;
using HUD;

namespace ExpeditionExtraConfig
{
    [BepInPlugin("wolfo.ExpeditionExtraConfig", "ExpeditionExtraConfig", "1.3.0")]
    public class ExpeditionExtraConfig : BaseUnityPlugin
    {
        public static bool initialized = false;
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        }

        private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            //After Config has loaded
            orig(self);
            WConfig.CfgUnlockAll_OnChange();

        }

        public void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            if (initialized)
            {
                return;
            }
            initialized = true;

            UnityEngine.Debug.Log("ExpeditionExtraConfig: Wolfo Mod Loaded");
            MachineConnector.SetRegisteredOI("ExpeditionExtraConfig", WConfig.instance);
            OnHooks();
        }

        public void OnHooks()
        {
            UnityEngine.Debug.Log("ExpeditionExtraConfig: On Hooks being added");

         
            On.SaveState.ctor += ChangeStartingStats;
            KarmaStuff.Start();
            Monk.Start();
            Spearmaster.Start();
            Artificer.Start();
            Rivulet.Start();
            Saint.Start();
            //
            Score.Start();

            Slugpup.Start();
            ColorMenu.Start();
            Jukebox.Start();
            On.Player.ctor += StartWithStomachPearl; //Handles Pearl and Ballin
            On.Expedition.ExpeditionGame.GetRegionWeight += ExpeditionGame_GetRegionWeight; //More Garbage Wastes!!!!


            On.HUD.Map.ctor += OtherStuff.BiggerMapExped;
            IL.Menu.PauseMenu.ctor += OtherStuff.PauseMenu_ctor;
            On.Music.MusicPlayer.GameRequestsSong += OtherStuff.AlwaysPlayMusic;

            On.HUD.Map.ctor += VistaMap.AddVistaPointsToMap;
            On.Expedition.ChallengeTools.AppendAdditionalCreatureSpawns += ChallengeTools_AppendAdditionalCreatureSpawns;


            On.Menu.ChallengeSelectPage.UpdateChallengeButtons += FixPointsNotBeingDisplayed;
 
            On.GateKarmaGlyph.ctor += RemoveRoboGate;

            //On.Expedition.VistaChallenge.ChallengeName += VistaChallenge_ChallengeName;

            NewMissions.Start();
            PassageStuff.Start();
        }

   
        private string VistaChallenge_ChallengeName(On.Expedition.VistaChallenge.orig_ChallengeName orig, VistaChallenge self)
        {
            return orig(self);
        }

        private void ChallengeTools_AppendAdditionalCreatureSpawns(On.Expedition.ChallengeTools.orig_AppendAdditionalCreatureSpawns orig)
        {
            orig();
            int num2;
            ChallengeTools.ExpeditionCreature item2 = new ChallengeTools.ExpeditionCreature
            {
                creature = DLCSharedEnums.CreatureTemplateType.StowawayBug,
                points = (ChallengeTools.creatureScores.TryGetValue(DLCSharedEnums.CreatureTemplateType.StowawayBug.value, out num2) ? num2 : 0),
                spawns = 1
            };
            if (ChallengeTools.creatureSpawns.ContainsKey(MoreSlugcatsEnums.SlugcatStatsName.Gourmand.value))
            {
                ChallengeTools.creatureSpawns[MoreSlugcatsEnums.SlugcatStatsName.Gourmand.value].Add(item2);
            }
             
        }

        private void RemoveRoboGate(On.GateKarmaGlyph.orig_ctor orig, GateKarmaGlyph self, bool side, RegionGate gate, RegionGate.GateRequirement requirement)
        {
            orig(self, side, gate, requirement);
            if (ModManager.MSC && requirement == MoreSlugcatsEnums.GateRequirement.RoboLock)
            {
                if (ModManager.Expedition && gate.room.game.rainWorld.ExpeditionMode && WConfig.cfgRemoveRoboLock.Value)
                {
                    if (gate.room.world.name == "DM")
                    {
                        return;
                    }
                    self.requirement = RegionGate.GateRequirement.OneKarma;
                    if (gate.karmaRequirements[0] == MoreSlugcatsEnums.GateRequirement.RoboLock)
                    {
                        gate.karmaRequirements[0] = RegionGate.GateRequirement.OneKarma;
                    }
                    if (gate.karmaRequirements[1] == MoreSlugcatsEnums.GateRequirement.RoboLock)
                    {
                        gate.karmaRequirements[1] = RegionGate.GateRequirement.OneKarma;
                    }
                }
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




        public void ChangeStartingStats(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self, saveStateNumber, progression);
            UnityEngine.Debug.Log("SaveState_ctor");
            if (Custom.rainWorld.ExpeditionMode)
            { 
                self.deathPersistentSaveData.karmaCap = WConfig.cfgKarmaStartMax.Value - 1;

                bool visitsPebbleKarma = false;
                if (saveStateNumber == SlugcatStats.Name.White)
                {
                    visitsPebbleKarma = true;
                }
                else if (saveStateNumber == SlugcatStats.Name.Yellow)
                {
                    visitsPebbleKarma = true;
                    if (WConfig.cfgMonk_StartWithPups.Value)
                    {
                        self.forcePupsNextCycle = 1;
                    }
                }
                else if (saveStateNumber == SlugcatStats.Name.Red)
                {
                    visitsPebbleKarma = true;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                {
                    visitsPebbleKarma = true;
                    if (WConfig.cfgGourmand_StartWithPups.Value)
                    {
                        self.forcePupsNextCycle = 1;
                    }
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    self.hasRobo = WConfig.cfgArtificerRobo.Value;
                    if (WConfig.cfgArti_MaxKarmaBool.Value)
                    {
                        self.deathPersistentSaveData.karmaCap = WConfig.cfgArti_MaxKarma.Value - 1;
                    }
                }
                if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    self.miscWorldSaveData.SLOracleState.neuronsLeft = 7;  //Set to 5 due to Hunter
                    self.miscWorldSaveData.pebblesEnergyTaken = !WConfig.cfgRivuletShortCycles.Value;
                    self.deathPersistentSaveData.altEnding = true; //This just fixes some junk think it's worth keeping
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Spear && WConfig.cfgSpearOverseer.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 0;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    self.miscWorldSaveData.SLOracleState.neuronsLeft = 7; //Set to 5 due to Hunter
                    if (WConfig.cfgSaint_MaxKarmaBool.Value)
                    {
                        self.deathPersistentSaveData.karmaCap = WConfig.cfgSaint_MaxKarma.Value - 1;
                    }
                    self.deathPersistentSaveData.SaintEnlightMessage = true;
                    self.deathPersistentSaveData.KarmicBurstMessage = true;
                }
                if (visitsPebbleKarma && WConfig.cfgMaxKarmaPebbles.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 55;
                }
            }
        }

        public int ExpeditionGame_GetRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            if (region == "GW")
            {
                return 5;
            }
            /*else if (region == "DM")
            {
                return 6;
            }*/
            return orig(region);
        }


    }
}
