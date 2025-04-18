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
    public class Spearmaster
    {
        public static void Start()
        {
            IL.OverseerAbstractAI.PlayerGuideUpdate += OverseerAbstractAI_PlayerGuideUpdate;
            IL.WorldLoader.OverseerSpawnConditions += WorldLoader_OverseerSpawnConditions;

        }

        private static void WorldLoader_OverseerSpawnConditions(ILContext il)
        {
            ILCursor c = new(il);

            c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("MoreSlugcats.MoreSlugcatsEnums/SlugcatStatsName", "Spear"));

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("MiscWorldSaveData", "SSaiConversationsHad")))
            {
                c.EmitDelegate<Func<int, int>>((karma) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgSpearOverseer.Value)
                        {
                            return 0;
                        }
                    }
                    return karma;
                });
                Debug.Log("ExpeditionConfig: WorldLoader_cfgSpearOverseer IL Hook Success");
            }
            else
            {
                Debug.Log("ExpeditionConfig: WorldLoader_cfgSpearOverseer IL Hook Failed");
            }
        }

        public static void OverseerAbstractAI_PlayerGuideUpdate(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("MiscWorldSaveData", "SSaiConversationsHad")))
            {
                c.EmitDelegate<Func<int, int>>((karma) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        if (WConfig.cfgSpearOverseer.Value)
                        {
                            return 0;
                        }
                    }
                    return karma;
                });
                Debug.Log("ExpeditionConfig: OverseerAbstractAI cfgSpearOverseer IL Hook Success");
            }
            else
            {
                Debug.Log("ExpeditionConfig: OverseerAbstractAI cfgSpearOverseer IL Hook Failed");
            }
        }
    }
}