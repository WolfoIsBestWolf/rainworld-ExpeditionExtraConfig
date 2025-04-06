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
    public class Monk
    {
        public static void Start()
        {
            On.Expedition.AchievementChallenge.Points += AchievementChallenge_Points;
            On.Expedition.CycleScoreChallenge.Points += CycleScoreChallenge_Points;
            On.Expedition.GlobalScoreChallenge.Points += GlobalScoreChallenge_Points;
            On.Expedition.HuntChallenge.Points += HuntChallenge_Points;
        }

        private static int HuntChallenge_Points(On.Expedition.HuntChallenge.orig_Points orig, HuntChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Yellow)
            {
                if (WConfig.cfgMonk_CombatScore.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        private static int GlobalScoreChallenge_Points(On.Expedition.GlobalScoreChallenge.orig_Points orig, GlobalScoreChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Yellow)
            {
                if (WConfig.cfgMonk_CombatScore.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        private static int CycleScoreChallenge_Points(On.Expedition.CycleScoreChallenge.orig_Points orig, CycleScoreChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Yellow)
            {
                if (WConfig.cfgMonk_CombatScore.Value)
                {
                    return (int)(orig(self) * 1.1f);
                }
            }
            return orig(self);
        }

        private static int AchievementChallenge_Points(On.Expedition.AchievementChallenge.orig_Points orig, AchievementChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.Yellow)
            {
                if (WConfig.cfgMonk_CombatScore.Value)
                {
                    if (self.CombatRequired())
                    {
                        return (int)(orig(self) * 1.1f);
                    }
                   
                }
            }
            return orig(self);
        }
    }
}