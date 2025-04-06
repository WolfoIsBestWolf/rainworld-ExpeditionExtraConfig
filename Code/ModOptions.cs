using JetBrains.Annotations;
using Menu.Remix;
using Menu.Remix.MixedUI;
using MoreSlugcats;
using RWCustom;
using System;
using UnityEngine;

namespace ExpeditionExtraConfig
{
    public class WConfig : OptionInterface
    {

		public static WConfig instance = new WConfig();

        /*public static Configurable<bool> cfgPassageTeleportation = instance.config.Bind("cfgPassageTeleportation", false, 
        new ConfigurableInfo("Allow completed passages to be used as teleports. In Expedition an unlock, where you get teleports for challenges, is a inteded replacement.", null, "", new object[]
            {
                "Passage Teleportation"
            }));*/
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
		public static Configurable<int> cfgKarmaCapStart = instance.config.Bind("cfgKarmaCapStart", 5, 
            new ConfigurableInfo("Change starting Max Karma. Vanilla is 5.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
			{
				"Starting Max Karma"
			}));

        public static Configurable<bool> cfgRemovePermaDeath = instance.config.Bind("cfgRemovePermaDeath", false, 
            new ConfigurableInfo("Expeditions no longer end when dying at 1 Karma. This does not apply to the Doomed Burden.", null, "", new object[]
            {
                "Remove Perma Death"
            }));
        public static Configurable<bool> cfgUnlockAll = instance.config.Bind("cfgUnlockAll", false, 
            new ConfigurableInfo("Makes things not require unlocks. Intended for testing. Does not actually add unlock progress or achievements", null, "", new object[]
            {
                "Unlock Everything"
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

        #region Rivulet
        public static Configurable<bool> cfgRivuletBall = instance.config.Bind("cfgRivuletBall", false,
            new ConfigurableInfo("Rivulet starts with the Rarefaction Cell. Not all too useful but funny.", null, "", new object[]
            {
                "Rarefaction Cell Start"
            }));

        public static Configurable<bool> cfgRivuletShortCycles = instance.config.Bind("cfgRivuletShortCycles", true, 
            new ConfigurableInfo("Shorter Cycles and more Shelter Failures like the start of Rivulets campaign. To make his expeditions feel more different. Cycle Point challenges will require less points.", null, "", new object[]
			{
				"Rivulet Shorter Cycles" 
			}));

        /*public static Configurable<int> cfgRiv_HeavyRainDuration = instance.config.Bind("cfgRiv_HeavyRainDuration", 0,
          new ConfigurableInfo("At the start of a Rivulet Expedition, how many cycles should be shorter.", new ConfigAcceptableRange<int>(0, 200), "", new object[]
          {
                "Strong Rain Duration"
          }));*/

        public static Configurable<int> cfgRiv_HeavyRainChance = instance.config.Bind("cfgRiv_HeavyRainChance", 30,
           new ConfigurableInfo("Chance for a cycle to randomly be shorter and have likelier shelter failure. It will not get rerolled by dying.", new ConfigAcceptableRange<int>(16, 100), "", new object[]
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
            new ConfigurableInfo("Rivulet gets 1.1x score on challenges that involve long travel. To make up for some cycles being shorter.", null, "", new object[]
        {
                "Travel Bonus Score"
        }));
        #endregion


        #region Saint
        public static Configurable<bool> cfgSaint_AscendKill = instance.config.Bind("cfgSaint_AscendKill", true,
         new ConfigurableInfo("Ascending usually does not count as murder, and would be rather meanginless for challenges.", null, "", new object[]
         {
                "Ascend Kills"
         }));

        public static Configurable<float> cfgSaintAscendPointPenalty = instance.config.Bind("cfgSaintAscendPointPenalty", 1f, new ConfigurableInfo("Saint gets 1.35x score multiplier for combat challenges. This multiplier will be used instead if you Ascended creatures as it's easier. (Only applies when run is finished with 10 max Karma)", new ConfigAcceptableRange<float>(0.2f, 1.35f), "", new object[]
        {
                "Ascension Point penalty"
        }));

        public static Configurable<bool> cfgSaint_Echoes = instance.config.Bind("cfgSaint_Echoes", true, 
			new ConfigurableInfo("Allow Saint to encounter Echos regardless of Karma and without previous visits like in the normal game. Can't visit Echos on cycle 0.", null, "", new object[]
			{
				"Saint Echo mechanics"
			}));
        public static Configurable<bool> cfgSaintSubmergedEcho = instance.config.Bind("cfgSaintSubmergedEcho", false, 
            new ConfigurableInfo("Allow Echo encounters to choose the Submerged Superstructure Echo for Saint", null, "", new object[]
        {
                "Submerged Superstructure Echo"
        }));

        public static Configurable<bool> cfgSaint_MaxKarmaBool = instance.config.Bind("cfgSaint_MaxKarmaBool", false,
           new ConfigurableInfo("Should Saint start with less Karma in Expedition mode", null, "", new object[]
           {
                "Different Max Karma"
           }));

        public static Configurable<int> cfgSaint_MaxKarma = instance.config.Bind("cfgSaint_MaxKarma", 6,
           new ConfigurableInfo("Saint starting max Karma amount for above config. This can make it easier to reach Karma 10 with Echoes if that is enabled.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
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

 

		public static Configurable<bool> cfgHiddenDelveries = instance.config.Bind("cfgHiddenDelveries", false, 
            new ConfigurableInfo("Allow Pearl Deliveries, Pearl Hoarding, Neuron Delvieries to be chosen for Hidden challenges again. It'll only chose low difficulty amounts if hidden.",null, "", new object[]
			{
				"Hidden Deliviers"
			}));

        public static Configurable<bool> cfgPauseWarning = instance.config.Bind<bool>("cfgPauseWarning", false, 
            new ConfigurableInfo("Show the exit warning about losing Karma sooner.", null, "", new object[]
            {
                "Early exit warning"
            }));
        public static Configurable<bool> cfgMusicPlayMore = instance.config.Bind<bool>("cfgMusicPlayMore", false, 
            new ConfigurableInfo("Music triggers play every cycle instead of having a 5 Cycle Cooldown. Multiple tracks can also play in one cycle.", null, "", new object[]
            {
                "Music plays more often"
            }));




        //public static Configurable<bool> cfgPassageTeleports; //Apparently vanill as an unlock
        public static Configurable<float> cfgMapRevealRadius = instance.config.Bind("cfgMapRevealRadius", 2f,
           new ConfigurableInfo("Map Reveal Radius multiplier during Expedition. To more easily fill out the map in Expedition so you can focus more on your objectives. Also because I cant figure out how to transfer map progress.", new ConfigAcceptableRange<float>(1f, 50f), "", new object[]
           {
                "Map Reveal Radius multiplier"
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
            new ConfigurableInfo("During Expeditions, guarantee a Slugpup every 10 cycles instead of every 25. For Hunter it is 5 Cycles regardless of config.", null, "", new object[]
            {
                "Frequent Slugpups"
            }));
        public static Configurable<bool> cfgPupsMotherAchievement = instance.config.Bind("cfgPupsMotherAchievement", false,
            new ConfigurableInfo("Allow Mother Passage as a possible Objective. This is disabled in vanilla because of no Slugpups. The randomness of Pups may make this annoying rather than challenge.", null, "", new object[]
            {
                "Mother Passage"
            }));


        public static Configurable<bool> cfgMonk_StartWithPups = instance.config.Bind("cfgMonk_StartWithPups", false,
          new ConfigurableInfo("Monk will start his Expedition with 2 Pups, if pups are enabled.", null, "", new object[]
          {
                "Start with pups"
          }));
        public static Configurable<bool> cfgGourmand_StartWithPups = instance.config.Bind("cfgGourmand_StartWithPups", false,
          new ConfigurableInfo("Gourmand will start his Expedition with 2 Pups, if pups are enabled.", null, "", new object[]
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
           new ConfigurableInfo("Should Artificer start with less Karma in Expedition mode", null, "", new object[]
           {
                "Different Max Karma"
           }));

        public static Configurable<int> cfgArti_MaxKarma = instance.config.Bind("cfgArti_MaxKarma", 4,
           new ConfigurableInfo("Artificer starting Max Karma amount for above config. This can force her to use Scav Corpses to go through some gates.", new ConfigAcceptableRange<int>(1, 10), "", new object[]
           {
                "Starting Max Karma"
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

   

 

 

		public override void Initialize()
		{
			base.Initialize();
			this.Tabs = new OpTab[]
			{
                new OpTab(this, "General"),
				new OpTab(this, "Character")
            };
			this.AddCheckbox();


            cfgUnlockAll.OnChange += CfgUnlockAll_OnChange;
            CfgUnlockAll_OnChange();
        }

        private void CfgUnlockAll_OnChange()
        {
            if (cfgUnlockAll.Value && UnlockAll.added == false)
            {
                UnlockAll.Add();
            }
            else if (cfgUnlockAll.Value == false && UnlockAll.added)
            {
                UnlockAll.Remove();
            }
        }

        private void PopulateWithConfigs(int tabIndex, ConfigurableBase[][] lists, [CanBeNull] string[] names, [CanBeNull] Color[] colors, int splitAfter)
        {
            new OpLabel(new Vector2(100f, 560f), new Vector2(400f, 30f), this.Tabs[tabIndex].name, FLabelAlignment.Center, true, null);
            OpTab opTab = this.Tabs[tabIndex];
            float num = 40f;
            float num2 = 20f;
            float num3 = tabIndex == 1 ? 540f : 500f;
            UIconfig uiconfig = null;
            for (int i = 0; i < lists.Length; i++)
            {
				if (names != null)
				{
                    var label = new OpLabel(new Vector2(num2, num3 - num + 10f), new Vector2(260f, 30f), "~ " + names[i] + " ~", FLabelAlignment.Center, true, null);
                    if (colors != null)
                    {
                        label.color = colors[i];
                    }
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
                    switch (ValueConverter.GetTypeCategory(lists[i][j].settingType))
                    {
                        case ValueConverter.TypeCategory.Boolean:
                            {
                                num += 30f;
                                Configurable<bool> configurable = lists[i][j] as Configurable<bool>;
                                OpCheckBox opCheckBox = new OpCheckBox(configurable, new Vector2(num2, num3 - num))
                                {
                                    description = OptionInterface.Translate(configurable.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opCheckBox, uiconfig ?? opCheckBox);
                                OpLabel opLabel = new OpLabel(new Vector2(num2 + 40f, num3 - num), new Vector2(240f, 30f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opCheckBox.bumpBehav,
                                    description = opCheckBox.description
                                };
								if (colors != null)
								{
									opCheckBox.colorEdge = colors[i];
									opLabel.color = colors[i];
                                }
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
                                OpUpdown opUpdown = new OpUpdown(configurable2, new Vector2(num2, num3 - num), 70f)
                                {
                                    description = OptionInterface.Translate(configurable2.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown, uiconfig ?? opUpdown);
                                OpLabel opLabel2 = new OpLabel(new Vector2(num2 + 80f, num3 - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable2.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown.bumpBehav,
                                    description = opUpdown.description
                                };
                                if (colors != null)
                                {
                                    opUpdown.colorEdge = colors[i];
                                    opLabel2.color = colors[i];
                                }
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
                                OpUpdown opUpdown2 = new OpUpdown(configurable3, new Vector2(num2, num3 - num), 70f, decimalPoints)
                                {
                                    description = OptionInterface.Translate(configurable3.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown2, uiconfig ?? opUpdown2);
                                OpLabel opLabel3 = new OpLabel(new Vector2(num2 + 80f, num3 - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable3.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown2.bumpBehav,
                                    description = opUpdown2.description
                                };
                                if (colors != null)
                                {
                                    opUpdown2.colorEdge = colors[i];
                                    opLabel3.color = colors[i];
                                }
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
                    num3 -= 70f;
                }
                if (i == splitAfter)
                {
                    num2 += 300f;
                    num3 = tabIndex == 1 ? 540f : 500f;
                    num = 40f;
                    uiconfig = null;
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
            var Watch = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Night)*2.2f;
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
 

            ConfigurableBase[][] array = new ConfigurableBase[4][];
            Color[] colors = new Color[]
             {
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
                cheatColor,
             };
            array[0] = new ConfigurableBase[]
            {
                cfgKarmaStart,
                cfgKarmaCapStart,
                cfgMaxKarmaEchos,
                cfgMaxKarmaPebbles,
                cfgKarmaFlower,
               
            };
            array[1] = new ConfigurableBase[]
            {
                cfgMusicPlayMore,
                cfgPauseWarning,
                //cfgScoreDisplay,
                //cfgColorMenu,
            };
            array[2] = new ConfigurableBase[]
            {
                cfgHiddenDelveries,
                cfgMapRevealRadius,
                cfgPupsSpawn,
                cfgPupsSpawnNonDefault,
                cfgPupsSpawnFrequently,
                cfgPupsMotherAchievement,
                //cfgPassageTeleportation,
            };
            array[3] = new ConfigurableBase[]
            {
                cfgRemovePermaDeath,
                cfgUnlockAll
            };
            string[] names = new string[]
             {
                "Karma",
                "Other",
                "General",
                "Cheats",   
             };
			instance.PopulateWithConfigs(0, array, names, colors, 1);
		 



            array = new ConfigurableBase[7][];
            colors = new Color[]
             {
                Yellow,
                Gourmand,
                Red,
				Arti,
				Spear,
				Riv,
				Saint,
             };
            array[0] = new ConfigurableBase[]
            {
               cfgMonk_CombatScore,
               cfgMonk_StartWithPups,
            };
            array[1] = new ConfigurableBase[]
           {
                cfgGourmand_StartWithPups,
           };
            array[2] = new ConfigurableBase[]
            {
                cfgHunterPlusKarma,
                cfgStomachPearl,
            };
            array[3] = new ConfigurableBase[]
            {
                cfgArtificerRobo,
                cfgArti_MaxKarmaBool,
                cfgArti_MaxKarma,
            };
            array[4] = new ConfigurableBase[]
            {
                cfgSpearOverseer,
            };
            array[5] = new ConfigurableBase[]
           {
               cfgRivuletBall,
			   cfgRivuletShortCycles,
			   //cfgRiv_HeavyRainDuration,
			   cfgRiv_HeavyRainChance,
               cfgRiv_RainMult,
			   cfgRiv_ShelterFailRate,
			   cfgRiv_ShortCyclePointBonus,
           };
            array[6] = new ConfigurableBase[]
           {
                cfgSaint_Echoes,
                cfgSaint_AscendKill,
                cfgSaintAscendPointPenalty,
                cfgSaint_MaxKarmaBool,
                cfgSaint_MaxKarma,
           };
            instance.PopulateWithConfigs(1, array, null, colors, 4);



            OpLabel TitleLabel = new OpLabel(new Vector2(150f, 520), new Vector2(300f, 30f), "~Expedition Extra Config~", FLabelAlignment.Center, true, null);
            TitleLabel.description = "Various settings for Expedition Mode";
            TitleLabel.color = White;

            /*OpLabel Title = new OpLabel(200, 560, "~Expedition Extra Config~", true) //300 true center I guess
			{
                description = "Various settings for Expedition Mode",
                color = Gourmand
            };*/
			

            this.Tabs[0].AddItems(new UIelement[]
            {
                TitleLabel,
            });

            return;
	 
		}


		 
	}
}
