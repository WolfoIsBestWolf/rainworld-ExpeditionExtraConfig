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
    public class OtherStuff
    {
        public static void PauseMenu_ctor(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("RainWorldGame", "clock"),
                x => x.MatchLdcI4(1200)))
            {
                //c.Index += 1;
                c.EmitDelegate<Func<int, int>>((karma) =>
                {
                    if (WConfig.cfgPauseWarning.Value)
                    {
                        return 300;
                    }
                    return karma;
                });
               Debug.Log("ExpeditionConfig: Pause IL Hook Success");
            }
            else
            {
               Debug.Log("ExpeditionConfig: Pause IL Hook Failed");
            }

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdfld("DeathPersistentSaveData", "karma")))
            {
                //c.Index += 1;
                c.EmitDelegate<Func<int, int>>((karma) =>
                {
                    if (WConfig.cfgRemovePermaDeath.Value)
                    {
                        return 1;
                    }
                    return karma;
                });
                Debug.Log("cfgRemovePermaDeath no spooky message");
            }
            else
            {
                Debug.Log("cfgRemovePermaDeath no spooky message fail");
            }
        }

        public static void BiggerMapExped(On.HUD.Map.orig_ctor orig, HUD.Map self, HUD.HUD hud, HUD.Map.MapData mapData)
        {
            orig(self, hud, mapData);
            if (Custom.rainWorld.ExpeditionMode)
            {
                //UnityEngine.Debug.Log(self.DiscoverResolution);
                self.DiscoverResolution = 7f * WConfig.cfgMapRevealRadius.Value;
                //UnityEngine.Debug.Log(self.DiscoverResolution);
            }

        }

        public static void AlwaysPlayMusic(On.Music.MusicPlayer.orig_GameRequestsSong orig, Music.MusicPlayer self, MusicEvent musicEvent)
        {
            if (WConfig.cfgMusicPlayMore.Value)
            {
                self.hasPlayedASongThisCycle = false;
                musicEvent.cyclesRest = 0;
          
            }     
            orig(self, musicEvent);
        }

    }
}