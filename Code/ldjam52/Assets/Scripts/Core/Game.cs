using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Model;

using GameFrame.Core.Audio.Continuous;
using GameFrame.Core.Audio.Multi;

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
            PopulateKnownPlants(gameState);
            SetPlayerValues(gameState);

            return gameState;
        }

        public void PopulateKnownPlants(GameState gameState)
        {
            if (gameState.KnownPlants == default)
            {
                gameState.KnownPlants = new Dictionary<System.Guid, Plant>();
            }

            foreach (Plant plant in gameState.GameMode.AvailablePlants)
            {
                if (!gameState.KnownPlants.ContainsKey(plant.ID))
                {
                    gameState.KnownPlants.Add(plant.ID, plant);
                }
            }
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

        private void SetPlayerValues(GameState gameState)
        {
            var newFarmStorage = new FarmStorage()
            {
                StorageSize = this.SelectedGameMode.Player.StartingFarmStorage.StorageSize,
                MoneyBalance = this.SelectedGameMode.Player.StartingFarmStorage.MoneyBalance,
                StorageItems = new List<StorageItem>()
            };

            foreach (var startStorageItem in this.SelectedGameMode.Player.StartingFarmStorage.StorageItems)
            {
                newFarmStorage.StorageItems.Add(new StorageItem()
                {
                    PlantId = startStorageItem.PlantId,
                    StorageAmountPlants = startStorageItem.StorageAmountPlants,
                    StorageAmountSeeds = startStorageItem.StorageAmountSeeds,
                });
            }

            gameState.FarmStorage = newFarmStorage;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}