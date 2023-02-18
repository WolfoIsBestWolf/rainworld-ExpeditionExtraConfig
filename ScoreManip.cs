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
    public class Score
    {
        public static void OnEnable()
        {
            //Riv Bonus Points
            if (EECSettings.cfgRivuletShortCycles.Value)
            {
                On.Expedition.VistaChallenge.Points += BonusVistaChallenge_Points;
                On.Expedition.PearlHoardChallenge.Points += BonusPearlHoardChallenge_Points;
                On.Expedition.PearlDeliveryChallenge.Points += BonusPearlDeliveryChallenge_Points;
                On.Expedition.NeuronDeliveryChallenge.Points += BonusNeuronDeliveryChallenge_Points;
            }
            //Saint gets 1.35x score for
            On.Expedition.CycleScoreChallenge.Points += CycleScoreChallenge_Points;
            if (EECSettings.cfgSaintAscendPoints.Value)
            {
                On.Expedition.GlobalScoreChallenge.Points += GlobalScoreChallenge_Points;
                On.Expedition.HuntChallenge.Points += HuntChallenge_Points;
            }
            //MS points 
            //On.Expedition.EchoChallenge.Points += EchoChallenge_Points; //How does this crash the game even without changes
        }



        public static int HuntChallenge_Points(On.Expedition.HuntChallenge.orig_Points orig, HuntChallenge self)
        {
            if (ExpeditionGame.runData != null && EECSettings.cfgSaintAscendPoints.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * EECSettings.cfgSaintAscendPointPenalty.Value);
                }
            }
            return orig(self);
        }

        private static int GlobalScoreChallenge_Points(On.Expedition.GlobalScoreChallenge.orig_Points orig, GlobalScoreChallenge self)
        {
            if (ExpeditionGame.runData != null && EECSettings.cfgSaintAscendPoints.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * EECSettings.cfgSaintAscendPointPenalty.Value);
                }
            }
            return orig(self);
        }

        private static int CycleScoreChallenge_Points(On.Expedition.CycleScoreChallenge.orig_Points orig, CycleScoreChallenge self)
        {
            if (ExpeditionGame.runData != null && EECSettings.cfgSaintAscendPoints.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * EECSettings.cfgSaintAscendPointPenalty.Value);
                }
            }
            else if (ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                return (int)(orig(self) * EECSettings.cfgRivuletShortCycleBonus.Value);
            }
            return orig(self);
        }

        public static int BonusNeuronDeliveryChallenge_Points(On.Expedition.NeuronDeliveryChallenge.orig_Points orig, NeuronDeliveryChallenge self)
        {
            if (ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                return (int)(orig(self) * EECSettings.cfgRivuletShortCycleBonus.Value);
            }
            return orig(self);
        }

        public static int BonusPearlDeliveryChallenge_Points(On.Expedition.PearlDeliveryChallenge.orig_Points orig, PearlDeliveryChallenge self)
        {
            if (ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                return (int)(orig(self) * EECSettings.cfgRivuletShortCycleBonus.Value);
            }
            return orig(self);
        }

        public static int BonusPearlHoardChallenge_Points(On.Expedition.PearlHoardChallenge.orig_Points orig, PearlHoardChallenge self)
        {
            if (ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                return (int)(orig(self) * EECSettings.cfgRivuletShortCycleBonus.Value);
            }
            return orig(self);
        }

        public static int BonusVistaChallenge_Points(On.Expedition.VistaChallenge.orig_Points orig, VistaChallenge self)
        {
            if (ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                return (int)(orig(self) * EECSettings.cfgRivuletShortCycleBonus.Value);
            }
            return orig(self);
        }

        public static int PearlDeliveryChallenge_RegionPoints(On.Expedition.PearlDeliveryChallenge.orig_RegionPoints orig, PearlDeliveryChallenge self)
        {
            if (self.region == "MS")
            {
                return 35;
            }
            else if (self.region == "LM")
            {
                return 10;
            }
            return orig(self);
        }
    }
}