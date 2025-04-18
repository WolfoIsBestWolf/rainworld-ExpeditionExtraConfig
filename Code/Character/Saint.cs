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
            IL.Expedition.EchoChallenge.Generate += EchoChallengeAddMSEcho;

            On.Player.ClassMechanicsArtificer += Player_ClassMechanicsArtificer;

            //Saint no spear someone asked idk
            //IL.Player.ThrowObject;
            //IL.Player.ThrowToGetFree
            //Remove various perks
            //Remove Global Score
            //Remove Cycle Score
            //Remove Violent Passages
            //Remove Spear Pinning
            //Remove Hunting

            IL.Player.ThrowObject += RemoveSaintSpearExpedition;
            IL.Player.ThrowToGetFree += RemoveSaintSpearExpedition;
            On.Expedition.PinChallenge.ValidForThisSlugcat += PinChallenge_ValidForThisSlugcat;

            On.GhostWorldPresence.SpawnGhost += SaintEasierEchoEncounters;
        }

        private static bool PinChallenge_ValidForThisSlugcat(On.Expedition.PinChallenge.orig_ValidForThisSlugcat orig, PinChallenge self, SlugcatStats.Name slugcat)
        {
            if (slugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                if (WConfig.cfgSaint_NoSpear.Value)
                {
                    return false;
                }
            }
            return orig(self, slugcat);
        }

        private static void RemoveSaintSpearExpedition(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("MoreSlugcats.MoreSlugcatsEnums/SlugcatStatsName", "Saint"));

            if (c.TryGotoNext(MoveType.After,
               x => x.MatchLdsfld("ModManager", "Expedition")))
            {
                c.EmitDelegate<Func<bool, bool>>((karma) =>
                {
                    if (WConfig.cfgSaint_NoSpear.Value)
                    {
                        return false;
                    }
                    return karma;
                });
                //Debug.Log("ExpeditionConfig: RemoveSaintSpearExpedition IL Hook Success");
            }
            else
            {
                Debug.Log("ExpeditionConfig: RemoveSaintSpearExpedition IL Hook Failed");
            }
        }


        public static bool SaintEasierEchoEncounters(On.GhostWorldPresence.orig_SpawnGhost orig, GhostWorldPresence.GhostID ghostID, int karma, int karmaCap, int ghostPreviouslyEncountered, bool playingAsRed)
        {
            if (Custom.rainWorld.ExpeditionMode && Custom.rainWorld.progression.currentSaveState.cycleNumber > 0)
            {
                /*if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    UnityEngine.Debug.Log("Artificer only when max Karma and previous encounter");
                    if (karma >= karmaCap)
                    {
                        return ghostPreviouslyEncountered < 2;
                    }
                }*/
                if (Custom.rainWorld.progression.PlayingAsSlugcat == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                {
                    if (WConfig.cfgSaint_Echoes.Value)
                    {
                        //Idk why this works but it just does??
                        UnityEngine.Debug.Log("Saint regardless of Karma and previous encounter");
                        return ghostPreviouslyEncountered < 2;
                    }
                }
            }
            return orig(ghostID, karma, karmaCap, ghostPreviouslyEncountered, playingAsRed);
        }


        private static void Player_ClassMechanicsArtificer(On.Player.orig_ClassMechanicsArtificer orig, Player self)
        {
            if (self.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint && self.KarmaCap >= 9 && self.godTimer > 0)
            {
                return;
            }
            orig(self);
        }

        private static void WatcherExplosiveJumpFix(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("Player/InputPackage", "spec")))
            {
                //c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, Player, bool>>((input, slug) =>
                {
                    if (slug.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                    {
                        return false;
                    }
                    return input;
                });
                UnityEngine.Debug.Log(": WatcherExplosiveJumpFix");
            }
            else
            {
                UnityEngine.Debug.Log(": WatcherExplosiveJumpFix fail");
            }
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
            /*ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "get_Item"),
            x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "Add")
            ))
            */
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(0));
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(0)))
            {
                //c.Index -= 3;
                c.EmitDelegate<Func<List<string>, List<string>>>((list) =>
                {
                    if (WConfig.cfgSaintSubmergedEcho.Value && ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Saint)
                    {
                        if (WConfig.cfgRemoveRoboLock.Value || ExpeditionData.challengeDifficulty > 0.9f)
                        {
                            Debug.Log("Added Submerged Echo");
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