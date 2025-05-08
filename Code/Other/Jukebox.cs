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
using Menu.Remix;
using Menu;
using UnityEngine.UIElements;

namespace ExpeditionExtraConfig
{
    public class Jukebox
    {
        
        public static bool Autoplay = false;
        public static SymbolButton? autoplayButton;

        public static void Start()
        {
            On.Menu.MusicTrackButton.ctor += UnlockSongs;
            On.Menu.MusicTrackContainer.Singal += WrapAround;

            On.Menu.ExpeditionJukebox.Singal += ExpeditionJukebox_Singal;
            IL.Menu.ExpeditionJukebox.Update += ExpeditionJukebox_Update;

            On.Menu.ExpeditionJukebox.ctor += AddAutoPlayButton;
            On.Menu.ExpeditionJukebox.Update += ExpeditionJukebox_Update1;
            On.Menu.ExpeditionJukebox.GrafUpdate += ExpeditionJukebox_GrafUpdate;
        }

        private static void ExpeditionJukebox_GrafUpdate(On.Menu.ExpeditionJukebox.orig_GrafUpdate orig, ExpeditionJukebox self, float timeStacker)
        {
            orig(self, timeStacker);
            if (autoplayButton != null)
            {
                autoplayButton.symbolSprite.color = (Autoplay ? Menu.Menu.MenuRGB(Menu.Menu.MenuColors.MediumGrey) : Menu.Menu.MenuRGB(Menu.Menu.MenuColors.VeryDarkGrey));
            }
        }

        private static void ExpeditionJukebox_Update1(On.Menu.ExpeditionJukebox.orig_Update orig, ExpeditionJukebox self)
        {
            orig(self);
            if (autoplayButton != null && (autoplayButton.Selected || autoplayButton.MouseOver))
            {
                self.infoLabel.text = T.Translate("Jukebox_Autoplay_Desc");
                self.infoLabelFade = 1f;
            }
        }

        private static void AddAutoPlayButton(On.Menu.ExpeditionJukebox.orig_ctor orig, ExpeditionJukebox self, ProcessManager manager)
        {
            orig(self, manager);
            if (WConfig.cfgJukeboxAdditions.Value)
            {
                autoplayButton = new SymbolButton(self, self.pages[0], "medianext", "AUTOPLAY", new Vector2(820f, 70f));
                autoplayButton.size = new Vector2(60f, 60f);
                autoplayButton.roundedRect.size = self.shuffleButton.size;
                self.pages[0].subObjects.Add(autoplayButton);
            }
       
        }

        private static void ExpeditionJukebox_Update(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("Menu.ExpeditionJukebox", "shuffle")))
            {
                c.EmitDelegate<Func<ExpeditionJukebox, ExpeditionJukebox>>((jukeBox) =>
                {
                    if (Autoplay)
                    {
                        jukeBox.manager.musicPlayer.FadeOutAllSongs(0f);
                        jukeBox.NextTrack(false);
                        jukeBox.seekBar.SetProgress(0f);
                        jukeBox.currentSong.label.text = ExpeditionProgression.TrackName(jukeBox.songList[jukeBox.selectedTrack]);
                        jukeBox.pendingSong = 1;
                        jukeBox.trackContainer.GoToPlayingTrackPage();
                    }
                    return jukeBox;
                });
            }
            else
            {
                Debug.Log("IL Failed Jukebox autoplay");
            }
        }


        private static void ExpeditionJukebox_Singal(On.Menu.ExpeditionJukebox.orig_Singal orig, ExpeditionJukebox self, MenuObject sender, string message)
        {
            orig(self, sender, message);
            if (message == "AUTOPLAY")
            {
                self.shuffle = false;
                self.repeat = false;
                Autoplay = !Autoplay;
                if (Autoplay)
                {
                    self.PlaySound(SoundID.MENU_Player_Join_Game);
                }
                else
                {
                    self.PlaySound(SoundID.MENU_Button_Press_Init);
                }
            }
            else if (message == "SHUFFLE")
            {
                Autoplay = false; 
                self.repeat = false;
            }
            else if (message == "REPEAT")
            {
                Autoplay = false; 
                self.shuffle = false;
            }
        }

        private static void WrapAround(On.Menu.MusicTrackContainer.orig_Singal orig, MusicTrackContainer self, MenuObject sender, string message)
        {
            if (WConfig.cfgJukeboxAdditions.Value)
            {
                if (message == "PAGEBACK")
                {
                    if (self.currentPage - 1 < 0)
                    {
                        self.currentPage = self.maxPages - 1;
                        self.SwitchPage();
                        return;
                    }
                }
                if (message == "PAGENEXT")
                {
                    //Debug.Log(self.maxPages);
                    if (self.currentPage + 2 > self.maxPages)
                    {
                        self.currentPage = 0;
                        self.SwitchPage();
                        return;
                    }
                }
            }
            
            orig(self, sender, message);
        }


        private static void UnlockSongs(On.Menu.MusicTrackButton.orig_ctor orig, Menu.MusicTrackButton self, Menu.Menu menu, Menu.MenuObject owner, string displayText, string singalText, Vector2 pos, Vector2 size, Menu.SelectOneButton[] buttonArray, int index)
        {
            //Debug.Log(singalText);
            orig(self, menu, owner, displayText, singalText, pos, size, buttonArray, index);
            if (singalText == "mus-1")
            {
                if (WConfig.cfgUnlockJukebox.Value)
                {
                    (menu as ExpeditionJukebox).demoMode = true;
                }
            }
        }

    }
}