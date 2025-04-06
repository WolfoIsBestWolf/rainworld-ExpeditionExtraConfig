using Watcher;
using MoreSlugcats;
using System.Collections.Generic;

namespace ExpeditionExtraConfig
{
    public class WatcherExpedition
    {
        public static void Start()
        {
            //Ripple level not Karma
            //WatcherMode

            //Would Echoes spawn? How would that be defined
            //Would the RoT whatever be progressed?

            //Well it just breaks lol

            On.Expedition.ExpeditionData.GetPlayableCharacters += ExpeditionData_GetPlayableCharacters;
            On.Expedition.ExpeditionProgression.CheckUnlocked += ExpeditionProgression_CheckUnlocked;
        }

        private static bool ExpeditionProgression_CheckUnlocked(On.Expedition.ExpeditionProgression.orig_CheckUnlocked orig, ProcessManager manager, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return true;
            }
            return orig(manager, slugcat);
        }

        private static List<SlugcatStats.Name> ExpeditionData_GetPlayableCharacters(On.Expedition.ExpeditionData.orig_GetPlayableCharacters orig)
        {
            var list = orig();
            if (ModManager.Watcher)
            {
                list[1] = WatcherEnums.SlugcatStatsName.Watcher;
                /*list.Add(WatcherEnums.SlugcatStatsName.Watcher);
                foreach (var item in list)
                {
                   UnityEngine.Debug.Log(item);
                }*/
            }
            return list;
        }
    }
}