
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Core;


using UnityEngine;

namespace Assets.Scripts.Scene.FieldTestScene
{
    public class TestFieldBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (Assets.Scripts.Base.Core.Game.State == default) {
                InitializeGameState();
            }
        }

        protected void InitializeGameState()
        {
            LoadGameSettings();
            // Maybe add a Tutorial scene, where the user can set "skip" for the next time.
            var gameState = new GameState()
            {
                CurrentScene = SceneNames.TestField,
                GameMode = Base.Core.Game.AvailableGameModes[0]
            };

            gameState.FarmStorage = gameState.GameMode.Player.StartingFarmStorage;
            Assets.Scripts.Base.Core.Game.PopulateKnownPlants(gameState);

            Assets.Scripts.Base.Core.Game.Start(gameState);
        }

        public void LoadGameSettings()
        {
            if (Base.Core.Game.AvailableGameModes.Count == 0)
            {
                var filePath = Application.streamingAssetsPath + "/GameModes.json";
                StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<List<GameMode>>(filePath, SetGameSettings));
            }
        }

        private List<GameMode> SetGameSettings(List<GameMode> loadedGameModes)
        {
            if (loadedGameModes?.Count > 0)
            {
                foreach (var gameMode in loadedGameModes)
                {
                    Base.Core.Game.AvailableGameModes.Add(gameMode);
                }
            }

            if (Base.Core.Game.SelectedGameMode == default)
            {
                Base.Core.Game.SelectedGameMode = loadedGameModes[0];
            }

            return loadedGameModes;
        }

    }
}
