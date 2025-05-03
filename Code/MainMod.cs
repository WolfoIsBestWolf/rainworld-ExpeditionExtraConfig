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
    [BepInPlugin("wolfo.ExpeditionExtraConfig", "ExpeditionExtraConfig", "1.40")]
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

            Debug.Log("ExpeditionExtraConfig: Wolfo Mod Loaded");
            MachineConnector.SetRegisteredOI("ExpeditionExtraConfig", WConfig.instance);
            OnHooks();
        }

        public void OnHooks()
        {
            Debug.Log("ExpeditionExtraConfig: On Hooks being added");

         
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
         

            NewMissions.Start();
            NewPerks.Start();
            PassageStuff.Start();
            On.RainWorldGame.ctor += UnlockAll.SpawnRainbowCycleIfCheated;
            On.Menu.UnlockDialog.TogglePerk += UnlockAll.UnlockDialog_TogglePerk;
             
            On.GateKarmaGlyph.ctor += RemoveRoboGate;
            On.HUD.Map.GateMarker.ctor += RemoveRoboGateMark;
            Futile.atlasManager.LoadAtlas("atlases/eec_sprites");

             IL.Menu.ExpeditionMenu.ValidateQuestRewards += ExpeditionMenu_ValidateQuestRewards;
        }

        private void ExpeditionMenu_ValidateQuestRewards(ILContext il)
        {
            //This rechecks how many perks you should have,
            //And then, it does nothing
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdstr("SET PERK LIMIT TO: "));
 
            if (c.TryGotoPrev(MoveType.After,
            x => x.MatchLdloc(0)))
            {
                c.EmitDelegate<Func<int, int>>((karma) =>
                {
                    ExpeditionData.perkLimit = karma;
                    if (WConfig.cfgUnlockPerkSlots.Value)
                    {
                        ExpeditionData.perkLimit = 8;
                    }
                    ExpeditionData.perkLimit += WConfig.cfgBonusPerkSlots.Value;
                    ExpLog.Log("Actually setting Perk Limit to : "+ExpeditionData.perkLimit);
                    return karma;
                });
            }
            else
            {
               Debug.Log("ExpeditionExtraConfig: ValidateQuestRewards Failed");
            }
        }

       

        private void RemoveRoboGateMark(On.HUD.Map.GateMarker.orig_ctor orig, Map.GateMarker self, Map map, int room, RegionGate.GateRequirement karma, bool showAsOpen)
        {
            if (ModManager.MSC && karma == MoreSlugcatsEnums.GateRequirement.RoboLock)
            {
                if (Custom.rainWorld.ExpeditionMode && WConfig.cfgRemoveRoboLock.Value)
                {
                    if (map.RegionName != "DM")
                    {
                        karma = RegionGate.GateRequirement.OneKarma;
                        showAsOpen = true;
                    }
                   
                }
            }
            orig(self, map, room, karma, showAsOpen);
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

        
        public void StartWithStomachPearl(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            //UnityEngine.Debug.Log("Player_ctor");
            if (world.game.rainWorld.ExpeditionMode && world.game.rainWorld.progression.currentSaveState.cycleNumber == 0 && self.abstractCreature.Room.name == ExpeditionData.startingDen && world.rainCycle.CycleProgression <= 3f)
            {
                if ( self.SlugCatClass == SlugcatStats.Name.Red)
                {
                    if (WConfig.cfgStomachPearl.Value)
                    {
                        world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, DataPearl.AbstractDataPearl.DataPearlType.Red_stomach);
                    }       
                }
                else if (self.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    if (WConfig.cfgStomachPearl.Value)
                    {
                        world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Rivulet_stomach);
                    }
                    if (WConfig.cfgRivuletBall.Value && self.abstractCreature.Room.realizedRoom.shelterDoor != null)
                    {
                        WorldCoordinate pos3 = new WorldCoordinate(self.abstractCreature.Room.index, self.abstractCreature.Room.realizedRoom.shelterDoor.playerSpawnPos.x, self.abstractCreature.Room.realizedRoom.shelterDoor.playerSpawnPos.y, 0);
                        AbstractPhysicalObject abstractPhysicalBall = new AbstractPhysicalObject(world, MoreSlugcatsEnums.AbstractObjectType.EnergyCell, null, pos3, world.game.GetNewID());
                        self.abstractCreature.Room.entities.Add(abstractPhysicalBall);
                        abstractPhysicalBall.RealizeInRoom();
                    }
                }
                else if (ExpeditionData.activeMission == "EEC_Future1")
                {
                    world.game.FirstRealizedPlayer.objectInStomach = new AbstractPhysicalObject(world, AbstractPhysicalObject.AbstractObjectType.Lantern, null, abstractCreature.spawnDen, world.game.GetNewID());
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
                
                if (Expedition.ExpeditionData.activeMission == "EEC_Future1")
                {
                    self.currentTimelinePosition = SlugcatStats.SlugcatToTimeline(MoreSlugcatsEnums.SlugcatStatsName.Saint);
                }
                else if (Expedition.ExpeditionData.activeMission == "EEC_Past1")
                {
                    self.currentTimelinePosition = SlugcatStats.SlugcatToTimeline(MoreSlugcatsEnums.SlugcatStatsName.Spear);
                }

                bool visitsPebbleKarma = false;
                if (saveStateNumber == SlugcatStats.Name.White)
                {
                    visitsPebbleKarma = true; 
                    if (WConfig.cfgSurvivor_StartWithPups.Value)
                    {
                        self.forcePupsNextCycle = 1;
                    }
                }
                else if (saveStateNumber == SlugcatStats.Name.Yellow)
                {
                    visitsPebbleKarma = true;
                    
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
                    self.miscWorldSaveData.pebblesEnergyTaken = !WConfig.cfgRivuletShortCycles.Value;
                    self.deathPersistentSaveData.altEnding = true; //This just fixes some junk think it's worth keeping
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Spear && WConfig.cfgSpearOverseer.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 0;
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
                if (visitsPebbleKarma && WConfig.cfgMaxKarmaPebbles.Value)
                {
                    self.miscWorldSaveData.SSaiConversationsHad = 55;
                }
                if (SlugcatStats.AtOrAfterTimeline(self.currentTimelinePosition, SlugcatStats.Timeline.Rivulet))
                {
                    self.miscWorldSaveData.SLOracleState.neuronsLeft = 7; //Set to 5 due to Hunter
                }

                if (ExpeditionGame.activeUnlocks.Contains("unl-eec-Pups"))
                {
                    self.forcePupsNextCycle = 1;
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
