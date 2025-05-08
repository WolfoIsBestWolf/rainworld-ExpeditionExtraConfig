using JetBrains.Annotations;
using Menu.Remix;
using Menu.Remix.MixedUI;
using MoreSlugcats;
using RWCustom;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionExtraConfig
{
    public class VanillaPreset<T>
    {
        public VanillaPreset(Configurable<T> config, T vanillaValue, T remixValue)
        {
            this.config = config;
            this.vanillaValue = vanillaValue;
        }
        public Configurable<T> config;
        public T remixValue;
        public T vanillaValue;
    }

    public class WConfig : OptionInterface
    {

		public static WConfig instance = new WConfig();

        public static Configurable<bool> cfgNewPerksForceUnlock = instance.config.Bind("cfgNewPerksForceUnlock", false,
          new ConfigurableInfo("Perks added by mod do not require to be unlocked.", null, "", new object[]
          {
                    "Unlock all new perks"
          }));

        public static Configurable<bool> cfgCycle0_Save = instance.config.Bind("cfgCycle0_Save", false,
          new ConfigurableInfo("The game will save immediately upon starting an Expedition", null, "", new object[]
          {
                    "Save Cycle 0"
          }));
        public static Configurable<bool> cfgCycle0_Karma = instance.config.Bind("cfgCycle0_Karma", false,
          new ConfigurableInfo("You wont lose Karma or game over on Cycle 0 / the start of an expedition.", null, "", new object[]
          {
                    "Lossless Cycle 0"
          }));


        public static Configurable<bool> cfgRandom_Difficulty = instance.config.Bind("cfgRandom_Difficulty", false,
          new ConfigurableInfo("Challenges will be of a random difficulty level, but never lower than what is selected.", null, "", new object[]
          {
                    "Randomize Difficulty"
          }));

        public static Configurable<bool> cfgHidden_Passage = instance.config.Bind("cfgHidden_Passage", true,
          new ConfigurableInfo("Allow Passage challenges to be Hidden", null, "", new object[]
          {
                    "Hidden Passage"
          }));
 
        public static Configurable<bool> cfgHidden_Hunting = instance.config.Bind("cfgHidden_Hunting", true,
          new ConfigurableInfo("Allow Hunting challenges to be Hidden", null, "", new object[]
          {
                    "Hidden Hunting"
          }));
        public static Configurable<bool> cfgHidden_ItemCollecting = instance.config.Bind("cfgHidden_ItemCollecting", true,
           new ConfigurableInfo("Allow Item Hoarding challenges to be Hidden", null, "", new object[]
           {
                    "Hidden Item Collecting"
           }));
        public static Configurable<bool> cfgHidden_Delivery = instance.config.Bind("cfgHidden_Delivery", false,
          new ConfigurableInfo("Allow Pearl & Neuron Delivery challenges to be Hidden. Vanilla is false.", null, "", new object[]
          {
                    "Hidden Deliveries"
          }));
 
        public static Configurable<bool> cfgHidden_PearlHoard = instance.config.Bind("cfgHidden_PearlHoard", false,
            new ConfigurableInfo("Allow Pearl Hoarding challenges to be Hidden", null, "", new object[]
            {
                    "Hidden Pearl Hoarding"
            }));
        public static Configurable<bool> cfgHidden_EchoVista = instance.config.Bind("cfgHidden_EchoVista", true,
          new ConfigurableInfo("Allow Echo & Vista Visiting challenges to be Hidden", null, "", new object[]
          {
                    "Hidden Vistas & Echo"
          }));
            public static Configurable<bool> cfgHidden_Score = instance.config.Bind("cfgHidden_Score", true,
          new ConfigurableInfo("Allow Cycle & Overall Score challenges to be Hidden", null, "", new object[]
          {
                    "Hidden Score Challenges"
          }));
        public static Configurable<bool> cfgHidden_Filter = instance.config.Bind("cfgHidden_Filter", false,
     new ConfigurableInfo("Hidden Challenges still respect the challenge filter.\nThis can be easily abused which is why it's not in vanilla.", null, "", new object[]
     {
                    "Hidden Challenge Filter"
     }));

 



        public static Configurable<bool> cfgHoardingBonus = instance.config.Bind("cfgHoardingBonus", true,
           new ConfigurableInfo("Pearl & Item hoarding will reward some extra points if asking to hoard a lot.", null, "", new object[]
           {
                    "Hoarding bonus score"
           }));
        public static Configurable<bool> cfgVistaPearlScore = instance.config.Bind("cfgVistaPearlScore", true,
                  new ConfigurableInfo("Vistas going to OE, MS, LC, SS, RM, DM give more score as they are out of the way regions, similiar to Pearl Delivery.\nPearl Delivery from MS, LM get more score, as devs forgot to set a value.", null, "", new object[]
                  {
                    "Vista & Pearl bonus score"
                  }));


        public static Configurable<bool> cfgSaint_NoSpear = instance.config.Bind("cfgSaint_NoSpear", false,
           new ConfigurableInfo("Remove Saints ability to throw a spear to match his Campagin.\nSpear Pinning will be unavailable. Combat challenges will be rendered basically impossible.", null, "", new object[]
           {
                    "No Spear throwing"
           }));
        public static Configurable<bool> cfgSaint_NoCombat = instance.config.Bind("cfgSaint_NoCombat", false,
           new ConfigurableInfo("Combat challenges will not appear as Saint. Cycle Score, Global Score, Hunting and some passages. ", null, "", new object[]
           {
                    "No Combat challenges"
           }));
        #region Vistas
        public static Configurable<bool> cfgShowVistaOnMap = instance.config.Bind("cfgShowVistaOnMap", true,
            new ConfigurableInfo("Shows Vista Points on the map, regardless of if you've been to that room. To help with some of the guess work or looking it up", null, "", new object[]
            {
                "Vista Points on map"
            }));
        public static Configurable<bool> cfgVistaCircle = instance.config.Bind("cfgVistaCircle", false,
                  new ConfigurableInfo("For above config, Vista Points on the map will also emit a circle. I do not like how this looks at the moment", null, "", new object[]
                  {
                "Vista Radar on map"
                  }));
        public static Configurable<bool> cfgSnowMeter = instance.config.Bind("cfgSnowMeter", true,
                  new ConfigurableInfo("Enable Rain Meter for Saint. Not sure why they didn't want us to know when the blizzard happens.", null, "", new object[]
                  {
                "Saint Snow Meter"
                  }));


        public static Configurable<bool> cfgVista_LM = instance.config.Bind("cfgVista_LM", true,
           new ConfigurableInfo("For Spearmaster & Artificer, Add Vista Spots in Waterfront Facility.\nLocated in : LM_TOWER04 | LM_B01 | LM_C04", null, "", new object[]
           {
                    "Vistas - Waterfront Facility"
           }));
        public static Configurable<bool> cfgVista_MS = instance.config.Bind("cfgVista_MS", true,
           new ConfigurableInfo("For Rivulet, Add Vista Spots in Submerged Superstructer. All will be located before the Heart.\nLocated in : MS_AI |  MS_AIR03 | MS_FARSIDE", null, "", new object[]
           {
                    "Vistas - Submerged Superstructure"
           }));
        public static Configurable<bool> cfgVista_SS = instance.config.Bind("cfgVista_SS", true,
           new ConfigurableInfo("Add Vista Spots in Five Pebbles.\nLocated in : SS_D04 | SS_E05 | SS_I03", null, "", new object[]
           {
                    "Vistas - Five Pebbles"
           }));
        public static Configurable<bool> cfgVista_OE = instance.config.Bind("cfgVista_OE", true,
                   new ConfigurableInfo("Allow Outer Expanse Vistas for Monk & Survivor.", null, "", new object[]
                   {
                    "Outer Expanse for Monk & Survivor"
                    //"Outer Expanse for more Slugcats."
                   }));

        #endregion
        #region Hunting

        public static Configurable<int> cfg_HuntChallengeLimit = instance.config.Bind("cfg_HuntChallengeLimit", 15,
           new ConfigurableInfo("Hunting challenges never ask for more than this many kills. Vanilla is 15.\nSet lower if you dont want kill 15 Green Lizards or King Vultures to be options.", new ConfigAcceptableRange<int>(3, 15), "", new object[]
           {
                    "Hunting Limit"
           }));


        public static Configurable<int> cfg_HuntChallengeLimitRed = instance.config.Bind("cfg_HuntChallengeLimitRed", 10,
        new ConfigurableInfo("Hunts targeting high scoring targets will never ask for more than this many kills. To avoid dragged out Expeditions due to creature rarity or personal preference.\nRed Lizard, Blizzard Lizard, Red Centipede, Vulture, King Vulture, Miros Vulture, Miros Bird, Elite Scav, Inspector.", new ConfigAcceptableRange<int>(1, 15), "", new object[]
               {
                "Hunting Limit - Dangerous"
               }));
        public static Configurable<int> cfg_HuntChallengeLimitRot = instance.config.Bind("cfg_HuntChallengeLimitRot", 4,
           new ConfigurableInfo("Hunts targetting Brother & Daddy Long Legs will never ask for more than this many kills.\nDue to limited ways to kill them, requiring 8+ kills may be overly frustrating.", new ConfigAcceptableRange<int>(1, 15), "", new object[]
           {
                "Hunting Limit - Rot"
           }));
        #endregion


        public static Configurable<bool> cfgCustomColorMenu = instance.config.Bind<bool>("cfgCustomColorMenu", true,
            new ConfigurableInfo("Adds the custom slugcat color menu from Remix to the Expedition character select screen.\nOnly if Jolly Co-op is disabled (that has it's own).", null, "", new object[]
            {
                        "Custom Color menu for Expedition"
            }));

        public static Configurable<bool> cfgPassageTeleportation = instance.config.Bind("cfgPassageTeleportation", false, 
        new ConfigurableInfo("Completed passages will grant teleportation tokens. This is also available as a Perk, if you don't want it for free.\nCompleting challenges with the 'Enable Passages' perk will still grant passages, so it still has value.", null, "", new object[]
            {
                "Passage Teleports"
            }));
        public static Configurable<bool> cfgKarmaFlower = instance.config.Bind("cfgKarmaFlower", false, 
            new ConfigurableInfo("Allow natural Karma Flowers to spawn during Expedition. Hunter still can not find them", null, "", new object[]
			{
				"Natural Karma Flowers"
			}));
		public static Configurable<bool> cfgMaxKarmaEchos = instance.config.Bind("cfgMaxKarmaEchos", true, 
            new ConfigurableInfo("Normally Echos do not increase max Karma in Expedition. This will make expeditions somewhat easier and allow Saint to reach his potential.", null, "", new object[]
			{
				"Echoes increase max Karma"
			}));
		public static Configurable<bool> cfgMaxKarmaPebbles = instance.config.Bind("cfgMaxKarmaPebbles", true, 
            new ConfigurableInfo("Five Pebbles will increase your Karma like in campaigns. Increase by 1 for Hunter/Gourmand and set to max for Survivor/Monk.", null, "", new object[]
					{
				"Pebbles increases max Karma"
					}));

		public static Configurable<int> cfgKarmaStart = instance.config.Bind("cfgKarmaStart", 2, 
            new ConfigurableInfo("Change starting Karma. Vanilla is 2.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
			{
				"Starting Karma"
			}));
		public static Configurable<int> cfgKarmaStartMax = instance.config.Bind("cfgKarmaStartMax", 5, 
            new ConfigurableInfo("Change starting Max Karma. Vanilla is 5.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
			{
				"Starting Max Karma"
			}));

        public static Configurable<bool> cfgRemovePermaDeath = instance.config.Bind("cfgRemovePermaDeath", false, 
            new ConfigurableInfo("Expeditions no longer end when dying at 1 Karma. This does not apply to the Doomed Burden.", null, "", new object[]
            {
                "Remove Perma Death"
            }));
        public static Configurable<bool> cfgUnlockEgg = instance.config.Bind("cfgUnlockEgg", false,
           new ConfigurableInfo("Eggspedition easter egg that makes you glow Rainbow that requires you to stand in a specific spot as each Slugcat.\nNo Unlock config is permament.", null, "", new object[]
           {
                "Unlock Rainbow"
           }));
        public static Configurable<int> cfgBonusPerkSlots = instance.config.Bind("cfgBonusPerkSlots", 0,
           new ConfigurableInfo("Grants extra perk slots.", new ConfigAcceptableRange<int>(0, 30) , "", new object[]
           {
                "Extra Perk Slots"
           }));
        public static Configurable<bool> cfgUnlockPerkSlots = instance.config.Bind("cfgUnlockPerkSlots", false,
            new ConfigurableInfo("Sets perk slots to 8, the highets possible in vanilla.\nNo Unlock config is permament.", null, "", new object[]
            {
                "Unlock Perk Slots"
            }));
        public static Configurable<bool> cfgUnlockAll = instance.config.Bind("cfgUnlockAll", false,
           new ConfigurableInfo("Makes Perks and Missions not require unlocks. Intended for testing.\nNo Unlock config is permament or adds progress.", null, "", new object[]
           {
                "Unlock Perks & Missions"
           }));
        public static Configurable<bool> cfgInfinitePassage = instance.config.Bind("cfgInfinitePassage", false,
            new ConfigurableInfo("Infinite passage teleports. Does not require perk. Intended for testing.", null, "", new object[]
            {
                "Infinite Passages"
            }));
        /*public static Configurable<bool> cfgScoreDisplay = instance.config.Bind("cfgScoreDisplay", true, 
            new ConfigurableInfo("Displays Points, on the challenge menu. This is a bug fix. Disable it if they fix it officially I guess.", null, "", new object[]
           {
                "Score on challenge Menu"
           }));*/


        public static Configurable<bool> cfgSpearOverseer = instance.config.Bind("cfgSpearOverseer", true, 
            new ConfigurableInfo("Spearmaster will be followed by his Red Overseer. This should have no gameplay effect.", null, "", new object[]
			{
				"Spearmaster Overseer"
			}));

        public static Configurable<bool> cfgUnlockJukebox = instance.config.Bind("cfgUnlockJukebox", false,
            new ConfigurableInfo("Music tracks in the Jukebox no longer require to be unlocked.", null, "", new object[]
            {
                "Unlock Music"
            }));
        public static Configurable<bool> cfgJukeboxAdditions = instance.config.Bind("cfgJukeboxAdditions", true,
            new ConfigurableInfo("Adds a Autoplay button so tracks play one after another. You can go from the first to the last page and vice versa.", null, "", new object[]
            {
                "Jukebox Additions"
            }));

        public static Configurable<bool> cfgRemoveRoboLock = instance.config.Bind("cfgRemoveRoboLock", true,
           new ConfigurableInfo("During Expeditions, Allows everyone to access Metropolis or Submerged Superstructure from the top.\nLike in Artificer Expeditions, it is replace it with a 1 Karma Gate.", null, "", new object[]
           {
                "Remove ID Drone Gates"
           }));

 
        #region Rivulet
        public static Configurable<bool> cfgRivuletBall = instance.config.Bind("cfgRivuletBall", false,
            new ConfigurableInfo("Rivulet starts with the Rarefaction Cell. This is also available as a Perk, if you don't want it for free.", null, "", new object[]
            {
                "Rarefaction Cell Start"
            }));

        public static Configurable<bool> cfgRivuletShortCycles = instance.config.Bind("cfgRivuletShortCycles", true, 
            new ConfigurableInfo("Shorter Cycles and more Shelter Failures. To make his expeditions feel more like his campaign.  Cycle Point challenges will require less points.", null, "", new object[]
			{
				"Rivulet Shorter Cycles" 
			}));

        /*public static Configurable<int> cfgRiv_HeavyRainDuration = instance.config.Bind("cfgRiv_HeavyRainDuration", 0,
          new ConfigurableInfo("At the start of a Rivulet Expedition, how many cycles should be shorter.", new ConfigAcceptableRange<int>(0, 200), "", new object[]
          {
                "Strong Rain Duration"
          }));*/

        public static Configurable<int> cfgRiv_HeavyRainChance = instance.config.Bind("cfgRiv_HeavyRainChance", 25,
           new ConfigurableInfo("Chance for above config on any given cycle. Sooner rain and likelier shelter failure. It will not get rerolled by dying.", new ConfigAcceptableRange<int>(5, 100), "", new object[]
           {
                "Strong Rain Chance"
           }));

        public static Configurable<float> cfgRiv_RainMult = instance.config.Bind("cfgRiv_RainMult", 0.50f, 
            new ConfigurableInfo("Cycle length multiplier for above config. In Campaign its 0.33 in most regions and 0.5 in some.", new ConfigAcceptableRange<float>(0.3f, 1f), "", new object[]
			{
                "Cycle Length mult"
            }));
		/*public static Configurable<float> cfgRiv_RainMultRegional = instance.config.Bind("cfgRivuletMultiRegional", 0.50f, 
		    new ConfigurableInfo("Rain Multiplier for Pipeyard/Exterior/Shaded/Subterranean/Shoreline for Riv Shorter Cycles in Expedition. Vanilla 0.5", new ConfigAcceptableRange<float>(0.25f, 1f), "", new object[]
			{
				"Rain Multiplier"
			}));*/
		public static Configurable<int> cfgRiv_ShelterFailRate = instance.config.Bind("cfgRiv_ShelterFailRate", 30, 
            new ConfigurableInfo("Shelter Fail rates for above config. In Campaign its is 25 in most regions and 40 in some and 8 everywhere after Pebbles.", new ConfigAcceptableRange<int>(8, 100), "", new object[]
			{
				"Shelter Fail chance"
			}));
        /*public static Configurable<float> cfgRiv_ShelterFailRateRegional = instance.config.Bind("cfgRivuletShelterRateRegional", 25f, 
         new ConfigurableInfo("Shelter Fail rates for Pipeyard/Shoreline/Shaded/Garbage for Riv Shorter Cycles in Expedition. Vanilla 40, recommended 25", new ConfigAcceptableRange<float>(0f, 100f), "", new object[]
			{
				"Shelter Fail rates"
			}));*/
        public static Configurable<bool> cfgRiv_ShortCyclePointBonus = instance.config.Bind("cfgRiv_ShortCyclePointBonus", true,
            new ConfigurableInfo("Rivulet gets 1.1x score on challenges that involve long travel. To make up for some cycles being shorter.\nOnly active if short cycles enabled.", null, "", new object[]
        {
                "Travel Bonus Score"
        }));
        #endregion


        #region Saint
        public static Configurable<bool> cfgSaint_AscendKill = instance.config.Bind("cfgSaint_AscendKill", true,
         new ConfigurableInfo("Ascending usually does not count as kills for challenges.", null, "", new object[]
         {
                "Ascend Kills"
         }));

        public static Configurable<float> cfgSaintAscendPointPenalty = instance.config.Bind("cfgSaintAscendPointPenalty", 1f, 
            new ConfigurableInfo("Saint gets 1.35x score multiplier for combat challenges. This multiplier will be used instead if you Ascended creatures as it's easier.\n(Only applies when run is finished with 10 max Karma and above config enabled.)", new ConfigAcceptableRange<float>(0.2f, 1.35f), "", new object[]
        {
                "Ascension Point penalty"
        }));

        public static Configurable<bool> cfgSaint_Echoes = instance.config.Bind("cfgSaint_Echoes", true, 
			new ConfigurableInfo("Allow Saint to encounter Echos regardless of Karma and without previous visits like in his Campaign. Still can't visit Echos on cycle 0.", null, "", new object[]
			{
				"Saint Echo mechanics"
			}));
        public static Configurable<bool> cfgSaintSubmergedEcho = instance.config.Bind("cfgSaintSubmergedEcho", false, 
            new ConfigurableInfo("For Saint, allow Submerged Superstructure for Echo Challenges\nThis will be tremendously difficult if ID gates are not disabled or you aren't using the Rivulet Perk.", null, "", new object[]
        {
                "Submerged Superstructure Echo"
        }));

        public static Configurable<bool> cfgSaint_MaxKarmaBool = instance.config.Bind("cfgSaint_MaxKarmaBool", false,
           new ConfigurableInfo("Should Saint start with a different max Karma amount in Expedition mode", null, "", new object[]
           {
                "Different Max Karma"
           }));

        public static Configurable<int> cfgSaint_MaxKarma = instance.config.Bind("cfgSaint_MaxKarma", 6,
           new ConfigurableInfo("Starting max Karma amount for above config. This can make it easier to reach Karma 10 with Echoes if that is enabled.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
           {
                "Starting Max Karma"
           }));
        #endregion

        public static Configurable<bool> cfgStomachPearl = instance.config.Bind("cfgStomachPearl", true, 
			new ConfigurableInfo("Hunter and Rivulet start with their Pearl. This can help various challenges.", null, "", new object[]
			{
				"Stomach Pearl"
			}));
		public static Configurable<int> cfgHunterPlusKarma = instance.config.Bind("cfgHunterPlusKarma", 2, 
			new ConfigurableInfo("Increase Hunters starting Karma by this amount. In his campaign he starts with 2 more Karma.", new ConfigAcceptableRange<int>(0, 5), "", new object[]
			{
				"Starting Karma+"
			}));

 

		/*public static Configurable<bool> cfgHiddenDeliveries = instance.config.Bind("cfgHiddenDeliveries", false, 
            new ConfigurableInfo("Allow Pearl Delivery, Pearl Hoarding & Neuron Gifting to be chosen as Hidden challenges again.\nThis was removed in a patch. Hidden gives 2x points as usual.", null, "", new object[]
			{
				"Hidden Deliveries"
			}));*/

        /*public static Configurable<bool> cfgPauseWarning = instance.config.Bind<bool>("cfgPauseWarning", false, 
            new ConfigurableInfo("Show the exit warning about losing Karma sooner.", null, "", new object[]
            {
                "Early exit warning"
            }));*/
        public static Configurable<bool> cfgMusicPlayMore = instance.config.Bind<bool>("cfgMusicPlayMore", false, 
            new ConfigurableInfo("Music triggers play every cycle instead of having a 5 Cycle Cooldown. Multiple tracks can also play in one cycle.", null, "", new object[]
            {
                "Music plays more often"
            }));


        public static Configurable<bool> cfgTestingMission = instance.config.Bind("cfgTestingMission", false,
   new ConfigurableInfo("", null, "", new object[]
   {
                "Developer Info"
   }));


        //public static Configurable<bool> cfgPassageTeleports; //Apparently vanill as an unlock
        public static Configurable<float> cfgMapRevealRadius = instance.config.Bind("cfgMapRevealRadius", 2f,
           new ConfigurableInfo("Map Reveal Radius multiplier during Expedition. To more easily fill out the map in Expedition so you can focus more on your objectives. Also because I cant figure out how to transfer map progress.", new ConfigAcceptableRange<float>(1f, 20f), "", new object[]
           {
                "Expedition Map Reveal mult"
           }));

        //Pups and allow Mother
        #region Pups

        public static Configurable<bool> cfgPupsSpawn = instance.config.Bind("cfgPupsSpawn", true, 
			new ConfigurableInfo("Allow Slugpups to spawn in Expedition, for Survivor, Monk, Hunter, Gourmand", null, "", new object[]
            {
                "Slugpups in Expedition"
            }));
        public static Configurable<bool> cfgPupsSpawnNonDefault = instance.config.Bind("cfgPupsSpawnNonDefault", false,
            new ConfigurableInfo("For above config, allow Slugpups to also spawn for Rivulet, Saint, Spearmaster, Artificer", null, "", new object[]
            {
                "Slugpups non default characters"
            }));
        public static Configurable<bool> cfgPupsSpawnFrequently = instance.config.Bind("cfgPupsSpawnFrequently", true,
            new ConfigurableInfo("During Expeditions, guarantee a Slugpup every 10 cycles instead of every 25. For Hunter it is 5 Cycles regardless. To help with RNG for Mother Passage", null, "", new object[]
            {
                "Frequent Slugpups"
            }));
        public static Configurable<bool> cfgPupsMotherAchievement = instance.config.Bind("cfgPupsMotherAchievement", true,
            new ConfigurableInfo("Allow Mother Passage as a possible objective. If Slugpups are enabled.\nThis is disabled in vanilla because of no Slugpups.", null, "", new object[]
            {
                "Mother Passage"
            }));

        public static Configurable<bool> cfgSurvivor_StartWithPups = instance.config.Bind("cfgSurvivor_StartWithPups", false,
          new ConfigurableInfo("Survivor will start his Expedition with 2 Pups, to make him more interesting. This is also available as a Perk, if you don't want it for free.", null, "", new object[]
          {
                "Start with pups"
          }));
 
        public static Configurable<bool> cfgGourmand_StartWithPups = instance.config.Bind("cfgGourmand_StartWithPups", false,
          new ConfigurableInfo("Gourmand will start his Expedition with 2 Pups, like the first time rebooting into his campaign after finishing it.", null, "", new object[]
          {
                "Start with pups"
          }));
        #endregion

        //Monk leaves Flowers on death in Vanilla


        //public static Configurable<bool> cfgArti_CorpseGates; //Corpses increase Karma in vanilla

        #region Artificer
        /*public static Configurable<bool> cfgArti_EchoMechanics = instance.config.Bind("cfgEchoArtificer", false, 
           new ConfigurableInfo("Should Artificer encounter Echos only at max Karma. This makes Expeditions harder. In vanilla you need a Karma Flower too, but as they dont spawn only max Karma for this setting.", null, "", new object[]
           {
               "Artificer Echo mechanics"
           }));*/

        public static Configurable<bool> cfgArtificerRobo = instance.config.Bind("cfgArtificerRobo", true, 
            new ConfigurableInfo("Artificer starts with her drone. This should be purely cosmetic", null, "", new object[]
           {
                "Artificer Drone"
           }));
        public static Configurable<bool> cfgArti_MaxKarmaBool = instance.config.Bind("cfgArti_MaxKarmaBool", false,
           new ConfigurableInfo("Should Artificer start with a different max Karma amount in Expedition mode", null, "", new object[]
           {
                "Different Max Karma"
           }));

        public static Configurable<int> cfgArti_MaxKarma = instance.config.Bind("cfgArti_MaxKarma", 4,
           new ConfigurableInfo("Starting Max Karma amount for above config. This can force her to use Scav Corpses to go through some gates.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
           {
                "Starting Max Karma"
           }));
        public static Configurable<bool> cfgArti_LungUp = instance.config.Bind("cfgArti_LungUp", true,
            new ConfigurableInfo("Increase Artificers lung capacity like in Jolly Co-op.", null, "", new object[]
           {
                ""
           }));
        #endregion
        //public static Configurable<bool> cfgArti_ScavKing;
        //public static Configurable<bool> cfgSaint_Rubicon; //???


        //Monk leaves Karma Flowers by default so
        public static Configurable<bool> cfgMonk_CombatScore = instance.config.Bind("cfgMonk_CombatScore", true,
           new ConfigurableInfo("Monk gets 1.1x score on Combat challenges. Saint gains 1.35x score on combat challenges in vanilla but both have weak spears so this seems fair.", null, "", new object[]
           {
                "Monk Combat Bonus Score"
           }));






        private OpHoldButton classicPresetBtn;
        public override void Initialize()
		{
			base.Initialize();
			this.Tabs = new OpTab[]
			{
                new OpTab(this, "Main"),
                new OpTab(this, "Character"),
                new OpTab(this, "Challenges"),
                new OpTab(this, "Cheats"),
            };
			this.AddCheckbox();
            VanillaValues();
            cfgUnlockAll.OnChange += CfgUnlockAll_OnChange;
            
        }

        public static void CfgUnlockAll_OnChange()
        {
            //Debug.Log(cfgUnlockAll.Value);
            if (cfgUnlockAll.Value && UnlockAll.added == false)
            {
                UnlockAll.Add();
            }
            else if (cfgUnlockAll.Value == false && UnlockAll.added)
            {
                UnlockAll.Remove();
            }
        }


        public void VanillaPreset(UIfocusable trigger)
        {
            for (int i = 0; i < boolPresets.Count; i++)
            {
                if (boolPresets[i].config.BoundUIconfig != null)
                {
                   boolPresets[i].config.BoundUIconfig.value = ValueConverter.ConvertToString<bool>(boolPresets[i].vanillaValue);
                }
            }
            for (int j = 0; j < intPresets.Count; j++)
            {
                if (intPresets[j].config.BoundUIconfig != null)
                {
                    intPresets[j].config.BoundUIconfig.value = ValueConverter.ConvertToString<int>(intPresets[j].vanillaValue);
                }
            }
            for (int k = 0; k < floatPresets.Count; k++)
            {
                if (floatPresets[k].config.BoundUIconfig != null)
                {
                    floatPresets[k].config.BoundUIconfig.value = ValueConverter.ConvertToString<float>(floatPresets[k].vanillaValue);
                }
            }
        }
        public static List<VanillaPreset<bool>> boolPresets;
        public static List<VanillaPreset<int>> intPresets;
        public static List<VanillaPreset<float>> floatPresets;

        public static void VanillaValues()
        {
            intPresets = new List<VanillaPreset<int>>()
            {
                new VanillaPreset<int>(cfgKarmaStart, 2, 15),
                new VanillaPreset<int>(cfgKarmaStartMax, 5, 15),
                new VanillaPreset<int>(cfg_HuntChallengeLimit, 15, 15),
                new VanillaPreset<int>(cfg_HuntChallengeLimitRot, 15, 4),
                new VanillaPreset<int>(cfg_HuntChallengeLimitRed, 15, 12),
                new VanillaPreset<int>(cfgHunterPlusKarma, 0, 2),
                new VanillaPreset<int>(cfgArti_MaxKarma, 5, 4),
                new VanillaPreset<int>(cfgSaint_MaxKarma, 5, 6),
                new VanillaPreset<int>(cfgRiv_HeavyRainChance, 100, 22),
                new VanillaPreset<int>(cfgRiv_ShelterFailRate, 20, 30),
new VanillaPreset<int>(cfgBonusPerkSlots, 0, 30),

            };
            floatPresets = new List<VanillaPreset<float>>()
            {
                new VanillaPreset<float>(cfgMapRevealRadius, 1f, 2f),
                new VanillaPreset<float>(cfgRiv_RainMult, 0.33f, 2f),
                new VanillaPreset<float>(cfgSaintAscendPointPenalty, 1.35f, 1.2f),
            };
            boolPresets = new List<VanillaPreset<bool>>()
            {
                new VanillaPreset<bool>(cfgMaxKarmaEchos, false, true),
                new VanillaPreset<bool>(cfgMaxKarmaPebbles, false, true),
                new VanillaPreset<bool>(cfgMonk_CombatScore, false, true),
                new VanillaPreset<bool>(cfgPupsMotherAchievement, false, true),
                new VanillaPreset<bool>(cfgShowVistaOnMap, false, true),
                new VanillaPreset<bool>(cfgRemoveRoboLock, false, true),
                new VanillaPreset<bool>(cfgPassageTeleportation, false, true),
 new VanillaPreset<bool>(cfgKarmaFlower, false, true),

                new VanillaPreset<bool>(cfgPupsSpawn, false, true),
                new VanillaPreset<bool>(cfgPupsSpawnNonDefault, false, true),
                new VanillaPreset<bool>(cfgPupsSpawnFrequently, false, true),
                new VanillaPreset<bool>(cfgMonk_CombatScore, false, true),
                new VanillaPreset<bool>(cfgSurvivor_StartWithPups, false, true),
                new VanillaPreset<bool>(cfgGourmand_StartWithPups, false, true),
                new VanillaPreset<bool>(cfgStomachPearl, false, true),
                new VanillaPreset<bool>(cfgArti_MaxKarmaBool, false, true),
                new VanillaPreset<bool>(cfgRivuletBall, false, true),
                new VanillaPreset<bool>(cfgRivuletShortCycles, false, true),
                new VanillaPreset<bool>(cfgRiv_ShortCyclePointBonus, false, true),
                new VanillaPreset<bool>(cfgSaint_Echoes, false, true),
                new VanillaPreset<bool>(cfgSaint_AscendKill, false, true),
                new VanillaPreset<bool>(cfgSaint_MaxKarmaBool, false, true),
                new VanillaPreset<bool>(cfgSaintSubmergedEcho, false, true),
                new VanillaPreset<bool>(cfgSaint_NoSpear, false, true),
                new VanillaPreset<bool>(cfgVista_MS, false, true),
                new VanillaPreset<bool>(cfgVista_LM, false, true),
                 new VanillaPreset<bool>(cfgVista_SS, false, true),
                 new VanillaPreset<bool>(cfgVista_OE, false, true),

                new VanillaPreset<bool>(cfgInfinitePassage, false, true),
                new VanillaPreset<bool>(cfgRemovePermaDeath, false, true),
                new VanillaPreset<bool>(cfgUnlockAll, false, true),
                new VanillaPreset<bool>(cfgUnlockPerkSlots, false, true),
                new VanillaPreset<bool>(cfgUnlockEgg, false, true),
                new VanillaPreset<bool>(cfgHoardingBonus, false, true),
                new VanillaPreset<bool>(cfgVistaPearlScore, false, true),
                 new VanillaPreset<bool>(cfgVistaCircle, false, true),
                  new VanillaPreset<bool>(cfgSnowMeter, false, true),
                
                //new VanillaPreset<bool>(cfgUnlockJukebox, false, true),
                new VanillaPreset<bool>(cfgHidden_Delivery, false, true),
            new VanillaPreset<bool>(cfgHidden_EchoVista, false, true),
            new VanillaPreset<bool>(cfgHidden_PearlHoard, false, true),
   
new VanillaPreset<bool>(cfgRandom_Difficulty, false, true),
new VanillaPreset<bool>(cfgHidden_Filter, false, true),

            new VanillaPreset<bool>(cfgHidden_Score, true, true),
 
            new VanillaPreset<bool>(cfgHidden_Hunting, true, true),
            new VanillaPreset<bool>(cfgHidden_ItemCollecting, true, true),
            new VanillaPreset<bool>(cfgHidden_Passage, true, true),
 


            };
        }


        private void PopulateWithConfigs(int tabIndex, ConfigurableBase[][] lists, [CanBeNull] string[] names, [CanBeNull] Color[] colors, int splitAfter)
        {
            new OpLabel(new Vector2(100f, 560f), new Vector2(400f, 30f), this.Tabs[tabIndex].name, FLabelAlignment.Center, true, null);
            OpTab opTab = this.Tabs[tabIndex];
            float num = 40f;
            float horizontal = 20f;
            float startingHeight = (tabIndex >= 1 ? 554f : 500f)-8f;
            float height = startingHeight;

            UIconfig uiconfig = null;
            for (int i = 0; i < lists.Length; i++)
            {
                /*if (i == 1 && tabIndex == 1)
                {
                    num3 -= 36f;
                }*/
                Color color = Menu.MenuColorEffect.rgbMediumGrey;
                if (colors != null)
                {
                    color = colors[i];
                }
                if (tabIndex == 3)
                {

                }
                if (i == splitAfter)
                {
                    horizontal += 300f;
                    height = startingHeight;
                    num = 40f;
                    //uiconfig = null;
                }
                else if (names != null && names[i] == "")
                {
                    if (i > 0)
                    {
                        if (names[i] == "")
                        {
                            height += 70f;
                        }
                    }
                }
                if (!(names == null || names[i] == ""))
                {
                    var label = new OpLabel(new Vector2(horizontal, height - num + 10f), new Vector2(260f, 30f), "~ " + names[i] + " ~", FLabelAlignment.Center, true, null);
                    label.color = color;
                    opTab.AddItems(new UIelement[]
                    {
                        label
                    });
                }
                FTextParams ftextParams = new FTextParams();
                if (InGameTranslator.LanguageID.UsesLargeFont(Custom.rainWorld.inGameTranslator.currentLanguage))
                {
                    ftextParams.lineHeightOffset = -12f;
                }
                else
                {
                    ftextParams.lineHeightOffset = -5f;
                }
                for (int j = 0; j < lists[i].Length; j++)
                {
                    if (tabIndex == 2)
                    {
                        if (lists[i][j] == cfgSaintSubmergedEcho)
                        {
                            color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint);
                        }
                        else if (lists[i][j] == cfgVista_LM)
                        {
                            color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer) * 1.5f;
                        }
                        else if (lists[i][j] == cfgVista_MS)
                        {
                            color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet);
                        }
                        else if (lists[i][j] == cfgVista_OE)
                        {
                            color = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Yellow);
                        }
                        else if (lists[i][j] == cfgHidden_Filter)
                        {
                            color = new Color(0.85f, 0.35f, 0.4f);
                        }
                        else
                        {
                            color = Menu.MenuColorEffect.rgbMediumGrey;
                        }
                    }
                   


                    switch (ValueConverter.GetTypeCategory(lists[i][j].settingType))
                    {
                        case ValueConverter.TypeCategory.Boolean:
                            {
                                num += 30f;
                                Configurable<bool> configurable = lists[i][j] as Configurable<bool>;
                                OpCheckBox opCheckBox = new OpCheckBox(configurable, new Vector2(horizontal, height - num))
                                {
                                    description = OptionInterface.Translate(configurable.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opCheckBox, uiconfig ?? opCheckBox);
                                OpLabel opLabel = new OpLabel(new Vector2(horizontal + 40f, height - num), new Vector2(240f, 30f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opCheckBox.bumpBehav,
                                    description = opCheckBox.description
                                };
                                opCheckBox.colorEdge = color;
                                opLabel.color = color;
                                opTab.AddItems(new UIelement[]
                                {
                                    opCheckBox,
								    opLabel
                                });
                                uiconfig = opCheckBox;
                                break;
                            }
                        case ValueConverter.TypeCategory.Integrals:
                            {
                                num += 36f;
                                Configurable<int> configurable2 = lists[i][j] as Configurable<int>;
                                OpUpdown opUpdown = new OpUpdown(configurable2, new Vector2(horizontal, height - num), 70f)
                                {
                                    description = OptionInterface.Translate(configurable2.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown, uiconfig ?? opUpdown);
                                OpLabel opLabel2 = new OpLabel(new Vector2(horizontal + 80f, height - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable2.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown.bumpBehav,
                                    description = opUpdown.description
                                };
                                opUpdown.colorEdge = color;
                                opLabel2.color = color;
                                opTab.AddItems(new UIelement[]
                                {
                                    opUpdown,
                                    opLabel2
                                });
                                uiconfig = opUpdown;
                                break;
                            }
                        case ValueConverter.TypeCategory.Floats:
                            {
                                Configurable<float> configurable3 = lists[i][j] as Configurable<float>;
                                byte decimalPoints = 1;
                                if (configurable3 == cfgRiv_RainMult || configurable3 == cfgSaintAscendPointPenalty)
                                {
                                    decimalPoints = 2;
                                }

                                num += 36f;
                                OpUpdown opUpdown2 = new OpUpdown(configurable3, new Vector2(horizontal, height - num), 70f, decimalPoints)
                                {
                                    description = OptionInterface.Translate(configurable3.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown2, uiconfig ?? opUpdown2);
                                OpLabel opLabel3 = new OpLabel(new Vector2(horizontal + 80f, height - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable3.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown2.bumpBehav,
                                    description = opUpdown2.description
                                };
                                opUpdown2.colorEdge = color;
                                opLabel3.color = color;
                                opTab.AddItems(new UIelement[]
                                {
                                    opUpdown2,
                                    opLabel3
                                });
                                uiconfig = opUpdown2;
                                break;
                            }
                    }
                }
                if (names != null)
                {
                    height -= 70f;
                }
               
            }
            for (int k = 0; k < lists.Length; k++)
            {
                if (k == 0 || k == 1)
                {
                    lists[k][0].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Up, lists[k][0].BoundUIconfig);
                }
                if (k == 0 || k == lists.Length - 1)
                {
                    lists[k][lists[k].Length - 1].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Down, FocusMenuPointer.GetPointer(FocusMenuPointer.MenuUI.SaveButton));
                }
            }
            int num4 = 0;
            for (int l = 1; l < lists.Length; l++)
            {
                for (int m = 0; m < lists[l].Length; m++)
                {
                    if (lists[l][m].BoundUIconfig != null)
                    {
                        lists[l][m].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Right, lists[l][m].BoundUIconfig);
                        if (num4 < lists[0].Length)
                        {
                            if (lists[0][num4].BoundUIconfig == null)
                            {
                                num4++;
                            }
                            else
                            {
                                UIfocusable.MutualHorizontalFocusableBind(lists[0][num4].BoundUIconfig, lists[l][m].BoundUIconfig);
                                lists[0][num4].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Left, FocusMenuPointer.GetPointer(FocusMenuPointer.MenuUI.CurrentTabButton));
                                num4++;
                            }
                        }
                        else
                        {
                            lists[l][m].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Left, lists[0][lists[0].Length - 1].BoundUIconfig);
                        }
                    }
                }
            }
        }

        private void AddCheckbox()
		{
            var White = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.White);
            var Red = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red);
            var Yellow = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Yellow);
            var Watcher = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Night)*4f;
            Color cheatColor = new Color(0.85f, 0.35f, 0.4f);

            var Arti = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer) * 1.5f;
            var Spear = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Spear) * 1.5f;
            var Saint = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint);
            var Riv = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet);
            var Gourmand = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Gourmand);

            //0,0 is bottom left and looks fine
            //575,575 is top right limit
            //Consider 50 for Y
            //+2.5y for Labels +35x for Labels Checkboxes
            //half of SizeX for UpDowns
            //Xsize defined in UpDowns

           

            #region Main
            ConfigurableBase[][] array = new ConfigurableBase[5][];
            Color[] colors = new Color[]
             {
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
                cheatColor,
             };
            array[0] = new ConfigurableBase[]
            {
                cfgKarmaStart,
                cfgKarmaStartMax,
                cfgMaxKarmaEchos,
                cfgMaxKarmaPebbles,
                cfgMapRevealRadius,
                cfgPassageTeleportation,  
            };
            array[1] = new ConfigurableBase[]
            {
                cfgJukeboxAdditions,
                cfgCustomColorMenu,
                cfgMusicPlayMore,
                //cfgPauseWarning,
            };
            array[2] = new ConfigurableBase[]
            {
                cfgPupsSpawn,
                cfgPupsSpawnNonDefault,
                cfgPupsSpawnFrequently,
            }; 
            array[3] = new ConfigurableBase[]
            {
                cfgKarmaFlower
            };
            array[4] = new ConfigurableBase[]
             {
                cfgRemovePermaDeath
             };
            
            string[] names = new string[]
             {
                "Assists",
                "General",
                "Slugpups",
                "Major Assists",
                "",
             };
            

            instance.PopulateWithConfigs(0, array, names, colors, 2);
            #endregion
            #region Tab Character
            colors = new Color[]
             {
                  Color.white,
                Yellow,
                Gourmand,
                Red,
				Arti,
                Spear,
                Riv,
				Saint,
             };
            array = new ConfigurableBase[][]
            {
                new ConfigurableBase[]
                {
                    cfgSurvivor_StartWithPups,
                },
                new ConfigurableBase[]
                {
                    cfgMonk_CombatScore,
                    //cfgMonk_StartWithPups,
                },
                new ConfigurableBase[]
                {
                    cfgGourmand_StartWithPups,
                },
                new ConfigurableBase[]
                {
                    cfgHunterPlusKarma,
                    cfgStomachPearl,
                },
                new ConfigurableBase[]
                {
                    cfgArtificerRobo,
                    cfgArti_MaxKarmaBool,
                    cfgArti_MaxKarma,
                },
                new ConfigurableBase[]
                {
                     cfgSpearOverseer,
                     //////cfgVista_Waterfront,
                },
                new ConfigurableBase[]
                {
                    cfgRivuletBall,
                   cfgRivuletShortCycles,
			       //cfgRiv_HeavyRainDuration,
			       cfgRiv_HeavyRainChance,
                   cfgRiv_RainMult,
                   cfgRiv_ShelterFailRate,
                   cfgRiv_ShortCyclePointBonus,
                },
                new ConfigurableBase[]
                {
                    cfgSaint_Echoes,
                    cfgSaint_MaxKarmaBool,
                    cfgSaint_MaxKarma,
                    cfgSaint_AscendKill,
                    cfgSaintAscendPointPenalty,
                    cfgSaint_NoSpear,
                    //////cfgSaintSubmergedEcho,                
                },
            };
            names = new string[]
             {
                 "",
                 "",
                 "",
                 "",
                 "",
                "",
                 "",
                 "",
             };

            instance.PopulateWithConfigs(1, array, null, colors, 6);
            #endregion
            #region Challenges
            array = new ConfigurableBase[][]
            {
                new ConfigurableBase[]
                {
                    cfgShowVistaOnMap,
                    cfgVistaCircle,
                    cfgVista_SS,
                    cfgVista_LM,
                    cfgVista_MS,
                    cfgVista_OE,
                },
                new ConfigurableBase[]
                {
                    cfg_HuntChallengeLimit,
                    cfg_HuntChallengeLimitRed,
                    cfg_HuntChallengeLimitRot,
                },
                 new ConfigurableBase[]
                {
                    cfgPupsMotherAchievement,
                    //cfgMonk_CombatScore,
                    //cfgRiv_ShortCyclePointBonus,
                    cfgHoardingBonus,
                    cfgVistaPearlScore,
                    cfgSaintSubmergedEcho,
                    //cfgSaintAscendPointPenalty,
                    cfgRandom_Difficulty,
                },
                 new ConfigurableBase[]
                {
                    cfgHidden_Delivery,
                    cfgHidden_PearlHoard,
                    cfgHidden_Filter,
                },
                new ConfigurableBase[]
                {
                    cfgSnowMeter,
                    cfgRemoveRoboLock,
                }
            };
            names = new string[]
             {
                 "Vista",
                 "Hunting",
                 "Challenges",
                 "Hidden",
                 "Misc",
             };
            instance.PopulateWithConfigs(2, array, names, null, 2);
            #endregion
            #region Cheat
            array = new ConfigurableBase[2][];
            colors = new Color[]
             {
                cheatColor,
                cheatColor,
                cheatColor,
             };
            array[0] = new ConfigurableBase[]
           {
               cfgInfinitePassage,
               cfgBonusPerkSlots,
                cfgUnlockJukebox,
                cfgUnlockEgg,
                cfgUnlockPerkSlots,
                cfgUnlockAll
           };
            array[1] = new ConfigurableBase[]
           {
                cfgTestingMission,
               
           };
            names = new string[]
             {
                "Cheats",
                "Testing",
             };
            Tabs[3].colorCanvas = cheatColor;
            Tabs[3].colorButton = cheatColor;

            instance.PopulateWithConfigs(3, array, names, colors, 1);
            #endregion 

            OpLabel TitleLabel = new OpLabel(new Vector2(150f, 520), new Vector2(300f, 30f), "~Expedition Extra Config~", FLabelAlignment.Center, true, null);
            TitleLabel.description = "Various settings for Expedition Mode";
            TitleLabel.color = White;


 
            this.classicPresetBtn = new OpHoldButton(new Vector2(472f, 0f), new Vector2(110f, 30f), "Set to Vanilla", 30f)
            {
                description = OptionInterface.Translate("Set all values to Vanilla values except for cosmetic settings.")
            };
            this.classicPresetBtn.OnPressDone += this.VanillaPreset;

            this.Tabs[0].AddItems(new UIelement[]
               {
                    TitleLabel,
                    this.classicPresetBtn,
               });
        }


		 
	}
}
