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
    public class UnlockAll
    {
        public static bool added = false;
        public static void Add()
        {
            added = true;
            UnityEngine.Debug.Log("ADDING UNLOCK ALL HOOKS");
            IL.Expedition.ExpeditionProgression.BurdenDescription += NoNeedForUnlocks;
            IL.Expedition.ExpeditionProgression.BurdenManualDescription += NoNeedForUnlocks;
             

            IL.Expedition.ExpeditionProgression.UnlockDescription += NoNeedForUnlocks;
            IL.Expedition.ExpeditionProgression.UnlockName += NoNeedForUnlocks;
            

            IL.Menu.ExpeditionManualDialog.PerkManualDescription += NoNeedForUnlocks;
            IL.Menu.MusicTrackButton.ctor += NoNeedForUnlocks;
            IL.Menu.PerkButton.ctor += NoNeedForUnlocks;

            IL.Menu.UnlockDialog.SetupBurdensPage += NoNeedForUnlocks;
            IL.Menu.UnlockDialog.SetupPerksPage += NoNeedForUnlocks;
         
            IL.Menu.UnlockDialog.Update += NoNeedForUnlocks;

            IL.Expedition.ExpeditionProgression.UnlockSprite += NoNeedForUnlocks2;
            IL.Menu.UnlockDialog.SetUpBurdenDescriptions += NoNeedForUnlocks4;

            On.Menu.UnlockDialog.TogglePerk += UnlockDialog_TogglePerk;
            On.Expedition.ExpeditionProgression.MissionAvailable += ExpeditionProgression_MissionAvailable;
            On.Expedition.ExpeditionProgression.MissionRequirements += ExpeditionProgression_MissionRequirements;
        }

        private static string ExpeditionProgression_MissionRequirements(On.Expedition.ExpeditionProgression.orig_MissionRequirements orig, string key)
        {
            if (WConfig.cfgUnlockAll.Value)
            {
                return "";
            }
            return orig(key);
        }

        public static void Remove()
        {
            added = false;
            UnityEngine.Debug.Log("REMOVING UNLOCK ALL HOOKS"); 
            IL.Expedition.ExpeditionProgression.BurdenDescription -= NoNeedForUnlocks;
            IL.Expedition.ExpeditionProgression.BurdenManualDescription -= NoNeedForUnlocks;


            IL.Expedition.ExpeditionProgression.UnlockDescription -= NoNeedForUnlocks;
            IL.Expedition.ExpeditionProgression.UnlockName -= NoNeedForUnlocks;


            IL.Menu.ExpeditionManualDialog.PerkManualDescription -= NoNeedForUnlocks;
            IL.Menu.MusicTrackButton.ctor -= NoNeedForUnlocks;
            IL.Menu.PerkButton.ctor -= NoNeedForUnlocks;

            IL.Menu.UnlockDialog.SetupBurdensPage -= NoNeedForUnlocks;
            IL.Menu.UnlockDialog.SetupPerksPage -= NoNeedForUnlocks;

            IL.Menu.UnlockDialog.Update -= NoNeedForUnlocks;

            IL.Expedition.ExpeditionProgression.UnlockSprite -= NoNeedForUnlocks2;
            IL.Menu.UnlockDialog.SetUpBurdenDescriptions -= NoNeedForUnlocks4;

            On.Menu.UnlockDialog.TogglePerk -= UnlockDialog_TogglePerk;
            On.Expedition.ExpeditionProgression.MissionAvailable -= ExpeditionProgression_MissionAvailable;
            On.Expedition.ExpeditionProgression.MissionRequirements -= ExpeditionProgression_MissionRequirements;
        }

        private static bool ExpeditionProgression_MissionAvailable(On.Expedition.ExpeditionProgression.orig_MissionAvailable orig, string key)
        {
            if (WConfig.cfgUnlockAll.Value)
            {
                return true;
            }
            return orig(key);
        }

        private static void UnlockDialog_TogglePerk(On.Menu.UnlockDialog.orig_TogglePerk orig, Menu.UnlockDialog self, string message)
        {
            if (WConfig.cfgUnlockAll.Value)
            {
                if (ExpeditionData.perkLimit < 8)
                {
                    ExpeditionData.perkLimit = 8;
                }
            }
            orig(self,message);
        }

        private static void NoNeedForUnlocks4(ILContext il)
        {
            ILCursor c = new(il);
            for (int i = 0; i < 4; i++)
            {
                c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Expedition.ExpeditionData", "unlockables"));
                if (c.TryGotoNext(MoveType.After,
                    x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Contains")))
                {
                    c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                    {
                        if (WConfig.cfgUnlockAll.Value)
                        {
                            return true;
                        }
                        return karma;
                    });

                    UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks4 IL Success");
                }
                else
                {
                    UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks4 IL Failed");
                    Debug.Log(c);
                }
            }
        }

        private static void NoNeedForUnlocks2(ILContext il)
        {
            ILCursor c = new(il);
            for (int i = 0; i < 2; i++)
            {
                c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Expedition.ExpeditionData", "unlockables"));
                if (c.TryGotoNext(MoveType.After,
                    x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Contains")))
                {
                    c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                    {
                        if (WConfig.cfgUnlockAll.Value)
                        {
                            return true;
                        }
                        return karma;
                    });

                    UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks IL Success");
                }
                else
                {
                    UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks IL Failed");
                    Debug.Log(c);
                }
            }
        }
    
        private static void NoNeedForUnlocks(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("Expedition.ExpeditionData", "unlockables"));
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Contains")))
                {
                c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                {
                    if (WConfig.cfgUnlockAll.Value)
                    {
                        return true;
                    }
                    return karma;
                });

                UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks IL Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: NoNeedForUnlocks IL Failed");
                Debug.Log(c);
            }
        }
    }
}