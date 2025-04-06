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
    public class Saint
    {
        public static void Start()
        {


            //Saint gets 1.35x score for
            //Just kinda mean maybe, like if you get full Karma anyways you deserve it
            On.Expedition.CycleScoreChallenge.Points += SaintLessPointsIfKarma10_HuntInCycle;
            On.Expedition.GlobalScoreChallenge.Points += SaintLessPointsIfKarma10_HuntLong;
            On.Expedition.HuntChallenge.Points += SaintLessPointsIfKarma10_HuntCreature;

            IL.Player.ClassMechanicsSaint += SaintAscendMurder;
            //IL.Expedition.EchoChallenge.Generate += EchoChallengeAddMSEcho;
        }


        public static void SaintAscendMurder(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchIsinst("Creature"),
            x => x.MatchCallOrCallvirt("Creature", "Die")
            ))
            {
                //c.Index -= 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<PhysicalObject, Player, PhysicalObject>>((creature, player) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgSaint_AscendKill.Value)
                        {
                            if (creature is Creature && !(creature as Creature).dead)
                            {
                                player.SessionRecord.AddKill((creature as Creature));
                            }
                        }
                    }
                    return creature;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Saint Murder Points Success"); //could be improved
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Saint Murder Points Fail");
            }
        }


        private static void EchoChallengeAddMSEcho(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "get_Item"),
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Add")
            ))
            {
                c.Index -= 3;
                c.EmitDelegate<Func<List<string>, List<string>>>((list) =>
                {
                    if (WConfig.cfgSaintSubmergedEcho.Value && ExpeditionData.challengeDifficulty > 0.85f && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                    {
                        if (!list.Contains("MS"))
                        {
                            list.Add("MS");
                        }
                    }
                    return list;
                });
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Allow MS Echo Saint IL Success");
            }
            else
            {
                UnityEngine.Debug.Log("ExpeditionExtraConfig: Allow MS Echo Saint IL Failed");
            }
        }

         


        
        public static int SaintLessPointsIfKarma10_HuntCreature(On.Expedition.HuntChallenge.orig_Points orig, HuntChallenge self)
        {
            if (ExpeditionGame.runData != null && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (WConfig.cfgSaint_AscendKill.Value && ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * WConfig.cfgSaintAscendPointPenalty.Value);
                }
            }
            return orig(self);
        }

        private static int SaintLessPointsIfKarma10_HuntLong(On.Expedition.GlobalScoreChallenge.orig_Points orig, GlobalScoreChallenge self)
        {
            if (ExpeditionGame.runData != null && WConfig.cfgSaint_AscendKill.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (WConfig.cfgSaint_AscendKill.Value && ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * WConfig.cfgSaintAscendPointPenalty.Value);
                }
            }
            return orig(self);
        }

        private static int SaintLessPointsIfKarma10_HuntInCycle(On.Expedition.CycleScoreChallenge.orig_Points orig, CycleScoreChallenge self)
        {
            if (ExpeditionGame.runData != null && WConfig.cfgSaint_AscendKill.Value && ModManager.MSC && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (WConfig.cfgSaint_AscendKill.Value && ExpeditionGame.runData.karmaCap == 9)
                {
                    return (int)(orig(self) / 1.35f * WConfig.cfgSaintAscendPointPenalty.Value);
                }
            }
            return orig(self);
        }

    }
}