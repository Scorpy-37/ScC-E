using ScorpEngine;
using System;
using System.Collections.Generic;

namespace ScorpEngine
{
    public class FoodSpawn : Behaviour
    {
        public int maxFood = 7;
        public static int foodAmount = 0;

        public void Init() {}
        public void Start() {}
        public void Update() {
            if (GameOver.gameEnded)
                return;
                
            if (foodAmount < maxFood)
            {
                Vector2 spot = new Vector2(
                    Engine.RandomNum(0, Engine.sceneWidth),
                    Engine.RandomNum(0, Engine.sceneHeight)
                );
                if (Engine.sceneData[(int)spot.x, (int)spot.y] == GameInit.sprites[0])
                {
                    Engine.sceneData[(int)spot.x, (int)spot.y] = GameInit.sprites[1];
                    foodAmount++;
                }
            }
        }
        public void LateUpdate() {}
    }
}