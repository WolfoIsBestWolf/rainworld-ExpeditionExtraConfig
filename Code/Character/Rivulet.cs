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
    public class Rivulet
    {
        public static bool RainyCycle = false;
        public static void Start()
        {
            //If Alt ending checked


            //Shelter Failure not set to -1 all the time, instead being 25 or 40
            //DetathRain mode not messed up
            //Random scripts where Rain is slowed or sped up disabled

            On.RainCycle.ctor += RollRandomRain;
 
            IL.RainCycle.ctor += RivuletShelterFailure;
            On.RainCycle.GetDesiredCycleLength += ShorterRain;
            IL.RainCycle.GetDesiredCycleLength += DisableVanillaShortCycles;
     

            #region Score/Challenge manip
            On.Expedition.CycleScoreChallenge.Generate += LessPointsNeeded_ScoreInOneCycle;

            On.Expedition.VistaChallenge.Points += RivMorePoints_Vista;
            On.Expedition.PearlHoardChallenge.Points += RivMorePoints_PearlHoard;
            On.Expedition.PearlDeliveryChallenge.Points += RivMorePoints_PearlDeliver;
            On.Expedition.NeuronDeliveryChallenge.Points += RivMorePoints_NeuronDeliver;
            #endregion
        }

       

     
        private static void DisableVanillaShortCycles(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("MiscWorldSaveData", "pebblesEnergyTaken")))
            {
                c.EmitDelegate<Func<bool, bool>>((failRate) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgRivuletShortCycles.Value)
                        {
                            return true;
                        }
                    }
                    return failRate;
                });
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Rivulet PreCycleChance Hook Failed");
            }
        }

        private static void RollRandomRain(On.RainCycle.orig_ctor orig, RainCycle self, World world, float minutes)
        {
            if (world.game.IsStorySession && Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    RainyCycle = false;
                    if (world.region.name != "MS")
                    {
                        if (WConfig.cfgRiv_HeavyRainChance.Value > 0)
                        {
                            //UnityEngine.Debug.Log("Random short cycle");
                            world.game.GetStorySession.SetRandomSeedToCycleSeed(1);
                            RainyCycle = UnityEngine.Random.Range(0, 100) < WConfig.cfgRiv_HeavyRainChance.Value;
                            Debug.Log("Expedition roll for short Rivulet Cycle : " + world.game.GetStorySession.saveState.cycleNumber + " result : " + RainyCycle);
                        }
                    }
                    /*if (world.game.GetStorySession.saveState.cycleNumber < EECSettings.cfgRiv_HeavyRainDuration.Value)
                    {
                        UnityEngine.Debug.Log("Guaranteed short cycle");
                         RainyCycle = true;
                    }*/                   
                }
            }
            orig(self,world,minutes);
        }

        public static int RivMorePoints_NeuronDeliver(On.Expedition.NeuronDeliveryChallenge.orig_Points orig, NeuronDeliveryChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value && WConfig.cfgRiv_ShortCyclePointBonus.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        public static int RivMorePoints_PearlDeliver(On.Expedition.PearlDeliveryChallenge.orig_Points orig, PearlDeliveryChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value && WConfig.cfgRiv_ShortCyclePointBonus.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        public static int RivMorePoints_PearlHoard(On.Expedition.PearlHoardChallenge.orig_Points orig, PearlHoardChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value && WConfig.cfgRiv_ShortCyclePointBonus.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        public static int RivMorePoints_Vista(On.Expedition.VistaChallenge.orig_Points orig, VistaChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value && WConfig.cfgRiv_ShortCyclePointBonus.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        public static int ShorterRain(On.RainCycle.orig_GetDesiredCycleLength orig, RainCycle self)
        {
            if (self.world.game.rainWorld.ExpeditionMode && self.world.game.TimelinePoint == SlugcatStats.Timeline.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value && RainyCycle)
                {
                    int num = orig(self);
                    num = (int)((float)num * WConfig.cfgRiv_RainMult.Value);
                    return num;
                }
            }
            return orig(self);
        }

        public static void RivuletShelterFailure(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("MiscWorldSaveData", "pebblesEnergyTaken")))
            {
                c.EmitDelegate<Func<bool, bool>>((pebblesEnergyTaken) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgRivuletShortCycles.Value)
                        {
                            return true;
                        }
                    }
                    return pebblesEnergyTaken;
                });
                //Highjack the 8 in the code, even tho we make pebblesEnergyTaken false for Expd
                c.TryGotoNext(MoveType.After,
                x => x.MatchLdcR4(8));  
                c.EmitDelegate<Func<float, float>>((failRate) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgRivuletShortCycles.Value && RainyCycle)
                        {
                            Debug.Log("Shelter Fail Rate " + WConfig.cfgRiv_ShelterFailRate.Value);
                            return WConfig.cfgRiv_ShelterFailRate.Value;
                        }
                    }
                    return failRate;
                });
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Rivulet PreCycleChance Hook Failed");
            }
        }

  

        public static Challenge LessPointsNeeded_ScoreInOneCycle(On.Expedition.CycleScoreChallenge.orig_Generate orig, CycleScoreChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                if (WConfig.cfgRivuletShortCycles.Value)
                {
                    CycleScoreChallenge temp = (CycleScoreChallenge)orig(self);
                    float multi = Mathf.Min(1, WConfig.cfgRiv_RainMult.Value * 1.2f);
                    temp.target = Mathf.RoundToInt(Mathf.Lerp(20f, 125f * multi, ExpeditionData.challengeDifficulty) / 5f) * 5;
                    return temp;
                }
               
            }
            return orig(self);
        }


    }
}