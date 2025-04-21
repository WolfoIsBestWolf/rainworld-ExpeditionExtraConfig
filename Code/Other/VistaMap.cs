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
                this.symbolSprite.isVisible = false; 
                map.inFrontContainer.AddChild(this.symbolSprite);
                inRoomPos = pos;
                challenge = vista;


                if (WConfig.cfgVistaCircle.Value)
                {
                    this.spCircle = new FSprite("vistaRing", true);
                    this.spCircle.scale = 0f;
                    this.spCircle.isVisible = false;
                    //this.spCircle.shader = Custom.rainWorld.Shaders["VectorCircleFadable"];
                    map.inFrontContainer.AddChild(this.spCircle);

                    //spCircle.element = Futile.atlasManager.GetElementWithName("Futile_White");
                    //spCircle.shader = map.hud.rainWorld.Shaders["VectorCircleFadable"];
                }
                //vistaCircle2 = new HUDCircle(map.hud, HUDCircle.SnapToGraphic.None, map.container, 1);
            }

            public Challenge challenge;
            public bool visible = false;
            public float timeSinceWrongLayer = 0;
            public float phase;
            public Vector2 position;
            public FSprite spCircle;
            //public HUDCircle vistaCircle2;
            public float alphaTimer = 0;
            public float resetTimer = 0;

            //public FadeCircle fadeCircle;

            public override void Update()
            {
                base.Update();
 
            }

            public override void Draw(float timeStacker)
            {
                base.Draw(timeStacker);
                this.visible = (this.fade > 0f && this.lastFade > 0f);
                this.bkgFade.isVisible = this.visible;
                this.symbolSprite.isVisible = this.visible;
                if (spCircle != null)
                {
                    this.spCircle.isVisible = this.visible;
                }
                /*if (vistaCircle2 != null)
                {
                    vistaCircle2.sprite.isVisible = this.visible;
                }*/
                if (!this.visible || challenge.hidden && !challenge.revealed  || challenge.completed)
                {
                    return;
                }

                float num = Mathf.Lerp(this.map.lastFade, this.map.fade, timeStacker) * Mathf.Lerp(this.lastFade, this.fade, timeStacker);
                float alpha = (0.4f + 0.6f * timeSinceWrongLayer);
                this.symbolSprite.alpha = num * alpha;
                this.bkgFade.alpha = num * 0.5f * alpha;


                Vector2 pos = this.map.RoomToMapPos(this.inRoomPos, this.room, 1);
                this.bkgFade.x = pos.x;
                this.bkgFade.y = pos.y;
                this.symbolSprite.x = pos.x;
                this.symbolSprite.y = pos.y;
                this.symbolSprite.color = new HSLColor(this.phase, 0.85f, 0.65f).rgb;
                this.bkgFade.color = symbolSprite.color;
                if (spCircle != null)
                {
                    this.spCircle.color = symbolSprite.color;
                    this.spCircle.x = pos.x;
                    this.spCircle.y = pos.y;
                }
                /*if (vistaCircle2 != null)
                {
                    this.vistaCircle2.sprite.color = symbolSprite.color;
                    this.vistaCircle2.sprite.x = pos.x;
                    this.vistaCircle2.sprite.y = pos.y;
                    this.vistaCircle2.pos = pos;
                }*/

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
                //float alpha = this.map.Alpha(map.mapData.LayerOfRoom(room), timeStacker, true);
                bkgFade.scale = 35f - 15f * timeSinceWrongLayer;

                if (spCircle != null)
                {
                    if (alphaTimer < 0f)
                    {
                        spCircle.scale = 0;
                        alphaTimer = 250f;
                        resetTimer = 100f;
                        //Debug.Log("Reset");
                    }
                    if (resetTimer < 0)
                    {
                        if (resetTimer < 0 && spCircle.scale < 15)
                        {
                            spCircle.scale += 0.02f * timeStacker;
                        }
                        else
                        {
                            //spCircle.scale += 0.02f * timeStacker;
                            alphaTimer -= timeStacker;
                        }
                    }
                    else
                    {
                        resetTimer -= timeStacker;
                    }
                    this.spCircle.alpha = num * alpha *0.5f * (alphaTimer / 250f);
                    /*Debug.Log("s" + circle.scale);
                    Debug.Log("a" + circle.alpha);
                    Debug.Log("t" + timer);*/
                }
                 
            }

            public override void Destroy()
            {
                base.Destroy();
               // vistaCircle2.ClearSprite();
            }
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