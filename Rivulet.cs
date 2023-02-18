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
    public class Riv
    {
        public static int RivuletRainCycle(On.RainCycle.orig_GetDesiredCycleLength orig, RainCycle self)
        {
            if (self.world.game.rainWorld.ExpeditionMode && ModManager.MSC && (self.world.game.session as StoryGameSession).saveState.saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && !self.world.singleRoomWorld && !self.world.game.GetStorySession.saveState.miscWorldSaveData.pebblesEnergyTaken)
            {
                int num = orig(self);

                if (self.world.region.name == "MS")
                {
                    num = (int)(num / 0.33f); //Full Rain Cycles for MS
                }
                else if (self.world.region.name == "VS" || self.world.region.name == "UW" || self.world.region.name == "SH" || self.world.region.name == "SB" || self.world.region.name == "SL")
                {
                    num = (int)(num / 0.5f * EECSettings.cfgRivuletMultiRegional.Value);
                }
                else
                {
                    num = (int)(num / 0.33f * EECSettings.cfgRivuletMulti.Value);
                }
                return num;
            }
            return orig(self);
        }

        public static void RivuletShelterFailure(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("MiscWorldSaveData", "pebblesEnergyTaken"));
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("DeathPersistentSaveData", "altEnding")))
            {
                c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, RainCycle, bool>>((Check, self) =>
                {
                    if (self.world.game.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    return self.world.game.GetStorySession.saveState.deathPersistentSaveData.altEnding;
                });
                Debug.Log("ExpeditionExtraConfig: Rivulet PreCycleChance Hook Success");


                Debug.Log(c);
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdcR4(25)); Debug.Log(c);
                c.EmitDelegate<Func<float, float>>((fail) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return 100;
                    }
                    return fail;
                });
                Debug.Log(c);
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdcR4(40)); Debug.Log(c);
                c.EmitDelegate<Func<float, float>>((fail) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return 69;
                    }
                    return fail;
                }); Debug.Log(c);
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: Rivulet PreCycleChance Hook Failed");
            }
        }

        public static bool RivuletAllowPebblesBall(On.Expedition.ExpeditionGame.orig_IsMSCRoomScript orig, UpdatableAndDeletable item)
        {
            // || item is MSCRoomSpecificScript.LC_FINAL
            if (item is MSCRoomSpecificScript.RM_CORE_EnergyCell)
            {
                return false;
            }
            return orig(item);
        }


        public static Challenge RivuletFriendlyCycleScore(On.Expedition.CycleScoreChallenge.orig_Generate orig, CycleScoreChallenge self)
        {
            if (EECSettings.cfgRivuletShortCycles.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                Challenge a = orig(self);
                float multi = (EECSettings.cfgRivuletMulti.Value + EECSettings.cfgRivuletMultiRegional.Value) / 2;
                (a as CycleScoreChallenge).target = (int)(Math.Round((a as CycleScoreChallenge).target * multi / 5) * 5);
                return a;
            }
            return orig(self);
        }


    }
}