using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Model;

using GameFrame.Core.Audio.Multi;
using GameFrame.Core.Audio.Single;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        public ContinuousAudioManager AmbienceAudioManager { get; set; }
        public ContinuousAudioManager BackgroundAudioManager { get; set; }
        public EffectsAudioManager EffectsAudioManager { get; set; }

        public IList<GameMode> AvailableGameModes { get; } = new List<GameMode>();
        public GameMode SelectedGameMode { get; set; }

        public bool LockCameraMovement { get; set; } = false;

        public void PlayButtonSound()
        {
            EffectsAudioManager.Play("Button");
        }

        protected override GameState InitializeGameState()
        {
            // Maybe add a Tutorial scene, where the user can set "skip" for the next time.
            var gameState = new GameState()
            {
                CurrentScene = SceneNames.World,
                GameMode = this.SelectedGameMode
            };

            GenerateWorld(gameState);

            return gameState;
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

        private void GenerateWorld(GameState gameState)
        {
            var worldGenerator = new Core.Generating.World.Generator(this.SelectedGameMode);

            if (worldGenerator.Generate())
            {
                gameState.World = worldGenerator.World;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}