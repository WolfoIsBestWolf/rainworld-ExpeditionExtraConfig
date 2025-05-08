using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;
using System.IO;
using Modding.Expedition;
using Menu;
using Mono.Cecil.Cil;
using System;
using static Expedition.ExpeditionProgression;

namespace ExpeditionExtraConfig
{
    public class NewPerks
    {

        public static void Start()
        {
            CustomPerks.Register(new CustomPerk[] 
            {
                new Perk_FireSpear(),
                new Perk_MonkGate(),
                new Perk_Passages(),
                new Perk_SpearWall(),
                new Perk_EnergyCell(),
                new Perk_Pups(),
            });

            IL.Room.Loaded += RoomSpawn; 
            On.Expedition.ExpeditionProgression.UnlockName += ExpeditionProgression_UnlockName;

            On.Menu.ExpeditionMenu.ValidateQuestRewards += ManageUnlocks;
        }

        private static void ManageUnlocks(On.Menu.ExpeditionMenu.orig_ValidateQuestRewards orig, ExpeditionMenu self)
        {
            if (!WConfig.cfgNewPerksForceUnlock.Value)
            {
                //Remove all incase they were Permamently gotten through the config.
                //Then recheck if actually earned
                //Clunky but such it is
                ExpeditionData.unlockables.Remove("unl-eec-EnergyCell");
                ExpeditionData.unlockables.Remove("unl-eec-FireSpear");
                ExpeditionData.unlockables.Remove("unl-eec-MonkGate");
                ExpeditionData.unlockables.Remove("unl-eec-Passages");
                ExpeditionData.unlockables.Remove("unl-eec-Pups");
                //ExpeditionData.unlockables.Remove("unl-eec-SpearWall");
            }            
            orig(self);
            //Seems to work fine as a "Validator"
            if (!WConfig.cfgNewPerksForceUnlock.Value)
            {
                ExpeditionProgression.EvaluateExpedition(new ExpeditionProgression.WinPackage
                {
                    points = 0,
                    challenges = new List<Challenge>(),
                    slugcat = MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Slugpup,
                });
            }
        }

     
        public static void RemoveAllModdedQuests()
        {
            var modQuests = ExpeditionProgression.customQuests["ExpeditionExtraConfig"];
            foreach (var mod in modQuests)
            {
                ExpLog.Log(mod.key);
                ExpeditionData.completedQuests.Remove(mod.key);
            }
        }


        public static void RoomSpawn(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("unl-lantern")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<string, Room, string>>((unlock, room) =>
                {
                    if (unlock == "unl-eec-EnergyCell")
                    {
                        WorldCoordinate pos12 = new WorldCoordinate(room.abstractRoom.index, room.shelterDoor.playerSpawnPos.x, room.shelterDoor.playerSpawnPos.y, 0);
                        AbstractPhysicalObject abstractPhysicalObject13 = new AbstractPhysicalObject(room.world, MoreSlugcatsEnums.AbstractObjectType.EnergyCell, null, pos12, room.game.GetNewID());
                        room.abstractRoom.entities.Add(abstractPhysicalObject13);
                        abstractPhysicalObject13.Realize();
                    }
                    else if(unlock == "unl-eec-ExplosiveSpear")
                    {
                        WorldCoordinate pos12 = new WorldCoordinate(room.abstractRoom.index, room.shelterDoor.playerSpawnPos.x, room.shelterDoor.playerSpawnPos.y, 0);
                        AbstractSpear abstractSpear2 = new AbstractSpear(room.world, null, pos12, room.game.GetNewID(), false);
                        abstractSpear2.explosive = true;
                        room.abstractRoom.entities.Add(abstractSpear2);
                        abstractSpear2.Realize();
                    }
                    return unlock;
                });
            }
            else
            {
                UnityEngine.Debug.Log("EEC: Spawn NewPerks fail");
            }
        }

        private static string ExpeditionProgression_UnlockName(On.Expedition.ExpeditionProgression.orig_UnlockName orig, string key)
        {
            return orig(key);
        }

    }
}