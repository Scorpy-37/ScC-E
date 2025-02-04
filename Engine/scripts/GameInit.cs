using ScorpEngine;
using System;

namespace ScorpEngine
{
    public class GameInit : Behaviour
    {
        public static char[] sprites = {'-', 'x', '#'};

        public void Init() {
            Engine.SetTheme(ConsoleColor.White, ConsoleColor.Black);
            Engine.hz = 30;

            Engine.sceneWidth = 21;
            Engine.sceneHeight = 21;

            Engine.viewportWidth = 21;
            Engine.viewportHeight = 21;

            Engine.cameraX = 0;
            Engine.cameraY = 0;
        }
        public void Start() {
            for(int x = 0; x < Engine.sceneWidth; x++)
            {
                for(int y = 0; y < Engine.sceneHeight; y++)
                    Engine.sceneData[x,y] = sprites[0];
            }
        }

        public static string Input = "";
        public void Update() {
            Input = Engine._input.ToString().ToLower();
        }
        public void LateUpdate() {}
    }
}