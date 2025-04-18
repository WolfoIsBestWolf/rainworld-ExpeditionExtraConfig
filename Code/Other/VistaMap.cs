using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using System.Linq;
using RWCustom;
using Menu;
using System;
using Expedition;
using MonoMod.Cil;
using System.Globalization;
using UnityEngine;
using HUD;
using System.IO;

namespace ExpeditionExtraConfig
{
    public class VistaMap
    {
        #region O
        public class VistaMarker2 : Map.MapObject
        {
            public int room;
            public Vector2 inRoomPos;

            public float fade;
            public float lastFade;
            public float fadeInSpeed;
            public FSprite bkgFade;
            public FSprite symbolSprite;

            public VistaMarker2(Map map, int room, Vector2 pos, Challenge vista) : base(map)
            {
                this.map = map;
                this.room = room;
                this.inRoomPos = pos;
                this.bkgFade = new FSprite("Futile_White", true);
                this.bkgFade.shader = map.hud.rainWorld.Shaders["FlatLight"];
                this.bkgFade.color = new Color(0f, 0f, 0f);
                map.inFrontContainer.AddChild(this.bkgFade);
                this.bkgFade.isVisible = false;

                this.symbolSprite = new FSprite("TravellerB", true);
                this.symbolSprite.scale = 2f;
                map.inFrontContainer.AddChild(this.symbolSprite);
                this.symbolSprite.isVisible = false;
                inRoomPos = pos;
                challenge = vista;
            }

            public Challenge challenge;
            public bool visible = false;
            public override void Update()
            {
                base.Update();
                this.lastFade = this.fade;
                this.fade = Mathf.Min(1f, this.fade + this.fadeInSpeed);
            }
            public void FadeIn(float fdSpd)
            {
                if (this.fadeInSpeed > 0f)
                {
                    return;
                }
                this.fadeInSpeed = 1f / fdSpd;
            }
            public override void Destroy()
            {
                base.Destroy();
                this.bkgFade.RemoveFromContainer();
                this.symbolSprite.RemoveFromContainer();
            }
            public override void Draw(float timeStacker)
            {
                base.Draw(timeStacker);
                this.visible = (this.map.fade > 0f && this.map.lastFade > 0f);
                this.bkgFade.isVisible = this.visible;
                this.symbolSprite.isVisible = this.visible;
                if (!this.visible || challenge.completed)
                {
                    return;
                }
                float num = Mathf.Lerp(this.map.lastFade, this.map.fade, timeStacker) * Mathf.Lerp(this.lastFade, this.fade, timeStacker);
                Vector2 vector = this.map.RoomToMapPos(this.inRoomPos, this.room, timeStacker);
                this.bkgFade.x = vector.x;
                this.bkgFade.y = vector.y;
                this.symbolSprite.x = vector.x;
                this.symbolSprite.y = vector.y;
                this.bkgFade.alpha = num * 0.5f;

                this.symbolSprite.alpha = num;
                //this.symbolSprite.color = Color.Lerp(new Color(1f, 0f, 0f), Menu.Menu.MenuRGB(Menu.Menu.MenuColors.White), 0.5f + 0.5f * Mathf.Sin(((float)this.map.counter + timeStacker) / 14f));
                this.phase += 0.13f * Time.deltaTime;
                if (this.phase > 1f)
                {
                    this.phase = 0f;
                }
                this.symbolSprite.color = new HSLColor(this.phase, 0.85f, 0.75f).rgb;
                this.bkgFade.color = symbolSprite.color;
                this.bkgFade.scale = 12.5f;
            }
            public float phase;
            public Vector2 position;
        }
        #endregion


        public class VistaMarker : Map.FadeInMarker
        {
 
            public VistaMarker(Map map, int room, Vector2 pos, Challenge vista) : base(map, room, new Vector2(480f, 240f), 3f)
            {

                this.fadeInRad = float.PositiveInfinity;
                this.symbolSprite = new FSprite("TravellerB", true);
                this.symbolSprite.scale = 2f;
                map.inFrontContainer.AddChild(this.symbolSprite);
                this.symbolSprite.isVisible = false;
                inRoomPos = pos;
                challenge = vista;
            }

            public Challenge challenge;
            public bool visible = false;
            public float timeSinceWrongLayer = 0;

            public override void Draw(float timeStacker)
            {
                base.Draw(timeStacker);
                this.visible = true;
                this.bkgFade.isVisible = this.visible;
                this.symbolSprite.isVisible = this.visible;
                if (!this.visible || challenge.hidden && !challenge.revealed  || challenge.completed)
                {
                    return;
                }

                float num = Mathf.Lerp(this.map.lastFade, this.map.fade, timeStacker) * Mathf.Lerp(this.lastFade, this.fade, timeStacker);
                Vector2 vector = this.map.RoomToMapPos(this.inRoomPos, this.room, timeStacker);
                this.bkgFade.x = vector.x;
                this.bkgFade.y = vector.y;
                this.symbolSprite.x = vector.x;
                this.symbolSprite.y = vector.y;
                this.bkgFade.alpha = num * 0.5f;

                this.symbolSprite.alpha = num;
                //this.symbolSprite.color = Color.Lerp(new Color(1f, 0f, 0f), Menu.Menu.MenuRGB(Menu.Menu.MenuColors.White), 0.5f + 0.5f * Mathf.Sin(((float)this.map.counter + timeStacker) / 14f));
                this.phase += 0.13f * Time.deltaTime;
                if (this.phase > 1f)
                {
                    this.phase = 0f;
                }
                if (timeSinceWrongLayer < 1 && map.mapData.LayerOfRoom(room) == this.map.layer)
                {
                    timeSinceWrongLayer += 3.5f * Time.deltaTime;
                }
                else if (timeSinceWrongLayer > 0)
                {
                    timeSinceWrongLayer -= 3.5f * Time.deltaTime;
                }

                float bright = 0.3f + 0.35f * timeSinceWrongLayer; //0.75f
                float sat = 0.6f + 0.25f * timeSinceWrongLayer;
                //float bright = map.mapData.LayerOfRoom(room) == this.map.layer ? 0.75f : 0.3f;
                //float sat = map.mapData.LayerOfRoom(room) == this.map.layer ? 0.85f : 0.6f;
                this.symbolSprite.color = new HSLColor(this.phase, sat, bright).rgb;
                this.bkgFade.color = symbolSprite.color;

                this.bkgFade.scale = 25f-10f*timeSinceWrongLayer;
            }
            public float phase;
            public Vector2 position;
        }


        public static void AddVistaPointsToMap(On.HUD.Map.orig_ctor orig, HUD.Map self, HUD.HUD hud, HUD.Map.MapData mapData)
        {
            orig(self, hud, mapData);
            if (Custom.rainWorld.ExpeditionMode && WConfig.cfgShowVistaOnMap.Value)
            {
                foreach (Challenge challenge in ExpeditionData.challengeList)
                {
                    if (challenge is VistaChallenge)
                    {
                        var vista = (challenge as VistaChallenge);
                        if (vista.region == self.RegionName && !vista.completed)
                        {
                            int index = -1;
                            if (RainWorld.roomNameToIndex.TryGetValue(vista.room, out index))
                            {
                                self.mapObjects.Add(new VistaMarker(self, index, vista.location, vista));
                            }
                            else
                            {
                                Debug.Log("Warning Vista Room is not present in region : " + vista.room);
                            }
                        }
                    }

                }
            }
         
        }


       
    }
}