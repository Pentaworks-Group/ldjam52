using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Base;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;

using Assets.Scripts.Constants;

namespace Assets.Scripts.Scene.Shops
{
    public class EquipmentShopBehaviour : MonoBehaviour
    {
        public TMP_Text PlantAnalyzerName;
        public TMP_Text PlantAnalyzerDesc;
        public TMP_Text FieldAnalyzerName;
        public TMP_Text FieldAnalyzerDesc;

        public Image PlantAnalyzerImage;
        public Image FieldAnalyzerImg;

        // Start is called before the first frame update
        void Start()
        {
            if (Assets.Scripts.Base.Core.Game.State == default)
            {
                InitializeGameState();
            }

            PlantAnalyzerName.text = Base.Core.Game.State.PlantAnalyzer.Name;
            PlantAnalyzerDesc.text = Base.Core.Game.State.PlantAnalyzer.Description;
            FieldAnalyzerName.text = Base.Core.Game.State.FieldAnalyzer.Name;
            FieldAnalyzerDesc.text = Base.Core.Game.State.FieldAnalyzer.Description;

            PlantAnalyzerImage.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(Base.Core.Game.State.PlantAnalyzer.ImgName);
            FieldAnalyzerImg.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(Base.Core.Game.State.FieldAnalyzer.ImgName);
            //            PlantAnalyzerImg.sprite = Base.Core.Game.State.PlantAnalyzer.ImgName;

        }

        // Update is called once per frame
        void Update()
        {

        }

        // TEST
        protected void InitializeGameState()
        {
            LoadGameSettings();
            // Maybe add a Tutorial scene, where the user can set "skip" for the next time.
            var gameState = new GameState()
            {
                CurrentScene = "EquipmentShopScene",
                GameMode = Base.Core.Game.AvailableGameModes[0]
            };

            gameState.FarmStorage = gameState.GameMode.Player.StartingFarmStorage;
            Assets.Scripts.Base.Core.Game.PopulateKnownPlants(gameState);
            Assets.Scripts.Base.Core.Game.GenerateAnalyzers(gameState);

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