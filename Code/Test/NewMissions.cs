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
using System.Reflection;

namespace ExpeditionExtraConfig
{
    public class NewMissions
    {
        public static void Start()
        {
            On.Expedition.ExpeditionProgression.ParseMissionFiles += ExpeditionProgression_ParseMissionFiles;
            //On.Expedition.ExpeditionProgression.MissionFromJson += ExpeditionProgression_MissionFromJson;

            IL.Room.Loaded += AddDepthsScript;
            On.Expedition.DepthsFinishScript.Update += DepthsFinishScript_Update;
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

     
        private static void ExpeditionProgression_ParseMissionFiles(On.Expedition.ExpeditionProgression.orig_ParseMissionFiles orig)
        {
            orig();
            if (!WConfig.cfgTestingMission.Value)
            {
                return;
            }

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

            /*foreach (var region in Custom.rainWorld.progression.regionNames)
            {
                var vistaRegion = ChallengeTools.VistaLocations[region];
                foreach (var room in vistaRegion)
                {
                    string args = "\"VistaChallenge~"+ region + "><"+ room.Key + "><"+ room.Value.x + "><"+ room.Value.y + "><0><0><0\",";
                    Debug.Log(args);
                }
            }*/

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

            var MissionList = new List<ExpeditionProgression.Mission>() { vistaMission, vistaMissionW };

            ExpeditionProgression.missionList.Add(vistaMission);
            ExpeditionProgression.missionList.Add(vistaMissionW);
            if (!ExpeditionProgression.customMissions.ContainsKey("VistaTesting"))
            {
                ExpeditionProgression.customMissions.Add("VistaTesting", MissionList);
            }
           
        }
    }
}