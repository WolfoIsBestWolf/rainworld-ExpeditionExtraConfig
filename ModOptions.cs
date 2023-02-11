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

		
		public static Configurable<bool> cfgKarmaFlower = instance.config.Bind("cfgKarmaFlower", true, new ConfigurableInfo("Normally Karma Flowers do not spawn. This will make expeditions significantly easier. Vanilla is false.", null, "", new object[]
			{
				"Natural Karma Flowers"
			}));
		public static Configurable<bool> cfgGhostsIncreaseKarma = instance.config.Bind("cfgGhostsIncreaseKarma", true, new ConfigurableInfo("Normally Echos do not increase Max Karma. This will make expeditions somewhat easier and allow Saint to reach his potential", null, "", new object[]
			{
				"Echos increase max Karma"
			}));

		public static Configurable<int> cfgKarmaStart = instance.config.Bind("cfgKarmaStart", 1, new ConfigurableInfo("Change starting Karma. Vanilla is 1. Ranges from 0 to 9", new ConfigAcceptableRange<int>(0, 9), "", new object[]
			{
				"Starting Karma"
			}));
		public static Configurable<int> cfgKarmaCapStart = instance.config.Bind("cfgKarmaCapStart", 4, new ConfigurableInfo("Change starting Karma. Vanilla is 4. Ranges from 0 to 9", new ConfigAcceptableRange<int>(0, 9), "", new object[]
			{
				"Starting Karma Cap"
			}));


		public static Configurable<bool> cfgArtificerRobo = instance.config.Bind("cfgArtificerRobo", true, new ConfigurableInfo("Artificer starts with his drone. This should be purely cosmetic", null, "", new object[]
			{
				"Artificer Drone"
			}));
		public static Configurable<bool> cfgSpearOverseer = instance.config.Bind("cfgSpearOverseer", true, new ConfigurableInfo("Spearmaster will be followed by his Red Overseer. This has purely cosmetic.", null, "", new object[]
			{
				"Spearmaster Overseer"
			}));
		public static Configurable<bool> cfgRivuletShortCycles = instance.config.Bind("cfgRivuletShortCycles", true, new ConfigurableInfo("Shorter Rain Cycles and more Shelter Failures like the start of Rivulets campaign. The Refract Core will be in it's usual place. Challenges like Points gained in a cycle will be made more difficult if not impossible unless you steal the Refract Core", null, "", new object[]
			{
				"Rivulet Shorter Cycles" 
			}));
		public static Configurable<float> cfgRivuletBonusRain = instance.config.Bind("cfgRivuletBonusRain", 1f, new ConfigurableInfo("Multiplies Rivulets shorter Cycles up to the duration of the normal cycles.", new ConfigAcceptableRange<float>(1f, 3f), "", new object[]
			{
				"Rivulet Lesser Short Cycles"
			}));
		public static Configurable<bool> cfgSaintAscendPoints = instance.config.Bind("cfgSaintAscendPoints", true, new ConfigurableInfo("Ascending things will grant points. This can make combat challenges a cakewalk. Level 10 Karma can only be gotten if other settings are enabled.", null, "", new object[]
			{
				"Saint Ascension gives Points"
			}));

		public static Configurable<bool> cfgStomachPearl = instance.config.Bind("cfgStomachPearl", true, new ConfigurableInfo("Hunter and Rivulet start with their Pearl as a headstart for Chieftain or other challenges", null, "", new object[]
		{
				"Stomach Pearl"
		}));





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
			float XLayer = 75;
			

			OpUpdown OptKarmaStart = new OpUpdown(cfgKarmaStart, new Vector2(XLayer -25, YLayer- stackY * stk), 50f)
			{
				description = cfgKarmaStart.info.description
			};
			OpLabel LabelKarmaStart = new OpLabel(XLayer + labelX, YLayer + lableY, cfgKarmaStart.info.Tags[0] as string, false)
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
			OpCheckBox OptGhostsIncreaseKarma = new OpCheckBox(cfgGhostsIncreaseKarma, new Vector2(XLayer, YLayer - stackY*stk))
			{
				description = cfgGhostsIncreaseKarma.info.description,
			};
			OpLabel LabelGhostsIncreaseKarma = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgGhostsIncreaseKarma.info.Tags[0] as string, false)
			{
				description = cfgGhostsIncreaseKarma.info.description,
			};


			XLayer = 325f;
			stk = 0;
			OpCheckBox OptStomachPearl = new OpCheckBox(cfgStomachPearl, new Vector2(XLayer, YLayer - stackY * stk))
			{
				description = cfgStomachPearl.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Red),
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
			OpCheckBox OptSpearOverseer = new OpCheckBox(cfgSpearOverseer, new Vector2(XLayer, YLayer - stackY*stk))
			{
				description = cfgSpearOverseer.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Spear),
			};
			OpLabel LabelSpearOverseer = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgSpearOverseer.info.Tags[0] as string, false)
			{
				description = cfgSpearOverseer.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Spear)
			};

			stk++;
			OpCheckBox OptRivRain = new OpCheckBox(cfgRivuletShortCycles, new Vector2(XLayer, YLayer - stackY*stk))
			{
				description = cfgRivuletShortCycles.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivRain = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgRivuletShortCycles.info.Tags[0] as string, false)
			{
				description = cfgRivuletShortCycles.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};

			stk++;
			OpUpdown OptRivRain2 = new OpUpdown(cfgRivuletBonusRain, new Vector2(XLayer-25f, YLayer - stackY*stk), 50f)
			{
				description = cfgRivuletBonusRain.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet),
			};
			OpLabel LabelRivRain2 = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgRivuletBonusRain.info.Tags[0] as string, false)
			{
				description = cfgRivuletBonusRain.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
			};

			stk++;
			OpCheckBox OptSaintPoints = new OpCheckBox(cfgSaintAscendPoints, new Vector2(XLayer, YLayer - stackY*stk))
			{
				description = cfgSaintAscendPoints.info.description,
				colorEdge = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint),
			};
			OpLabel LabelSaintPoints = new OpLabel(XLayer + labelX, YLayer + lableY - stackY*stk, cfgSaintAscendPoints.info.Tags[0] as string, false)
			{
				description = cfgSaintAscendPoints.info.description,
				color = PlayerGraphics.DefaultSlugcatColor(MoreSlugcatsEnums.SlugcatStatsName.Saint)
			};

			this.Tabs[0].AddItems(new UIelement[]
			{
				OptKarmaStart,
				LabelKarmaStart,
				OptKarmaCapStart,
				LabelKarmaCapStart,
				OptKarmaFlower,
				LabelKarmaFlower,
				OptGhostsIncreaseKarma,
				LabelGhostsIncreaseKarma,
				OptStomachPearl,
				LabelStomachPearl,
				OptArtificerRobo,
				LabelArtificerRobo,
				OptSpearOverseer,
				LabelSpearOverseer,
				OptRivRain,
				LabelRivRain,
				OptRivRain2,
				LabelRivRain2,
				OptSaintPoints,
				LabelSaintPoints
			});
		}



	}
}
