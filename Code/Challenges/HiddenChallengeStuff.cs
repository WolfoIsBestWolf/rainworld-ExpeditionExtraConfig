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
    public class HiddenChallengeStuff
    {
        public static void Start()
        {
            UndoRemovedHiddenChallenges();
            // On.Expedition.Challenge.CanBeHidden += Challenge_CanBeHidden;
            On.Expedition.ChallengeOrganizer.RandomChallenge += ChallengeOrganizer_RandomChallenge;
        }

        private static Challenge ChallengeOrganizer_RandomChallenge(On.Expedition.ChallengeOrganizer.orig_RandomChallenge orig, bool hidden)
        {
            if (WConfig.cfgHidden_Filter.Value)
            {
                return orig(false);
            }
            return orig(hidden);
        }

        /*private static bool Challenge_CanBeHidden(On.Expedition.Challenge.orig_CanBeHidden orig, Challenge self)
        {
            if (self is AchievementChallenge)
            {
                return WConfig.cfgHidden_Passage.Value;
            }
            else if (self is EchoChallenge)
            {
                return WConfig.cfgHidden_Echo.Value;
            }
            else if (self is HuntChallenge)
            {
                return WConfig.cfgHidden_Hunting.Value;
            }
            else if (self is ItemHoardChallenge)
            {
                return WConfig.cfgHidden_ItemCollecting.Value;
            }
            else if (self is NeuronDeliveryChallenge)
            {
                return WConfig.cfgHidden_NeuronDelivery.Value;
            }
            else if (self is PearlDeliveryChallenge)
            {
                return WConfig.cfgHidden_PearlDelivery.Value;
            }
            else if (self is PearlHoardChallenge)
            {
                return WConfig.cfgHidden_PearlHoard.Value;
            }
            else if (self is VistaChallenge)
            {
                return WConfig.cfgHidden_Vista.Value;
            }
            else if (self is PinChallenge)
            {
                return WConfig.cfgHidden_Pin.Value;
            }
            else if (self is CycleScoreChallenge)
            {
                return WConfig.cfgHidden_CycleScore.Value;
            }
            else if (self is GlobalScoreChallenge)
            {
                return WConfig.cfgHidden_GlobalScore.Value;
            }

            return orig(self);
        }*/

        public static void UndoRemovedHiddenChallenges()
        {
            On.Expedition.PearlDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHidden_Delivery.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.PearlHoardChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHidden_PearlHoard.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.NeuronDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHidden_Delivery.Value)
                {
                    return true;
                }
                return orig(self);
            };

            On.Expedition.PearlHoardChallenge.Generate += (orig, self) =>
            {
                if (WConfig.cfgHidden_PearlHoard.Value)
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
                if (WConfig.cfgHidden_Delivery.Value)
                {
                    Challenge temp = orig(self);
                    if (temp.hidden)
                    {
                        (temp as NeuronDeliveryChallenge).neurons = Mathf.RoundToInt(UnityEngine.Random.Range(1f, Mathf.Lerp(1f, 2f, Mathf.InverseLerp(0.4f, 1f, ExpeditionData.challengeDifficulty))));
                        return temp;
                    }
                }
                return orig(self);
            };
        }
 
     }
}