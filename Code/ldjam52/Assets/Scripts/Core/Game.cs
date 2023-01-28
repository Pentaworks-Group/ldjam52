using System;
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Model;

using GameFrame.Core.Audio.Continuous;
using GameFrame.Core.Audio.Multi;
using GameFrame.Core.SavedGames;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions, SavedGamedPreviewImpl>
    {
        public ContinuousAudioManager AmbienceAudioManager { get; set; }
        public ContinuousAudioManager BackgroundAudioManager { get; set; }
        public EffectsAudioManager EffectsAudioManager { get; set; }

        public IList<GameMode> AvailableGameModes { get; } = new List<GameMode>();
        public GameMode SelectedGameMode { get; set; }

        public List<AudioClip> AudioClipListMenu { get; set; }
        public List<AudioClip> AudioClipListGame { get; set; }
        public List<AudioClip> AmbientClipList { get; set; }
        public List<String> AmbientEffectsClipList { get; set; }

        public TileController TileController { get; set; }

        public bool LockCameraMovement { get; set; } = false;

        public void PlayButtonSound()
        {
            EffectsAudioManager.Play("ButtonSound");
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
            PopulateShopStorage(gameState);
            SetPlayerValues(gameState);
            GenerateAnalyzers(gameState);

            return gameState;
        }

        public void PopulateShopStorage(GameState gameState)
        {
            if (gameState.AvailableShopItems == default)
            {
                gameState.AvailableShopItems = new(); 
                foreach (StorageItem item in gameState.GameMode.AvailableShopItems)
                {
                    gameState.AvailableShopItems.Add(item);
                }
            }
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
                AmbienceVolume = 0.3f,
                BackgroundVolume = 0.25f,
                IsMouseScreenEdgeScrollingEnabled = true,
                MoveSensivity = 0.5f,
                ZoomSensivity = 0.5f
            };
        }

        private void GenerateWorld(GameState gameState)
        {
            var worldGenerator = new Core.Generating.World.Generator(this.SelectedGameMode);

            if (worldGenerator.Generate())
            {
                gameState.World = worldGenerator.World;
            }
            else
            {
                throw new Exception("------------ FAILED TO GENERATE WORLD! ------------");
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

        public void GenerateAnalyzers(GameState gameState)
        {
            var newPlantAnalizer = new Analyzer
            {
                Name = "Plant Analyzer",
                Description = "Can analyse the plants genome and give you some informations about them.",
                ImgName = "Microscope",
                DevelopmentStages = Constants.DevelopmentStages.PlantAnalyticsStages,
            };
            newPlantAnalizer.CurrentDevelopmentStage = newPlantAnalizer.DevelopmentStages[0];
            gameState.PlantAnalyzer = newPlantAnalizer;

            var newFieldAnalizer = new Analyzer
            {
                Name = "Field Analyzer",
                Description = "Can analyse fields and give you some informations about them.",
                ImgName = "Scanner",
                DevelopmentStages = Constants.DevelopmentStages.FieldAnalyticsStages,
            };
            newFieldAnalizer.CurrentDevelopmentStage = newFieldAnalizer.DevelopmentStages[0];
            gameState.FieldAnalyzer = newFieldAnalizer;

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();


            Test();
        }

        private static void Test()
        {
            var filePath = Application.persistentDataPath + "/test.json";

            if (System.IO.File.Exists(filePath))
            {
                UnityEngine.Debug.Log("TEST exists");
                System.IO.File.Delete(filePath);
            }

            var timestamp = DateTime.Now;

            var sampleText = Newtonsoft.Json.JsonConvert.SerializeObject(new TestObject(timestamp));

            System.IO.File.WriteAllText(filePath, sampleText);

            UnityEngine.Debug.Log("Wrote file");

            if (System.IO.File.Exists(filePath))
            {
                UnityEngine.Debug.Log("File exists");

                var outText = System.IO.File.ReadAllText(filePath);
                                
                if (!String.IsNullOrEmpty(outText))
                {
                    UnityEngine.Debug.Log(String.Format("loaded json: {0}", outText));

                    var restoredTestObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TestObject>(outText);

                    if (restoredTestObject != default)
                    {
                        UnityEngine.Debug.Log(String.Format("Object restored. Timestamp matches: {0}", timestamp == restoredTestObject.Timestamp));
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Failed to deserialize...");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("No text loaded...");
                }
            }
            else
            {
                UnityEngine.Debug.Log("File does not exist...");
            }
        }
    }
}