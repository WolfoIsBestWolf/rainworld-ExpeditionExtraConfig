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
            On.RainWorldGame.ctor += AddILHooks;

            On.WorldLoader.OverseerSpawnConditions += SpearSunOverseer;
            On.RainCycle.GetDesiredCycleLength += RivuletRainCycle;
            On.Expedition.ExpeditionGame.IsMSCRoomScript += RivuletAllowPebblesBall;

            On.SaveState.ctor += ChangeStartingKarma; 
            On.Expedition.ExpeditionGame.PrepareExpedition += ExpeditionGame_PrepareExpedition;
            On.SlugcatStats.SlugcatStartingKarma += SlugcatStats_SlugcatStartingKarma;

            On.Expedition.AchievementChallenge.ValidForThisSlugcat += AchievementChallenge_ValidForThisSlugcat;
            On.Expedition.PearlDeliveryChallenge.RegionPoints += PearlDeliveryChallenge_RegionPoints;
            On.Expedition.ExpeditionGame.GetRegionWeight += ExpeditionGame_GetRegionWeight;


            On.Player.ctor += StartWithStomachPearl;
        }

        private void StartWithStomachPearl(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            Debug.Log(world.rainCycle.CycleProgression);
            if (world.game.rainWorld.ExpeditionMode && world.game.rainWorld.progression.currentSaveState.cycleNumber == 0 && world.rainCycle.CycleProgression <= 2f)
            {
                Debug.Log("Pearl");
                if (self.SlugCatClass == SlugcatStats.Name.Red)
                {
                    world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, DataPearl.AbstractDataPearl.DataPearlType.Red_stomach);
                }
                else if (self.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    world.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, abstractCreature.spawnDen, world.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Rivulet_stomach);
                }
                
            }
            Debug.Log("Player_ctor 222222222222222222222222222222222222222");
        }

        private bool RivuletAllowPebblesBall(On.Expedition.ExpeditionGame.orig_IsMSCRoomScript orig, UpdatableAndDeletable item)
        {
            if (item is MSCRoomSpecificScript.RM_CORE_EnergyCell)
            {
                return false;
            }
            return orig(item);
        }


        private void ExpeditionGame_PrepareExpedition(On.Expedition.ExpeditionGame.orig_PrepareExpedition orig)
        {
            orig(); 
            Expedition.ExpeditionGame.tempKarma = EECSettings.cfgKarmaStart.Value;
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {

            }
            Debug.Log("ExpeditionGame_PrepareExpedition " + ExpeditionData.slugcatPlayer.value);
        }

        private void ChangeStartingKarma(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self, saveStateNumber, progression);
            if (Custom.rainWorld.ExpeditionMode)
            {
                self.deathPersistentSaveData.karmaCap = EECSettings.cfgKarmaCapStart.Value; 
                //self.deathPersistentSaveData.karma = ExpeditionGame.tempKarma; //Temp Karma used to circumvent setting Karma each start
                if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && EECSettings.cfgRivuletShortCycles.Value)
                {
                    self.miscWorldSaveData.pebblesEnergyTaken = false;
                }
                else if (saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Artificer && EECSettings.cfgArtificerRobo.Value)
                {
                    self.hasRobo = true;
                }
            }
        }


        private int SlugcatStats_SlugcatStartingKarma(On.SlugcatStats.orig_SlugcatStartingKarma orig, SlugcatStats.Name slugcatNum)
        {
            if (RWCustom.Custom.rainWorld.ExpeditionMode)
            {
                if (ModManager.MSC && slugcatNum == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    return 4;
                }
                if (ModManager.MSC && slugcatNum == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    return 4;
                }
                return EECSettings.cfgKarmaCapStart.Value;
            }
            return orig(slugcatNum);
        }

        private static int RivuletRainCycle(On.RainCycle.orig_GetDesiredCycleLength orig, RainCycle self)
        {
            if (self.world.game.rainWorld.ExpeditionMode && ModManager.MSC && !self.world.singleRoomWorld && (self.world.game.session as StoryGameSession).saveState.saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {   
                int num = orig(self);
                float multi = EECSettings.cfgRivuletBonusRain.Value;

                if (self.world.region.name == "VS" || self.world.region.name == "UW" || self.world.region.name == "SH" || self.world.region.name == "SB" || self.world.region.name == "SL")
                {
                    num = (int)(num * multi / 1.5f);
                }
                else
                {
                    num = (int)(num * multi);
                }
                return num;
            }
            return orig(self);
        }

        private bool SpearSunOverseer(On.WorldLoader.orig_OverseerSpawnConditions orig, WorldLoader self, SlugcatStats.Name character)
        {
            //World tempWorld = self.ReturnWorld();
            if (ModManager.MSC && character == MoreSlugcatsEnums.SlugcatStatsName.Spear && RWCustom.Custom.rainWorld.ExpeditionMode)
            {
                if (self.ReturnWorld() != null && (self.ReturnWorld().region.name != "SS" || self.ReturnWorld().region.name != "DM"))
                {
                    return true;
                }
            }

            return orig(self, character);
        }

        private void AddConfigHook(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            Debug.Log("ExpeditionExtraConfig: Wolfo Mod Loaded");
            MachineConnector.SetRegisteredOI("ExpeditionExtraConfig", EECSettings.instance);
        }

        private static void AddILHooks(On.RainWorldGame.orig_ctor orig, RainWorldGame game, ProcessManager manager)
        {
            Debug.Log("ExpdExConfig: IL Hooks being added");
            if (EECSettings.cfgKarmaFlower.Value)
            {
                IL.Room.Loaded += EnableNaturalKarmaFlower;
            }
            if (EECSettings.cfgRivuletShortCycles.Value)
            {
                IL.RainCycle.ctor += RivuletShelterFailure;
            }
            if (EECSettings.cfgRivuletBonusRain.Value == 1)
            {
                On.RainCycle.GetDesiredCycleLength -= RivuletRainCycle;
            }
            if (EECSettings.cfgSaintAscendPoints.Value)
            {
                IL.Player.ClassMechanicsSaint += SaintAscendMurder;
            }


            IL.Menu.GhostEncounterScreen.GetDataFromGame += GhostEncounterScreen_GetDataFromGame; //Probably fine to always have this

            IL.StoryGameSession.ctor += StoryGameSession_ctor; //Smth about limiting Karma
            IL.World.SpawnGhost += World_SpawnGhost; //Ghost Encountering Mechanic hook

            orig(game, manager);
            On.RainWorldGame.ctor -= AddILHooks; //Seems fine to only ever run this once
        }



        private static void SaintAscendMurder(ILContext il)
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
                    if (creature is Creature && !(creature as Creature).dead)
                    {
                        if (Custom.rainWorld.ExpeditionMode)
                        {
                            player.SessionRecord.AddKill((creature as Creature));
                        }
                    }
                    return creature;
                });
                Debug.Log("aaa: Saint Murder Points Success"); //could be improved
            }
            else
            {
                Debug.Log("aaa: Saint Murder Points Fail");
            }
        }

        private static void World_SpawnGhost(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"),
            x => x.MatchBrfalse(out _),
            x => x.MatchLdsfld("ModManager", "Expedition")
            )) 
            { 
                Debug.Log(c);
                c.RemoveRange(9);
                Debug.Log(c);
                Debug.Log("aaa: Ghost Encounter IL Success");
            }
            else
            {
                Debug.Log("aaa: Ghost Encounter IL Failed");
            }
        }

        private static void GhostEncounterScreen_GetDataFromGame(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"));

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdflda("Menu.KarmaLadderScreen/SleepDeathScreenDataPackage", "karma")))
            {
                c.Index -= 1;
                Debug.Log(c);
                c.RemoveRange(8);
                Debug.Log(c);
                Debug.Log("aaa: GhostEncounterScreen IL Success");
            }
            else
            {
                Debug.Log("aaa: GhostEncounterScreen IL Failed");
            }

        }

        private static void StoryGameSession_ctor(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("ModManager", "Expedition"));

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("StoryGameSession", "saveState")))
            {
                c.Index -= 1;
                Debug.Log(c);
                c.RemoveRange(16);
                Debug.Log(c);
                Debug.Log("aaa: StoryGameSession_ctor IL Success");
            }
            else
            {
                Debug.Log("aaa: StoryGameSession_ctorIL Failed");
            }
        }


        private static void RivuletShelterFailure(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("MiscWorldSaveData", "pebblesEnergyTaken"));
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("DeathPersistentSaveData", "altEnding")))
            {
                c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, RainCycle, bool>>((Check, self) =>
                {
                    if (self.world.game.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    else
                    {
                        return self.world.game.GetStorySession.saveState.deathPersistentSaveData.altEnding;
                    }
                });
                Debug.Log("aaa: Rivulet PreCycleChance Hook Success");
            }
            else
            {
                Debug.Log("aaa: Rivulet PreCycleChance Hook Failed");
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
                Debug.Log("aaa: Karma Flower Hook Succeeded");
            }
            else
            {
                Debug.Log("aaa: Karma Flower Hook Failed");
            }
        }

        private int ExpeditionGame_GetRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            if (region == "GW" && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Spear)
            {
                return 6;
            }
            return orig(region);
        }

        private int PearlDeliveryChallenge_RegionPoints(On.Expedition.PearlDeliveryChallenge.orig_RegionPoints orig, PearlDeliveryChallenge self)
        {
            if (self.region == "MS")
            {
                return 35;
            }
            return orig(self);
        }

        private bool AchievementChallenge_ValidForThisSlugcat(On.Expedition.AchievementChallenge.orig_ValidForThisSlugcat orig, Expedition.AchievementChallenge self, SlugcatStats.Name slugcat)
        {
            if (((ModManager.MSC && slugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint) || (!ModManager.MSC && slugcat == SlugcatStats.Name.Yellow)) && self.ID == WinState.EndgameID.Scholar)
            {
                return false;
            }
            else if (ModManager.MSC && slugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint && self.ID == WinState.EndgameID.Hunter)
            {
                return false;
            }
            return orig(self, slugcat);
        }

    }
}
