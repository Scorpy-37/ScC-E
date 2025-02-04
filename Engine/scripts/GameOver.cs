using ScorpEngine;
using System;

namespace ScorpEngine
{
    public class GameOver : Behaviour
    {
        public static bool gameEnded = false;

        public void Init() {}
        public void Start() {}
        public void Update() {
            if (gameEnded)
            {
                Engine.SetTheme(ConsoleColor.Red, ConsoleColor.Black);
                Engine.DrawText("GAME OVER!", 1, 9, 2, 2);
                Engine.DrawText("Score: "+PlayerController.player.Count.ToString(), 1, 15, 1, 1);
            }
        }
        public void LateUpdate() {}
    }
}