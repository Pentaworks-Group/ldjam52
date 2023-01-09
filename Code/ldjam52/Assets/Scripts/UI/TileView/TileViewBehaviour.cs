using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model.Buildings;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.TileView
{
    public class TileViewBehaviour : MonoBehaviour
    {
        public WorldBehaviour worldBehaviour;
        private GameObject visiblityContainer;

        private GameObject buyTileContent;
        private TMP_Text biomeText;
        private TMP_Text buyCouldBuyText;
        private TMP_Text buyCostFundsAvailableText;
        private TMP_Text buyCostTotalCostAmount;
        private GameObject buyCostBuyAndBuildContainer;
        private TMP_Text buyCostBuyAndBuildAmount;
        private Button buyTileButton;
        private Button buyTileAndBuildFarmButton;

        private GameObject buildFarmContent;
        private TMP_Text buildFarmStatusText;
        private GameObject buildCostBuyAndBuildContainer;
        private TMP_Text buildCostBuyAndBuildAmount;
        private Button buildFarmButton;
        private Button buildBuyAndBuildFarmButton;

        private TileBehaviour tileBehaviour;
        private Action actionRequired;
        private List<TileBehaviour> surroundingTiles;
        private Int32 tilesTotalCost;

        public void Show(TileBehaviour tileBehaviour, Action actionRequired)
        {
            if (tileBehaviour != null && !worldBehaviour.WasEscPressed())
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
                Assets.Scripts.Base.Core.Game.PlayButtonSound();
            }
        }

        public void Hide()
        {
            this.tileBehaviour = null;
            this.actionRequired = null;

            SetVisibility(false);
            Assets.Scripts.Base.Core.Game.PlayButtonSound();
            worldBehaviour.PressEsc();
        }

        public void BuyTile(Boolean isBuyAndBuilt)
        {
            Base.Core.Game.EffectsAudioManager.Play("Buy");

            this.tileBehaviour.Tile.IsOwned = true;
            FarmStorageController.TakeMoneyOfStorage(this.tileBehaviour.Tile.Price);

            if (isBuyAndBuilt)
            {
                FarmStorageController.TakeMoneyOfStorage(tilesTotalCost);

                //FarmStorageController.TakeMoneyOfStorage(tilesTotalCost); // Farm price tbd

                var farm = new Farm()
                {
                    Position = this.tileBehaviour.Tile.Position
                };

                Base.Core.Game.State.World.Farm = farm;

                this.tileBehaviour.Tile.Building = farm;

                foreach (var surroundingBehvaiour in surroundingTiles)
                {
                    surroundingBehvaiour.Tile.IsOwned = true;
                    surroundingBehvaiour.Tile.Building = farm;
                }
            }
            else
            {
            }

            Hide();

            this.actionRequired?.Invoke();
        }

        public void BuildFarm()
        {
            Base.Core.Game.EffectsAudioManager.Play("Buy");

            if (tilesTotalCost > 0)
            {
                FarmStorageController.TakeMoneyOfStorage(tilesTotalCost); // Farm price tbd....
            }

            //FarmStorageController.TakeMoneyOfStorage(this.tileBehaviour.Tile.Price); // Farm price tbd....

            var farm = new Farm()
            {
                Position = this.tileBehaviour.Tile.Position
            };

            Base.Core.Game.State.World.Farm = farm;

            this.tileBehaviour.Tile.Building = farm;

            foreach (var surroundingBehvaiour in surroundingTiles)
            {
                surroundingBehvaiour.Tile.IsOwned = true;
                surroundingBehvaiour.Tile.Building = farm;
            }

            Hide();
        }

        private void DisplayBuildFarmContent()
        {
            buyTileContent.SetActive(false);

            biomeText.text = "";

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
                        }
                        else if (surroundingTile.Tile.Building != default)
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

            this.buildFarmStatusText.text = statusString;
            this.buildFarmButton.interactable = isAbleToBuild;
            this.buildBuyAndBuildFarmButton.interactable = isAbleToBuyAndBuild;

            if (errorLevel == 1)
            {
                this.buildCostBuyAndBuildContainer.SetActive(true);
                this.buildCostBuyAndBuildAmount.text = tilesTotalCost.ToString("n0");
            }
            else
            {
                this.buildCostBuyAndBuildContainer.SetActive(false);
            }

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

            biomeText.text = "The Biome of the Field is unknown";
            if (tileBehaviour.Tile.Field.Biome != null)
                biomeText.text = "This field is in the "+ tileBehaviour.Tile.Field.Biome.Name;


            buyCostFundsAvailableText.text = Base.Core.Game.State.FarmStorage.MoneyBalance.ToString("n0");
            buyCostTotalCostAmount.text = this.tileBehaviour.Tile.Price.ToString("n0");

            var checkBuyAndBuild = Base.Core.Game.State.World.Farm == default;

            this.buyTileAndBuildFarmButton.gameObject.SetActive(checkBuyAndBuild);
            this.buyCostBuyAndBuildContainer.SetActive(checkBuyAndBuild);

            if (checkBuyAndBuild)
            {
                var isBuyAndBuildPossible = true;

                surroundingTiles = Base.Core.Game.TileController.GetSurroundingTiles(this.tileBehaviour.Tile);

                if (surroundingTiles.Count > 0)
                {
                    if (surroundingTiles.Count != 8)
                    {
                        isBuyAndBuildPossible = false;
                    }

                    foreach (var surroundingTile in surroundingTiles)
                    {
                        if (!surroundingTile.Tile.IsOwned)
                        {
                            tilesTotalCost += surroundingTile.Tile.Price;
                        }
                        else if (surroundingTile.Tile.Building != default)
                        {
                            isBuyAndBuildPossible = false;
                            break;
                        }
                    }
                }

                if (isBuyAndBuildPossible && Base.Core.Game.State.FarmStorage.MoneyBalance < tilesTotalCost)
                {
                    isBuyAndBuildPossible = false;
                }

                if (isBuyAndBuildPossible)
                {
                    this.buyTileButton.interactable = true;
                    this.buyTileAndBuildFarmButton.interactable = true;

                    this.buyCostBuyAndBuildAmount.text = tilesTotalCost.ToString("n0");
                    this.buyCostBuyAndBuildContainer.SetActive(true);

                    buyCouldBuyText.text = "But it could.\nClick 'Buy' or 'Buy and Build' below, and it is yours!\nMaybe even with the Farm!";
                }
                else if (Base.Core.Game.State.FarmStorage.MoneyBalance >= this.tileBehaviour.Tile.Price)
                {
                    this.buyTileButton.interactable = true;
                    this.buyTileAndBuildFarmButton.interactable = false;

                    buyCouldBuyText.text = "But it could.\nClick 'Buy' below, and it is yours!\nDo it. Do it now!";
                }
                else
                {
                    this.buyTileButton.interactable = false;
                    this.buyTileAndBuildFarmButton.interactable = false;

                    buyCouldBuyText.text = "And it will remain this way.\nGo earn some money!\nSlacker.";
                }
            }
            else
            {
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
            this.biomeText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/BiomeText").GetComponent<TMP_Text>();
            this.buyCouldBuyText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CouldBuyText").GetComponent<TMP_Text>();
            this.buyCostFundsAvailableText = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/FundsAvailableAmount").GetComponent<TMP_Text>();
            this.buyCostTotalCostAmount = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/CostAmount").GetComponent<TMP_Text>();
            this.buyCostBuyAndBuildContainer = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/CostBuyAndBuildContainer").gameObject;
            this.buyCostBuyAndBuildAmount = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/TopTextContainer/CostContainer/CostBuyAndBuildContainer/CostBuyAndBuildAmount").GetComponent<TMP_Text>();
            this.buyTileButton = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/BuyArea/BuyTileButton").GetComponent<Button>();
            this.buyTileAndBuildFarmButton = transform.Find("TileViewToggle/ContentContainer/BuyTileContent/BuyArea/BuyTileAndBuildFarmButton").GetComponent<Button>();

            this.buildFarmContent = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent").gameObject;
            this.buildFarmStatusText = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/TopTextContainer/BuildFarmStatusText").GetComponent<TMP_Text>();
            this.buildCostBuyAndBuildContainer = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/TopTextContainer/CostBuyAndBuildContainer").gameObject;
            this.buildCostBuyAndBuildAmount = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/TopTextContainer/CostBuyAndBuildContainer/CostBuyAndBuildAmount").GetComponent<TMP_Text>();
            this.buildFarmButton = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/BuildFarmArea/BuildFarmButton").GetComponent<Button>();
            this.buildBuyAndBuildFarmButton = transform.Find("TileViewToggle/ContentContainer/BuildFarmContent/BuildFarmArea/BuyAndBuildFarmButton").GetComponent<Button>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && this.visiblityContainer.activeSelf)
            {
                GoBackOrClose();
            }
        }

        private void GoBackOrClose()
        {

            Hide();
            

        }
    }
}
