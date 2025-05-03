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
using System.Runtime.Serialization;
using System.IO;
using UnityEngine.UI;

namespace ExpeditionExtraConfig
{
    public class NewMissions
    {
        public static void Start()
        {
         
            //On.Expedition.ExpeditionProgression.MissionFromJson += ExpeditionProgression_MissionFromJson;

            IL.Room.Loaded += AddDepthsScript;
            On.Expedition.DepthsFinishScript.Update += DepthsFinishScript_Update;
            On.Expedition.ExpeditionProgression.ParseMissionFiles += CorrectMissionCategoryName;

            // On.HUD.RainMeter.ctor += RainMeter_ctor;

            On.Expedition.ExpeditionProgression.ParseMissionFiles += VistaMission;

            IL.World.SpawnGhost += World_SpawnGhost;
            On.World.CheckForRegionGhost += World_CheckForRegionGhost;
        }

        private static bool World_CheckForRegionGhost(On.World.orig_CheckForRegionGhost orig, SlugcatStats.Name slugcatIndex, string regionString)
        {
            if (Custom.rainWorld.ExpeditionMode && ExpeditionData.activeMission == "EEC_Future1")
            {
                if (regionString == "SL")
                {
                    Debug.Log("SL Echo on Mission 2");
                    return true;
                }
            }
            return orig(slugcatIndex, regionString);
        }

        private static void World_SpawnGhost(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("MoreSlugcats.MoreSlugcatsEnums/GhostID", "SL")))
            {
                c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("RainWorldGame", "get_StoryCharacter"));
                c.EmitDelegate<Func<SlugcatStats.Name, SlugcatStats.Name>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode && ExpeditionData.activeMission == "EEC_Future1")
                    {
                        Debug.Log("SL Echo on Mission");
                        return MoreSlugcatsEnums.SlugcatStatsName.Saint;
                    }
                    return room;
                });
            }
            else
            {
                Debug.Log("Failed World_SpawnGhost");
            }
        }

        private static void RainMeter_ctor(On.HUD.RainMeter.orig_ctor orig, HUD.RainMeter self, HUD.HUD hud, FContainer fContainer)
        {
            orig(self, hud, fContainer);
            if (ModManager.MSC && (hud.owner as Player).SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint && hud.map.RegionName != "HR")
            {
                //Not visible for Saint challenge
            }
        }

        private static void CorrectMissionCategoryName(On.Expedition.ExpeditionProgression.orig_ParseMissionFiles orig)
        {
            orig();
            Debug.Log("Fix Mission Category");
            var mod = ModManager.GetModById("ExpeditionExtraConfig");
            string dir = Path.GetFileName(mod.path).ToLowerInvariant();
            if (ExpeditionProgression.customMissions.ContainsKey(dir))
            {
                if (!ExpeditionProgression.customMissions.ContainsKey(mod.id))
                {
                    var pair = ExpeditionProgression.customMissions[dir];
                    ExpeditionProgression.customMissions.Add(mod.id, pair);
                }
                ExpeditionProgression.customMissions.Remove(dir);
            }

        }


        private static void AddDepthsScript(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdstr("SB_A14")))
            {
                c.EmitDelegate<Func<string, string>>((room) =>
                {
                    //Debug.Log(room);
                    if (room == "HR_I01")
                    {
                        return "SB_A14";
                    }
                    return room;
                });
            }
            else
            {
                Debug.Log("Failed AddDepthsScript");
            }

        }

        private static void DepthsFinishScript_Update(On.Expedition.DepthsFinishScript.orig_Update orig, DepthsFinishScript self, bool eu)
        {
            orig(self, eu);
            if (self.room != null)
            {
                if (self.room.shortCutsReady && self.room.abstractRoom.name == "HR_I01")
                {
                    self.room.shortcuts[3].shortCutType = ShortcutData.Type.DeadEnd;
                    self.Destroy();
                   /* for (int i = 0; i < self.room.shortcuts.Length; i++)
                    {
                        Debug.Log(i);
                        Debug.Log(self.room.shortcuts[i].connection.startCoord.ResolveRoomName());
                        Debug.Log(self.room.shortcuts[i].connection.destinationCoord.ResolveRoomName());
                        Debug.Log(self.room.shortcuts[i].shortCutType);
                        Debug.Log(self.room.shortcuts[i].StartTile);
                        Debug.Log(self.room.shortcuts[i].DestTile);
                        Debug.Log(self.room.shortcuts[i].destNode);
                        Debug.Log("");
                        //self.room.shortcuts[i].shortCutType = ShortcutData.Type.DeadEnd;
                    }*/
                }
            }
        }

     
        private static void VistaMission(On.Expedition.ExpeditionProgression.orig_ParseMissionFiles orig)
        {
            orig();
            //We could, add then remove the hook ig 
            #region Rubicon
            /*ExpeditionProgression.Mission misRubicon = new ExpeditionProgression.Mission
            {
                key = "EEC_Rubicon",
                name = "ReRubicon",
                slugcat = "Saint",
                challenges = new List<Challenge>(),
                requirements = new List<string>() { "bur-pursued", "bur-hunted" },
                den = "HR_C01"
            };
            var temp1 = new GlobalScoreChallenge();
            temp1.FromString("0><666><0><0><0");
            misRubicon.challenges.Add(temp1);

            var temp2 = new HuntChallenge();
            temp2.FromString("FireBug><6><0><0><0><0");
            misRubicon.challenges.Add(temp2);
            var temp3 = new VistaChallenge();
            temp3.FromString("HR><HR_L02><750><3525><0><0><0><0><0");
            misRubicon.challenges.Add(temp3);
            */

            #endregion
            if (!WConfig.cfgTestingMission.Value)
            {
                return;
            }
            ExpeditionProgression.Mission vistaMission = new ExpeditionProgression.Mission
            {
                key = "AllVistas_All",
                name = "All Vistas",
                slugcat = "White",
                challenges = new List<Challenge>(),
                requirements = new List<string>(),
                den = ""
            };
            ExpeditionProgression.Mission vistaMissionW = new ExpeditionProgression.Mission
            {
                key = "AllVistas_Watcher",
                name = "All Vistas Watcher",
                slugcat = "Watcher",
                challenges = new List<Challenge>(),
                requirements = new List<string>(),
                den = ""
            };
            foreach (string region in Custom.rainWorld.progression.regionNames)
            {
                if (region.StartsWith("W"))
                {
                    foreach (var room in ChallengeTools.VistaLocations[region])
                    {
                        var a = new VistaChallenge();
                        a.FromString(region + "><" + room.Key + "><" + room.Value.x + "><" + room.Value.y + "><0><0><0");
                        vistaMissionW.challenges.Add(a);
                    }
                }
                else
                {
                    foreach (var room in ChallengeTools.VistaLocations[region])
                    {
                        var a = new VistaChallenge();
                        a.FromString(region + "><" + room.Key + "><" + room.Value.x + "><" + room.Value.y + "><0><0><0");
                        vistaMission.challenges.Add(a);
                    }
                }
            }

  
            ExpeditionProgression.missionList.Add(vistaMission);
            ExpeditionProgression.missionList.Add(vistaMissionW);
            if (ExpeditionProgression.customMissions.ContainsKey("ExpeditionExtraConfig"))
            {
                if (ExpeditionProgression.customMissions["ExpeditionExtraConfig"].Count < 5)  
                {
                        ExpeditionProgression.customMissions["ExpeditionExtraConfig"].Add(vistaMission);
                        ExpeditionProgression.customMissions["ExpeditionExtraConfig"].Add(vistaMissionW);
                }
            }

        }
    }
}