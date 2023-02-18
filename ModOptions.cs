using BepInEx;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Collections.Generic;
using MoreSlugcats;
using Menu.Remix.MixedUI;

namespace ExpeditionExtraConfig
{
    public class EECSettings : OptionInterface
    {
	

		public static EECSettings instance = new EECSettings();

		
		public static Configurable<bool> cfgKarmaFlower = instance.config.Bind("cfgKarmaFlower", false, new ConfigurableInfo("Normally Karma Flowers do not spawn. This will make expeditions significantly easier. Vanilla is false. Hunter still wont be able to find them.", null, "", new object[]
			{
				"Natural Karma Flowers"
			}));
		public static Configurable<bool> cfgGhostsIncreaseKarma = instance.config.Bind("cfgGhostsIncreaseKarma", false, new ConfigurableInfo("Normally Echos do not increase Max Karma. This will make expeditions somewhat easier and allow Saint to reach his potential", null, "", new object[]
			{
				"Echos increase max Karma"
			}));
		public static Configurable<bool> cfgPebblesIncreaseKarma = instance.config.Bind("cfgPebblesIncreaseKarma", false, new ConfigurableInfo("Allow Pebbles to increase Karma as he would in the normal game. Increase by 1 for Hunter/Gourmand and set to max for Survivor/Monk.", null, "", new object[]
					{
				"Pebbles increases max Karma"
					}));

		public static Configurable<int> cfgKarmaStart = instance.config.Bind("cfgKarmaStart", 2, new ConfigurableInfo("Change starting Karma. Vanilla is 2. Ranges from 1 to 10", new ConfigAcceptableRange<int>(1, 10), "", new object[]
			{
				"Starting Karma"
			}));
		public static Configurable<int> cfgKarmaCapStart = instance.config.Bind("cfgKarmaCapStart", 5, new ConfigurableInfo("Change starting Karma. Vanilla is 5. Ranges from 1 to 10", new ConfigAcceptableRange<int>(1, 10), "", new object[]
			{
				"Starting Karma Cap"
			}));


		public static Configurable<bool> cfgArtificerRobo = instance.config.Bind("cfgArtificerRobo", true, new ConfigurableInfo("Artificer starts with his drone. This should be purely cosmetic", null, "", new object[]
			{
				"Artificer Drone"
			}));
		public static Configurable<bool> cfgSpearOverseer = instance.config.Bind("cfgSpearOverseer", true, new ConfigurableInfo("Spearmaster will be followed by his Red Overseer. This should have no gameplay effect.", null, "", new object[]
			{
				"Spearmaster Overseer"
			}));
		public static Configurable<bool> cfgRivuletShortCycles = instance.config.Bind("cfgRivuletShortCycles", false, new ConfigurableInfo("Shorter Rain Cycles and more Shelter Failures like the start of Rivulets campaign. Cycle Point challenges will have lower targets as to not become unbeatable.", null, "", new object[]
			{
				"Rivulet Shorter Cycles" 
			}));
		/*public static Configurable<float> cfgRivuletBonusRain = instance.config.Bind("cfgRivuletBonusRain", 1f, new ConfigurableInfo("Multiplies Rivulets shorter Cycles up to the duration of the normal cycles.", new ConfigAcceptableRange<float>(1f, 3f), "", new object[]
			{
				"Rivulet Lesser Short Cycles"
			}));*/
		public static Configurable<float> cfgRivuletMulti = instance.config.Bind("cfgRivuletMulti", 0.50f, new ConfigurableInfo("Rain Multiplier for most regions for Riv Shorter Cycles in Expedition. Vanilla 0.33, recommended 0.5", new ConfigAcceptableRange<float>(0.25f, 1f), "", new object[]
			{
				"Rain Multiplier"
			}));
		public static Configurable<float> cfgRivuletMultiRegional = instance.config.Bind("cfgRivuletMultiRegional", 0.50f, new ConfigurableInfo("Rain Multiplier for Pipeyard/Exterior/Shaded/Subterranean/Shoreline for Riv Shorter Cycles in Expedition. Vanilla 0.5", new ConfigAcceptableRange<float>(0.25f, 1f), "", new object[]
			{
				"Rain Multiplier"
			}));
		public static Configurable<float> cfgRivuletShelterRate = instance.config.Bind("cfgRivuletShelterRate", 25f, new ConfigurableInfo("Shelter Fail rates for most regions for Riv Shorter Cycles in Expedition. Vanilla 25", new ConfigAcceptableRange<float>(0f, 100f), "", new object[]
	{
				"Shelter Fail rates"
	}));
		public static Configurable<float> cfgRivuletShelterRateRegional = instance.config.Bind("cfgRivuletShelterRateRegional", 25f, new ConfigurableInfo("Shelter Fail rates for Pipeyard/Shoreline/Shaded/Garbage for Riv Shorter Cycles in Expedition. Vanilla 40, recommended 25", new ConfigAcceptableRange<float>(0f, 100f), "", new object[]
			{
				"Shelter Fail rates"
			}));
		public static Configurable<bool> cfgSaintAscendPoints = instance.config.Bind("cfgSaintAscendPoints", false, new ConfigurableInfo("Ascending things will grant points. This can make combat challenges a cakewalk. Level 10 Karma can only be gotten if other settings are enabled.", null, "", new object[]
			{
				"Saint Ascension gives Points"
			}));

		public static Configurable<bool> cfgEchoArtificer = instance.config.Bind("cfgEchoArtificer", false, new ConfigurableInfo("Should Artificer encounter Echos only at max Karma. This only makes Expeditions harder. In vanilla you need a Karma Flower too, but as they dont spawn only max Karma for this setting.", null, "", new object[]
			{
				"Artificer Echo mechanics"
			}));

		public static Configurable<bool> cfgEchoSaint = instance.config.Bind("cfgEchoSaint", false, new ConfigurableInfo("Should Saint encounter Echos regardless of Karma or previous visits like in the normal game. Can't visit Echos on cycle 0.", null, "", new object[]
			{
				"Saint Echo mechanics"
			}));
		public static Configurable<bool> cfgRivuletBall = instance.config.Bind("cfgRivuletBall", false, new ConfigurableInfo("Rivulet starts with a Rarefaction Cell. This is purely for fun and with a lot of challenges not worth keeping in your hands. The one in Pebbles and Moon will still be there. Rivulet be ballin.", null, "", new object[]
			{
				"Rarefaction Cell Start"
			}));

		public static Configurable<bool> cfgStomachPearl = instance.config.Bind("cfgStomachPearl", false, new ConfigurableInfo("Hunter and Rivulet start with their Pearl as a headstart for Chieftain or other challenges", null, "", new object[]
			{
				"Stomach Pearl"
			}));
		public static Configurable<int> cfgHunterPlusKarma = instance.config.Bind("cfgHunterPlusKarma", 0, new ConfigurableInfo("Hunter starts with extra Karma like in his campaign. Vanilla would be +2.", new ConfigAcceptableRange<int>(0, 5), "", new object[]
			{
				"Hunter bonus starting Karma"
			}));
		public static Configurable<bool> cfgMoreRegions = instance.config.Bind("cfgMoreRegions", false, new ConfigurableInfo("Allow Pearl Delivery to choose Waterfront for Spear/Arti, Submerged Superstructure for Rivulet and it's Echo for Saint", null, "", new object[]
			{
				"MS and LM content"
			}));
		public static Configurable<bool> cfgBetterBlacklist = instance.config.Bind("cfgBetterBlacklist", false, new ConfigurableInfo("Blacklist Mother Long Legs due to creating impossible hunts. Singularity Bombs arent always possible to get. Gourmand Crafting + Natural Karma flowers could be a way however.", null, "", new object[]
			{
				"Blacklist MLL"
			}));
		/*public static Configurable<bool> cfgAdjustPoints = instance.config.Bind("cfgAdjustPoints", false, new ConfigurableInfo("Get more points when doing Riv Short Cycle and less points if you chose to Ascend hunts instead of killing", null, "", new object[]
			{
				"Adjust Points"
			}));*/
		public static Configurable<float> cfgRivuletShortCycleBonus = instance.config.Bind("cfgRivuletShortCycleBonus", 1.1f, new ConfigurableInfo("Bonus point for long distance challenges like Pearl Deliveries, Hoarding, Vistas when Riv Short Cycles is enabled.", new ConfigAcceptableRange<float>(1f, 1.3f), "", new object[]
			{
				"Short Cycles Bonus Points"
			}));
		public static Configurable<float> cfgSaintAscendPointPenalty = instance.config.Bind("cfgSaintAscendPointPenalty", 0.8f, new ConfigurableInfo("Saint has a 1.35x score multiplier for combat challenges. This multiplier will be used instead if you Ascended creatures as it's easier. (Only applies when run is finished with 9 max Karma)", new ConfigAcceptableRange<float>(0.2f, 1.35f), "", new object[]
			{
				"Ascension Point penalty"
			}));
		/*public static Configurable<bool> cfgArtificerScavKing = instance.config.Bind("cfgArtificerScavKing", true, new ConfigurableInfo("Scavenger King will spawn in his dome and end the expedition similiar to entering Depths", null, "", new object[]
			{
				"Scav King End"
			}));*/
		/*public static Configurable<bool> cfgRivuletCycleScore = instance.config.Bind("cfgRivuletCycleScore", true, new ConfigurableInfo("Cycle Score challenges will have lower requirements for Rivulet to account for shorter Cycles. ", null, "", new object[]
			{
				"Easier Cycle Score"
			}));*/


		public override void Initialize()
		{
			base.Initialize();
			this.Tabs = new OpTab[]
			{
				new OpTab(this, "Options")
			};
			this.AddCheckbox();
		}

		private void AddCheckbox()
		{
			//0,0 is bottom left and looks fine
			//575,575 is top right limit
			//Consider 50 for Y
			//+2.5y for Labels +35x for Labels Checkboxes
			//half of SizeX for UpDowns
			//Xsize defined in UpDowns
			float lableY = 2.5f;
			float labelX = 35f;
			float stackY = 40f;
			int stk = 0;
			//float stack = 0;

			float YLayer = 500;
			float XLayer = 50;

			OpLabel Title = new OpLabel(XLayer, 560, "Expedition Extra Config", true) //300 true center I guess
			{
				description = "Various settings for Expedition Mode",
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
			};
			//Title.label.anchorX = 0.5f; //Because setting allignment doesn't work

			XLayer = 350f;
			stk = 0;
			OpUpdown OptHunterPlusKarma = new OpUpdown(cfgHunterPlusKarma, new Vector2(XLayer - 25, YLayer - stackY * stk), 50f)
			{
				description = cfgHunterPlusKarma.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red)
			};
			OpLabel LabelHunterPlusKarma = new OpLabel(XLayer + labelX, YLayer + lableY, cfgHunterPlusKarma.info.Tags[0] as string, false)
			{
				description = cfgHunterPlusKarma.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red)
			};
			stk++;
			OpCheckBox OptStomachPearl = new OpCheckBox(cfgStomachPearl, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgStomachPearl.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red)
			};
			OpLabel LabelStomachPearl = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgStomachPearl.info.Tags[0] as string, false)
			{
				description = cfgStomachPearl.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red)
			};
			stk++;
			OpCheckBox OptArtificerRobo = new OpCheckBox(cfgArtificerRobo, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgArtificerRobo.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer),
			};
			OpLabel LabelArtificerRobo = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgArtificerRobo.info.Tags[0] as string, false)
			{
				description = cfgArtificerRobo.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer)
			};
			stk++;
			OpCheckBox OptEchoArtificer = new OpCheckBox(cfgEchoArtificer, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgEchoArtificer.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer),
			};
			OpLabel LabelEchoArtificer = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgEchoArtificer.info.Tags[0] as string, false)
			{
				description = cfgEchoArtificer.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Artificer)
			};
			stk++;
			OpCheckBox OptSpearOverseer = new OpCheckBox(cfgSpearOverseer, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgSpearOverseer.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Spear),
			};
			OpLabel LabelSpearOverseer = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgSpearOverseer.info.Tags[0] as string, false)
			{
				description = cfgSpearOverseer.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Spear)
			};
			stk++;
			OpCheckBox OptRivRain = new OpCheckBox(cfgRivuletShortCycles, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgRivuletShortCycles.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivRain = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgRivuletShortCycles.info.Tags[0] as string, false)
			{
				description = cfgRivuletShortCycles.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};
			stk++;
			OpUpdown OptRivRainMulti1 = new OpUpdown(cfgRivuletMulti, new Vector2(XLayer, YLayer - stackY * stk), 60f, 2)
			{
				description = cfgRivuletMulti.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpUpdown OptRivRainMulti2 = new OpUpdown(cfgRivuletMultiRegional, new Vector2(XLayer + 70, YLayer - stackY * stk), 60f, 2)
			{
				description = cfgRivuletMultiRegional.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivRainMulti = new OpLabel(XLayer + labelX + 105, YLayer + lableY - stackY * stk, cfgRivuletMulti.info.Tags[0] as string, false)
			{
				description = "Rain multipliers used for the Rivulet Shorter Cycles setting.",
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};
			stk++;
			OpUpdown OptRivRainShelter1 = new OpUpdown(cfgRivuletShelterRate, new Vector2(XLayer, YLayer - stackY * stk), 60f, 0)
			{
				description = cfgRivuletShelterRate.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpUpdown OptRivRainShelter2 = new OpUpdown(cfgRivuletShelterRateRegional, new Vector2(XLayer + 70, YLayer - stackY * stk), 60f, 0)
			{
				description = cfgRivuletShelterRateRegional.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivRainShelter = new OpLabel(XLayer + labelX + 105, YLayer + lableY - stackY * stk, cfgRivuletShelterRate.info.Tags[0] as string, false)
			{
				description = "Shelter fail rates used for the Rivulet Shorter Cycles setting.",
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};
			/*stk++;
			OpLabel LabelRivRainMulti2 = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgRivuletMultiRegional.info.Tags[0] as string, false)
			{
				description = cfgRivuletMultiRegional.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};*/
			stk++;
			OpCheckBox OptRivuletBall = new OpCheckBox(cfgRivuletBall, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgRivuletBall.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivuletBall = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgRivuletBall.info.Tags[0] as string, false)
			{
				description = cfgRivuletBall.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};
			stk++;
			OpCheckBox OptSaintPoints = new OpCheckBox(cfgSaintAscendPoints, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgSaintAscendPoints.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint),
			};
			OpLabel LabelSaintPoints = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgSaintAscendPoints.info.Tags[0] as string, false)
			{
				description = cfgSaintAscendPoints.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint)
			};
			stk++;
			OpCheckBox OptEchoSaint = new OpCheckBox(cfgEchoSaint, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgEchoSaint.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint),
			};
			OpLabel LabelEchoSaint = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgEchoSaint.info.Tags[0] as string, false)
			{
				description = cfgEchoSaint.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint)
			};

			
			XLayer = 50;

			OpUpdown OptSaintAscendPointPenalty = new OpUpdown(cfgSaintAscendPointPenalty, new Vector2(XLayer - 35f, YLayer - stackY * stk), 60f, 2)
			{
				description = cfgSaintAscendPointPenalty.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint),
			};
			OpLabel LabelSaintAscendPointPenalty = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgSaintAscendPointPenalty.info.Tags[0] as string, false)
			{
				description = cfgSaintAscendPointPenalty.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint)
			};
			stk--;
			OpUpdown OptRivuletShortCycleBonus = new OpUpdown(cfgRivuletShortCycleBonus, new Vector2(XLayer - 35f, YLayer - stackY * stk), 60f, 2)
			{
				description = cfgRivuletShortCycleBonus.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivuletShortCycleBonus = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgRivuletShortCycleBonus.info.Tags[0] as string, false)
			{
				description = cfgRivuletShortCycleBonus.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};
			stk--;
			OpCheckBox OptMoreRegions = new OpCheckBox(cfgMoreRegions, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgMoreRegions.info.description
			};
			OpLabel LabelMoreRegions = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgMoreRegions.info.Tags[0] as string, false)
			{
				description = cfgMoreRegions.info.description
			};
			stk--;
			OpCheckBox OptBetterBlacklist = new OpCheckBox(cfgBetterBlacklist, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgBetterBlacklist.info.description
			};
			OpLabel LabelBetterBlacklist = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgBetterBlacklist.info.Tags[0] as string, false)
			{
				description = cfgBetterBlacklist.info.description
			};
			stk = 0;
			OpUpdown OptKarmaStart = new OpUpdown(cfgKarmaStart, new Vector2(XLayer -25, YLayer- stackY * stk), 50f)
			{
				description = cfgKarmaStart.info.description
			};
			OpLabel LabelKarmaStart = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgKarmaStart.info.Tags[0] as string, false)
			{
				description = cfgKarmaStart.info.description
			};
			stk++;
			OpUpdown OptKarmaCapStart = new OpUpdown(cfgKarmaCapStart, new Vector2(XLayer-25, YLayer - stackY*stk), 50f)
			{
				description = cfgKarmaCapStart.info.description
			};
			OpLabel LabelKarmaCapStart = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgKarmaCapStart.info.Tags[0] as string, false)
			{
				description = cfgKarmaCapStart.info.description
			};
			stk++;
			OpCheckBox OptKarmaFlower = new OpCheckBox(cfgKarmaFlower, new Vector2(XLayer, YLayer - stackY*stk))
			{
				description = cfgKarmaFlower.info.description,
			};
			OpLabel LabelKarmaFlower = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgKarmaFlower.info.Tags[0] as string, false)
			{
				description = cfgKarmaFlower.info.description,
			};
			stk++;
			OpCheckBox OptGhostsIncreaseKarma = new OpCheckBox(cfgGhostsIncreaseKarma, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgGhostsIncreaseKarma.info.description,
			};
			OpLabel LabelGhostsIncreaseKarma = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgGhostsIncreaseKarma.info.Tags[0] as string, false)
			{
				description = cfgGhostsIncreaseKarma.info.description,
			};
			stk++;
			OpCheckBox OptPebblesIncreaseKarma = new OpCheckBox(cfgPebblesIncreaseKarma, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgPebblesIncreaseKarma.info.description,
			};
			OpLabel LabelPebblesIncreaseKarma = new OpLabel(XLayer + labelX, YLayer + lableY - stackY * stk, cfgPebblesIncreaseKarma.info.Tags[0] as string, false)
			{
				description = cfgPebblesIncreaseKarma.info.description,
			};

			this.Tabs[0].AddItems(new UIelement[]
			{
				Title,
				OptRivuletShortCycleBonus,
				LabelRivuletShortCycleBonus,
				OptSaintAscendPointPenalty,
				LabelSaintAscendPointPenalty,
				OptBetterBlacklist,
				LabelBetterBlacklist,
				OptMoreRegions,
				LabelMoreRegions,
				OptHunterPlusKarma,
				LabelHunterPlusKarma,
				OptKarmaStart,
				LabelKarmaStart,
				OptKarmaCapStart,
				LabelKarmaCapStart,
				OptKarmaFlower,
				LabelKarmaFlower,
				OptGhostsIncreaseKarma,
				LabelGhostsIncreaseKarma,
				OptPebblesIncreaseKarma,
				LabelPebblesIncreaseKarma,
				OptStomachPearl,
				LabelStomachPearl,
				OptArtificerRobo,
				LabelArtificerRobo,
				OptEchoArtificer,
				LabelEchoArtificer,
				OptSpearOverseer,
				LabelSpearOverseer,
				OptRivRain,
				LabelRivRain,
				OptRivuletBall,
				LabelRivuletBall,
				OptRivRainMulti1,
				LabelRivRainMulti,
				OptRivRainMulti2,
				OptRivRainShelter1,
				LabelRivRainShelter,
				OptRivRainShelter2,
				//LabelRivRainMulti2,
				OptSaintPoints,
				LabelSaintPoints,
				OptEchoSaint,
				LabelEchoSaint,
			});
		}



	}
}
