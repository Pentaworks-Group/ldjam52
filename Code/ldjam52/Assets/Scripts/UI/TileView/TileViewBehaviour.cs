using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.TileView
{
    public class TileViewBehaviour : MonoBehaviour
    {
        private GameObject visiblityContainer;

        private GameObject buyTileContent;
        private TMP_Text buyCouldBuyText;
        private TMP_Text buyCostFundsAvailableText;
        private TMP_Text buyCostFundsCostText;
        private Button buyTileButton;

        private GameObject buildFarmContent;
        private TMP_Text buildFarmStatusText;
        private Button buildFarmButton;
        private Button buildBuyAndBuildFarmButton;

        private TileBehaviour tileBehaviour;
        private Action actionRequired;
        private List<TileBehaviour> surroundingTiles;
        private Int32 tilesTotalCost;

        public void Show(TileBehaviour tileBehaviour, Action actionRequired)
        {
            if (tileBehaviour != null)
            {
                this.tileBehaviour = tileBehaviour;
                this.actionRequired = actionRequired;
                this.surroundingTiles = default;
                this.tilesTotalCost = 0;

                if (this.tileBehaviour.Tile.IsOwned)
                {
                    DisplayBuildFarmContent();
                }
                else
                {
                    DisplayBuyTileContent();
                }

                SetVisibility(true);
            }
        }

        public void Hide()
        {
            this.tileBehaviour = null;
            this.actionRequired = null;

            SetVisibility(false);
        }

        public void BuyTile()
        {
            Base.Core.Game.EffectsAudioManager.Play("Buy");

            FarmStorageController.TakeMoneyOfStorage(this.tileBehaviour.Tile.Price);
            this.tileBehaviour.Tile.IsOwned = true;

            Hide();

            this.actionRequired?.Invoke();
        }

        public void BuildFarm()
        {
            Base.Core.Game.EffectsAudioManager.Play("Buy");

            //FarmStorageController.TakeMoneyOfStorage(this.tileBehaviour.Tile.Price);

            var farm = new Farm()
            {
                Position = this.tileBehaviour.Tile.Position
            };

            Base.Core.Game.State.World.Farm = farm;

            this.tileBehaviour.Tile.Farm = farm;

            foreach (var surroundingBehvaiour in surroundingTiles)
            {
                surroundingBehvaiour.Tile.Farm = farm;
            }

            Hide();
        }

        private void DisplayBuildFarmContent()
        {
            buyTileContent.SetActive(false);

            var statusString = "You can build the Farm.";
            var errorLevel = -1;

            var isAbleToBuild = true;
            var isAbleToBuyAndBuild = true;

            if (!this.tileBehaviour.Tile.IsOwned)
            {
                statusString = "You dont own the select land...";
                errorLevel = 1;

                isAbleToBuild = false;
                tilesTotalCost += this.tileBehaviour.Tile.Price;
            }

            if (isAbleToBuild)
            {
                surroundingTiles = Base.Core.Game.TileController.GetSurroundingTiles(this.tileBehaviour.Tile);

                if (surroundingTiles.Count > 0)
                {
                    foreach (var surroundingTile in surroundingTiles)
                    {
                        if (!surroundingTile.Tile.IsOwned)
                        {
                            errorLevel = 1;
                            statusString = "You dont own all the surrounding tiles!";
                            tilesTotalCost += surroundingTile.Tile.Price;

                            isAbleToBuild = false;
                            break;
                        }
                        else if (surroundingTile.Tile.Farm != default)
                        {
                            statusString = "Surrounding tile is occupied by a building!";
                            errorLevel = 2;

                            isAbleToBuild = false;
                            isAbleToBuyAndBuild = false;
                            break;
                        }
                        else if (surroundingTile.Tile.Field.Seed != default)
                        {
                            statusString = "One or more of the surrounding fields are in use!";
                            errorLevel = 3;

                            isAbleToBuild = false;
                            isAbleToBuyAndBuild = false;
                            break;
                        }
                    }
                }
            }

            buildFarmStatusText.text = statusString;
            this.buildFarmButton.interactable = isAbleToBuild;
            this.buildBuyAndBuildFarmButton.interactable = isAbleToBuyAndBuild;

            var textColor = Color.green;

            switch (errorLevel)
            {
                case 1: textColor = Color.yellow; break;
                case 2: textColor = new Color(1, 0.647f, 0); break;
                case 3: textColor = Color.red; break;
            }

            buildFarmStatusText.color = textColor;

            buildFarmContent.SetActive(true);
        }

        private void DisplayBuyTileContent()
        {
            buildFarmContent.SetActive(false);

            buyCostFundsAvailableText.text = Base.Core.Game.State.FarmStorage.MoneyBalance.ToString("n0");
            buyCostFundsCostText.text = this.tileBehaviour.Tile.Price.ToString("n0");

            if (Base.Core.Game.State.FarmStorage.MoneyBalance >= this.tileBehaviour.Tile.Price)
            {
                this.buyTileButton.interactable = true;
                buyCouldBuyText.text = "But it could.\nClick 'Buy' below, and it is yours!\nDo it. Do it now!";
            }
            else
            {
                this.buyTileButton.interactable = false;
                buyCouldBuyText.text = "And it will remain this way.\nGo earn some money!\nSlacker.";
            }

            buyTileContent.SetActive(true);
        }

        private void SetVisibility(Boolean isVisible)
        {
            Base.Core.Game.LockCameraMovement = isVisible;

            this.visiblityContainer.SetActive(isVisible);
        }

        private void Awake()
        {
            this.visiblityContainer = transform.Find("TileViewToggle").gameObject;

            this.buyTileContent = transform.Find("TileViewToggle/ContentContainer/BuyTileContent").gameObject;
            this.buyCouldBuyText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CouldBuyText").GetComponent<TMP_Text>();
            this.buyCostFundsAvailableText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/FundsAvailableAmount").GetComponent<TMP_Text>();
            this.buyCostFundsCostText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/CostAmount").GetComponent<TMP_Text>();
            this.buyTileButton = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/BuyArea/BuyTileButton").GetComponent<Button>();

            this.buildFarmContent = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent").gameObject;
            this.buildFarmStatusText = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/TopTextContainer/BuildFarmStatusText").GetComponent<TMP_Text>();
            this.buildFarmButton = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/BuildFarmArea/BuildFarmButton").GetComponent<Button>();
            this.buildBuyAndBuildFarmButton = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/BuildFarmArea/BuyAndBuildFarmButton").GetComponent<Button>();
        }

        private void Update()
        {

        }
    }
}
