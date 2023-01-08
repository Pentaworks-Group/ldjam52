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
        public GameObject PlantAnalyzerPanel;
        public GameObject FieldAnalyzerPanel;

        public TMP_Text BalanceText;

        // Start is called before the first frame update
        void Start()
        {
            if (Assets.Scripts.Base.Core.Game.State == default)
            {
                InitializeGameState();
            }

            BalanceText.text = FarmStorageController.GetStorageBalance().ToString();

            InitAnalyzerPanel(Base.Core.Game.State.PlantAnalyzer, PlantAnalyzerPanel);
            InitAnalyzerPanel(Base.Core.Game.State.FieldAnalyzer, FieldAnalyzerPanel);
        }

        private void InitAnalyzerPanel(Analyzer analyzer, GameObject panel)
        {
            panel.transform.Find("AnalyzerName").GetComponent<TMP_Text>().text = analyzer.Name;
            panel.transform.Find("AnalyzerDesc").GetComponent<TMP_Text>().text = analyzer.Description;

            panel.transform.Find("AnalyzerImage").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(analyzer.ImgName);

            DevelopmentStage nextStage = getNextDevelopmentStage(analyzer);
            if (nextStage != null)
            {
                panel.transform.Find("NextStageDesc").GetComponent<TMP_Text>().text = "Next Stage: " + nextStage.Name;
                Image footer = panel.transform.Find("Footer").GetComponent<Image>();
                footer.transform.Find("CostText").GetComponent<TMP_Text>().text = nextStage.UpgradeCost.ToString();

                if (FarmStorageController.GetStorageBalance() < nextStage.UpgradeCost)
                {
                    Debug.Log("Not Enough Money to Upgrade");
                    footer.transform.Find("ButtonUpgrade").GetComponent<Button>().enabled = false;
                }
            }
            else
            {
                panel.transform.Find("ButtonUpgrade").gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpgradePlantAnalyzer()
        {
            DevelopmentStage nextStage = getNextDevelopmentStage(Base.Core.Game.State.PlantAnalyzer);
            
            if (nextStage != null && FarmStorageController.GetStorageBalance() >= nextStage.UpgradeCost)
            {
                //Check Money Amount
                Base.Core.Game.State.PlantAnalyzer.CurrentDevelopmentStage = nextStage;
            }
        }

        public void UpgradeFieldAnalyzer()
        {
            DevelopmentStage nextStage = getNextDevelopmentStage(Base.Core.Game.State.FieldAnalyzer);

            if (nextStage != null && FarmStorageController.GetStorageBalance() >= nextStage.UpgradeCost)
            {
                //Check Money Amount
                Base.Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage = nextStage;
            }
        }

        protected DevelopmentStage getNextDevelopmentStage(Analyzer analyzer)
        {
            int index = analyzer.DevelopmentStages.IndexOf(analyzer.CurrentDevelopmentStage);
            if (index < analyzer.DevelopmentStages.Count-1)
                return analyzer.DevelopmentStages[index+1];
            return null;
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