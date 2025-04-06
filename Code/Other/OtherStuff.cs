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
                //x => x.MatchStfld("Hud.TextPromt", "pausedWarningText"),
                //x => x.MatchBr(out _),
                //x => x.MatchLdarg(2),
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
               UnityEngine.Debug.Log("ExpeditionConfig: Pause IL Hook Success");
            }
            else
            {
               UnityEngine.Debug.Log("ExpeditionConfig: Pause IL Hook Failed");
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