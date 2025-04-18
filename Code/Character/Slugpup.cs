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
using System.Web.UI.WebControls;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace ExpeditionExtraConfig
{
    public class Slugpup
    {
 
        public static void Start()
        {
            //Taken from Pups4Everyone, this shit looks like Magic      
            //Hook hook = new Hook(typeof(StoryGameSession).GetProperty("slugPupMaxCount", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(ExpeditionExtraConfig).GetMethod("StoryGameSession_slugPupMaxCount_get", BindingFlags.Static | BindingFlags.Public));
            IL.AbstractRoom.RealizeRoom += AbstractRoom_RealizeRoom;
            IL.Expedition.AchievementChallenge.Generate += AllowMother_AchievementChallenge;

            Hook hook = new Hook(typeof(StoryGameSession).GetProperty("slugPupMaxCount", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Slugpup).GetMethod("StoryGameSession_slugPupMaxCount_get", BindingFlags.Static | BindingFlags.Public));
        }

        public delegate int orig_slugPupMaxCount(StoryGameSession self);
        public static int StoryGameSession_slugPupMaxCount_get(Slugpup.orig_slugPupMaxCount orig, StoryGameSession self)
        {
            //UnityEngine.Debug.Log("Original Pup Limit" + orig(self));
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ModManager.MSC)
                {
                    if (orig(self) < 2)
                    {
                        if (WConfig.cfgPupsSpawn.Value)
                        {
                            var slugName = Custom.rainWorld.progression.PlayingAsSlugcat;
                            if (WConfig.cfgPupsSpawnNonDefault.Value)
                            {
                                //UnityEngine.Debug.Log("Expedition Pup limit 2");
                                return 2;
                            }
                            else if (slugName == SlugcatStats.Name.White || slugName == SlugcatStats.Name.Yellow || slugName == SlugcatStats.Name.Red || slugName == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                            {
                                //UnityEngine.Debug.Log("Expedition Pup limit 2");
                                return 2;
                            }
                        }
                    }
                }
            }
            return orig(self);
        }



        private static void AbstractRoom_RealizeRoom(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcI4(25)))
            {
                c.EmitDelegate<System.Func<int, int>>((pupCount) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgPupsSpawnFrequently.Value)
                        {
                            return 10;
                        }
                    }                  
                    return pupCount;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Sooner Pups for all");
            }
        }

        public static void AllowMother_AchievementChallenge(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(1));
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(1)))
            {
                c.EmitDelegate<System.Func<List<WinState.EndgameID>, List<WinState.EndgameID>>>((list) =>
                {
                    if (WConfig.cfgPupsMotherAchievement.Value && WConfig.cfgPupsSpawn.Value)
                    {
                        var slug = ExpeditionData.slugcatPlayer;
                        if (WConfig.cfgPupsSpawnNonDefault.Value || slug == SlugcatStats.Name.Yellow || slug == SlugcatStats.Name.White || slug == SlugcatStats.Name.Red || slug == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                        {
                            UnityEngine.Debug.Log("Adding Mother");
                            //UnityEngine.Debug.Log(list.Count);
                            list.Add(MoreSlugcatsEnums.EndgameID.Mother);
                        }
                    }
                    return list;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: AllowMother_AchievementChallenge");
            }
            else
            {
                UnityEngine.Debug.Log("EEC IL FAILED : AllowMother");
            }
        }
    }
}