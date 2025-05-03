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

namespace ExpeditionExtraConfig
{
    public class NewPerks
    {

        public static void Start()
        {
            On.Menu.UnlockDialog.TogglePerk += BlockPerks;

        
            CustomPerks.Register(new CustomPerk[] 
            {
                new Perk_ExplosiveSpear(),
                new Perk_EnergyCell(),
                new Perk_MonkGate(),
                new Perk_Passages(),
                new Perk_Pups(),
                //new Perk_SpearWall(),

            });

            IL.Room.Loaded += RoomSpawn; 
            On.Expedition.ExpeditionProgression.UnlockName += ExpeditionProgression_UnlockName;
          
            
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

        public static void BlockPerks(On.Menu.UnlockDialog.orig_TogglePerk orig, UnlockDialog self, string message)
        {
            if (!ModManager.MSC || ExpeditionData.slugcatPlayer == SlugcatStats.Name.White && WConfig.cfgSurvivor_StartWithPups.Value || ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Gourmand && WConfig.cfgGourmand_StartWithPups.Value)
            {
                if (message == "unl-eec-Pups")
                {
                    self.PlaySound(SoundID.MENU_Error_Ping);
                    return;
                }
            }
            else if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && WConfig.cfgRivuletBall.Value)
            {
                if (message == "unl-eec-EnergyCell")
                {
                    self.PlaySound(SoundID.MENU_Error_Ping);
                    return;
                }
            }
            orig(self, message);
        }


       


    }
}