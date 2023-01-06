using System;

using Assets.Scripts.Constants;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        protected override GameState InitializeGameState()
        {
            // Maybe add a Tutorial scene, where the user can set "skip" for the next time.
            var gameState = new GameState()
            {
                CurrentScene = SceneNames.World,
            };

            GenerateWorld(gameState);

            return gameState;
        }

        private void GenerateWorld(GameState gameState)
        {
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions()
            {
                AreAnimationsEnabled = true,
                EffectsVolume = 0.7f,
                BackgroundVolume = 0.9f,
                AmbienceVolume = 0.125f
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}