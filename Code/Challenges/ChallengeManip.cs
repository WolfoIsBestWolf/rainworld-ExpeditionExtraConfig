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
            On.Expedition.PearlDeliveryChallenge.RegionPoints += Missing_MS_LM;
            On.Menu.ExpeditionMenu.ExpeditionSetup += ExpeditionMenu_ExpeditionSetup;
            On.Expedition.ChallengeTools.ParseCreatureSpawns += ChallengeTools_ParseCreatureSpawns;

            On.Expedition.HuntChallenge.Generate += LimitHunts_HuntChallenge_Generate;
            On.Expedition.ItemHoardChallenge.Points += ItemHoardChallenge_Points;
            On.Expedition.PearlHoardChallenge.Points += PearlHoardChallenge_Points;

            
            //On.Expedition.ChallengeTools.GenerateVistaLocations += ChallengeTools_GenerateVistaLocations;
            On.Expedition.Challenge.ValidForThisSlugcat += Challenge_ValidForThisSlugcat;

            On.Expedition.GlobalScoreChallenge.CreatureKilled += GlobalScoreChallenge_CreatureKilled;
            On.Expedition.CycleScoreChallenge.CreatureKilled += FixSomeCreaturesNotCounting_CycleScoreChallenge_CreatureKilled;
            
            On.Expedition.VistaChallenge.Points += VistaChallenge_Points;
            IL.Expedition.VistaChallenge.Generate += VistaChallenge_Generate;

            On.Expedition.ChallengeTools.AppendAdditionalCreatureSpawns += ChallengeTools_AppendAdditionalCreatureSpawns;

        }

        private static void VistaChallenge_Generate(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCall("SlugcatStats", "SlugcatStoryRegions")))
            {
                c.EmitDelegate<Func<List<string>, List<string>>>((list) =>
                {
                    if (WConfig.cfgVista_OE.Value)
                    {
                        if (ExpeditionData.slugcatPlayer == SlugcatStats.Name.White || ExpeditionData.slugcatPlayer == SlugcatStats.Name.Yellow)
                        {
                            list.Add("OE");
                        }
                    }
                    return list;
                });            
            }
            else
            {
                Debug.Log("ExpeditionExtraConfig: OE Vistas IL hook fail");
            }
        }

        private static int PearlHoardChallenge_Points(On.Expedition.PearlHoardChallenge.orig_Points orig, PearlHoardChallenge self)
        {
            if (WConfig.cfgHoardingBonus.Value)
            {
                if (self.amount == 4)
                {
                    return (int)(orig(self) * 1.15f);
                }
                else if (self.amount == 5)
                {
                    return (int)(orig(self) * 1.25f);
                }
            }
            return orig(self);
        }

        private static void ChallengeTools_ParseCreatureSpawns(On.Expedition.ChallengeTools.orig_ParseCreatureSpawns orig)
        {
            orig();
            if (WConfig.cfgHoardingBonus.Value)
            {
                ChallengeTools.creatureScores["SpitterSpider"] = 6;
                ChallengeTools.creatureScores["MotherSpider"] = 5;
                ChallengeTools.creatureScores["BrotherLongLegs"] = 17;
            }
            ChallengeTools.creatureScores["FireBug"] = 10;

            if (!ChallengeTools.creatureScores.ContainsKey("HRGuard"))
            {   
                ChallengeTools.creatureScores.Add("TempleGuard", 3);
                ChallengeTools.creatureScores.Add("HRGuard", 3);
            }
        }

        private static void GlobalScoreChallenge_CreatureKilled(On.Expedition.GlobalScoreChallenge.orig_CreatureKilled orig, GlobalScoreChallenge self, Creature crit, int playerNumber)
        {
            //MAYBE CAN STILL RUN ORIG SELF JUST LIKE MEH   

            if (self.completed || self.game == null || crit == null)
            {
                return;
            }
            CreatureTemplate.Type type = crit.abstractCreature.creatureTemplate.type;
            if (type != null && ChallengeTools.creatureScores.ContainsKey(type.value))
            {
                int points = ChallengeTools.creatureScores[type.value];
                self.score += points;
                ExpLog.Log(string.Concat(new string[]
                {
                    "Player ",
                    (playerNumber + 1).ToString(),
                    " killed ",
                    type.value,
                    " | +",
                    points.ToString()
                }));
            }
            self.UpdateDescription();
            if (self.score >= self.target)
            {
                self.score = self.target;
                self.CompleteChallenge();
            }
        }

        private static void FixSomeCreaturesNotCounting_CycleScoreChallenge_CreatureKilled(On.Expedition.CycleScoreChallenge.orig_CreatureKilled orig, CycleScoreChallenge self, Creature crit, int playerNumber)
        {
            if (self.completed || self.game == null || crit == null)
            {
                return;
            }
            CreatureTemplate.Type type = crit.abstractCreature.creatureTemplate.type;
            if (type != null && ChallengeTools.creatureScores.ContainsKey(type.value))
            {
                int points = ChallengeTools.creatureScores[type.value];
                self.score += points;
                ExpLog.Log(string.Concat(new string[]
                {
                    "Player ",
                    (playerNumber + 1).ToString(),
                    " killed ",
                    type.value,
                    " | +",
                    points.ToString()
                }));
            }
            self.UpdateDescription();
            if (self.score >= self.target)
            {
                self.score = self.target;
                self.CompleteChallenge();
            }
        }
      


        private static bool Challenge_ValidForThisSlugcat(On.Expedition.Challenge.orig_ValidForThisSlugcat orig, Challenge self, SlugcatStats.Name slugcat)
        {
            if (self is VistaChallenge)
            {
                if (!WConfig.cfgVista_MS.Value && (self as VistaChallenge).region == "MS")
                {
                    return false;
                }
                if (!WConfig.cfgVista_LM.Value && (self as VistaChallenge).region == "LM")
                {
                    return false;
                }
                if (!WConfig.cfgVista_SS.Value && (self as VistaChallenge).region == "SS")
                {
                    return false;
                }
            }
            return orig(self, slugcat);
        }

  

        private static int VistaChallenge_Points(On.Expedition.VistaChallenge.orig_Points orig, VistaChallenge self)
        {
            //MC Regions that are hella out of the way
            if (WConfig.cfgVistaPearlScore.Value)
            {
                if (self.region == "MS" || self.region == "LC" || self.region == "OE" || self.region == "HR")
                {
                    return orig(self) + 20;
                }
                else if (self.region == "DM" || self.region == "RM" || self.region == "SS")
                {
                    return orig(self) + 10;
                }
            }
            return orig(self);
        }


        private static int ItemHoardChallenge_Points(On.Expedition.ItemHoardChallenge.orig_Points orig, ItemHoardChallenge self)
        {
            if (WConfig.cfgHoardingBonus.Value)
            {
                if (self.amount == 8)
                {
                    return orig(self) + 14;
                }
                if (self.amount >= 6)
                {
                    return orig(self) + 7;
                }
            }
            return orig(self);
        }

        private static Challenge LimitHunts_HuntChallenge_Generate(On.Expedition.HuntChallenge.orig_Generate orig, HuntChallenge self)
        {          
            var temp = orig(self) as HuntChallenge;

            if (WConfig.cfg_HuntChallengeLimitRot.Value != 15)
            {
                if (temp.target == CreatureTemplate.Type.DaddyLongLegs || temp.target == CreatureTemplate.Type.BrotherLongLegs)
                {
                    temp.amount = (int)Mathf.Lerp(1f, WConfig.cfg_HuntChallengeLimitRot.Value, (float)Math.Pow((double)ExpeditionData.challengeDifficulty, 2.5));
                }
            }
            if (WConfig.cfg_HuntChallengeLimitRed.Value < 15)
            {
                int score = ChallengeTools.creatureScores[temp.target.value];
                if (score >= 12)
                {
                    temp.amount = Math.Min(temp.amount, WConfig.cfg_HuntChallengeLimitRed.Value);
                }
            }
            if (WConfig.cfg_HuntChallengeLimit.Value < 15)
            {
                temp.amount = Math.Min(temp.amount, WConfig.cfg_HuntChallengeLimit.Value);
            }
            /*Debug.Log(temp.target);
            Debug.Log(temp.target.value);*/
            return temp;
        }


        public static void ChallengeTools_AppendAdditionalCreatureSpawns(On.Expedition.ChallengeTools.orig_AppendAdditionalCreatureSpawns orig)
        {
            orig();
            int num2;
            ChallengeTools.ExpeditionCreature item2 = new ChallengeTools.ExpeditionCreature
            {
                creature = DLCSharedEnums.CreatureTemplateType.StowawayBug,
                points = (ChallengeTools.creatureScores.TryGetValue(DLCSharedEnums.CreatureTemplateType.StowawayBug.value, out num2) ? num2 : 0),
                spawns = 1
            };
            if (ChallengeTools.creatureSpawns.ContainsKey(MoreSlugcatsEnums.SlugcatStatsName.Gourmand.value))
            {
                ChallengeTools.creatureSpawns[MoreSlugcatsEnums.SlugcatStatsName.Gourmand.value].Add(item2);
            }

        }


        private static void ExpeditionMenu_ExpeditionSetup(On.Menu.ExpeditionMenu.orig_ExpeditionSetup orig, Menu.ExpeditionMenu self)
        {
            orig(self);
 
            UnityEngine.Debug.Log("ExpeditionExtraConfig: ExpeditionMenu_ExpeditionSetup");
            
            if (WConfig.cfgRemoveRoboLock.Value)
            {
                ChallengeTools.echoScores[(int)MoreSlugcatsEnums.GhostID.MS]= 60;
            }
            else
            {
                ChallengeTools.echoScores[(int)MoreSlugcatsEnums.GhostID.MS] = 120;
            }
            if (!ChallengeTools.achievementScores.ContainsKey(MoreSlugcatsEnums.EndgameID.Mother))
            {
                ChallengeTools.achievementScores.Add(MoreSlugcatsEnums.EndgameID.Mother, 65);
            }

        }

        public static void UndoRemovedHiddenChallenges()
        {
            On.Expedition.PearlDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDeliveries.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.PearlHoardChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDeliveries.Value)
                {
                    return true;
                }
                return orig(self);
            };
            On.Expedition.NeuronDeliveryChallenge.CanBeHidden += (orig, self) =>
            {
                if (WConfig.cfgHiddenDeliveries.Value)
                {
                    return true;
                }
                return orig(self);
            };

            On.Expedition.PearlHoardChallenge.Generate += (orig, self) =>
            {
                if (WConfig.cfgHiddenDeliveries.Value)
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
                if (WConfig.cfgHiddenDeliveries.Value)
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
 
        public static int Missing_MS_LM(On.Expedition.PearlDeliveryChallenge.orig_RegionPoints orig, PearlDeliveryChallenge self)
        {
            if (WConfig.cfgVistaPearlScore.Value)
            {
                if (self.region == "MS")
                {
                    return 35;
                }
                else if (self.region == "LM")
                {
                    return 10;
                }
            }           
            return orig(self);
        }
    }
}