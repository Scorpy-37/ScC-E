using ScorpEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ScorpEngine
{
    public class PlayerController : Behaviour
    {
        public static List<Vector2> player = new List<Vector2> { Vector2.Zero() };
        Vector2 dir = Vector2.Zero();
        public float moveInterval = 0.25f;
        float lastMoved = 0f;

        public void Init() {}
        public void Start() {
            player[0] = new Vector2(Engine.sceneWidth / 2 - 1, Engine.sceneHeight / 2);

            Engine.SetPixelSafe(player[0], GameInit.sprites[2]);
        }
        public void Update() {
            if (GameOver.gameEnded)
                return;
            
            if (dir.x != 0 || dir.y != 0)
                lastMoved += (float)Engine.delta;
            
            if (GameInit.Input == "w")
                dir = new Vector2(0,1);
            if (GameInit.Input == "s")
                dir = new Vector2(0,-1);
            if (GameInit.Input == "d")
                dir = new Vector2(1,0);
            if (GameInit.Input == "a")
                dir = new Vector2(-1,0);

            if (lastMoved > moveInterval)
            {
                Vector2 vp = player[0] + dir;
                if ((int)vp.x < 0 || (int)vp.x >= Engine.sceneWidth || (int)vp.y < 0 || (int)vp.y >= Engine.sceneHeight)
                {
                    GameOver.gameEnded = true;
                    return;
                }
                if (Engine.sceneData[(int)vp.x, (int)vp.y] == GameInit.sprites[2])
                {
                    GameOver.gameEnded = true;
                    return;
                }

                for (int i = 0; i < player.Count; i++)
                    Engine.SetPixelSafe(player[i], GameInit.sprites[0]);

                lastMoved -= moveInterval;
                
                for (int i = player.Count - 2; i >= 0; i--)
                    player[i+1] = player[i];
                player[0] += dir;

                if (Engine.sceneData[(int)player[0].x, (int)player[0].y] == GameInit.sprites[1])
                {
                    FoodSpawn.foodAmount--;
                    player.Add(player[player.Count-1]);
                }

                for (int i = 0; i < player.Count; i++)
                    Engine.SetPixelSafe(player[i], GameInit.sprites[2]);
            }
        }
        public void LateUpdate() {}
    }
}