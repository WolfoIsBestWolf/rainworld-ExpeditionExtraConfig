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
    public class Score
    {
        public static void Start()
        {
            UndoRemovedHiddenChallenges();
            On.Expedition.PearlDeliveryChallenge.RegionPoints += BetterScorePearlDelivery;
            On.Menu.ExpeditionMenu.ExpeditionSetup += ExpeditionMenu_ExpeditionSetup;
        }



        private static void ExpeditionMenu_ExpeditionSetup(On.Menu.ExpeditionMenu.orig_ExpeditionSetup orig, Menu.ExpeditionMenu self)
        {
            orig(self);
 
            UnityEngine.Debug.Log("ExpeditionExtraConfig: ExpeditionMenu_ExpeditionSetup");
            ChallengeTools.echoScores[(int)MoreSlugcatsEnums.GhostID.MS] = 90;
            if (!ChallengeTools.achievementScores.ContainsKey(MoreSlugcatsEnums.EndgameID.Mother))
            {
                ChallengeTools.achievementScores.Add(MoreSlugcatsEnums.EndgameID.Mother, 65);
            }
        }

        public static void UndoRemovedHiddenChallenges()
        {
            On.Expedition.PearlDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDelveries.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.PearlHoardChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDelveries.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.NeuronDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDelveries.Value)
                {
                    return true;
                }
                return orig(self);
            };

            On.Expedition.PearlHoardChallenge.Generate += (orig, self) =>
            {
                if (WConfig.cfgHiddenDelveries.Value)
                {
                    Challenge temp = orig(self);
                    if (temp.hidden)
                    {
                        (temp as PearlHoardChallenge).amount = (int)Mathf.Lerp(2f, 3f, ExpeditionData.challengeDifficulty);
                        return temp;
                    }
                }
                return orig(self);
            };
            On.Expedition.NeuronDeliveryChallenge.Generate += (orig, self) =>
            {
                if (WConfig.cfgHiddenDelveries.Value)
                {
                    if (WConfig.cfgHiddenDelveries.Value)
                    {
                        Challenge temp = orig(self);
                        if (temp.hidden)
                        {
                            (temp as NeuronDeliveryChallenge).neurons = Mathf.RoundToInt(UnityEngine.Random.Range(1f, Mathf.Lerp(1f, 2f, Mathf.InverseLerp(0.4f, 1f, ExpeditionData.challengeDifficulty))));
                            return temp;
                        }
                    }
                }
                return orig(self);
            };
        }
 
        public static int BetterScorePearlDelivery(On.Expedition.PearlDeliveryChallenge.orig_RegionPoints orig, PearlDeliveryChallenge self)
        {
            if (self.region == "MS")
            {
                return 35;
            }
            else if (self.region == "LM")
            {
                return 10;
            }
            return orig(self);
        }
    }
}