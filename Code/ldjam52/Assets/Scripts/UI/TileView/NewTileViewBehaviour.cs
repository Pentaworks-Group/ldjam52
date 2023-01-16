using System;
using System.Text;

using Assets.Scripts.Prefabs.World;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.TileView
{
    public class NewTileViewBehaviour : ViewBaseBehaviour
    {
        public UnityEvent<TileBehaviour> FieldViewRequested = new UnityEvent<TileBehaviour>();

        private readonly Color orangeColor = new Color(1, 0.647f, 0);

        private GameObject visiblityContainer;
        private TMP_Text headerText;
        private TMP_Text statusText;

        private GameObject buyButton;
        private Text buyButtonText;
        private GameObject buyAndBuildButton;
        private TileBehaviour tileBehaviour;

        public virtual void Show(TileBehaviour tileBehaviour)
        {
            if (tileBehaviour != null)
            {
                Show();

                this.tileBehaviour = tileBehaviour;

                LoadContent();

                SetVisibility(true);
            }
        }

        public override void Hide()
        {
            base.Hide();

            this.tileBehaviour = null;

            SetVisibility(false);
        }

        public void OnBuyClicked(Boolean isBuyAndBuilt)
        {
            //Base.Core.Game.EffectsAudioManager.Play("Buy");

            //this.tileBehaviour.Tile.IsOwned = true;
            //FarmStorageController.TakeMoneyOfStorage(this.tileBehaviour.Tile.Price);

            //if (isBuyAndBuilt)
            //{
            //    FarmStorageController.TakeMoneyOfStorage(tilesTotalCost - this.tileBehaviour.Tile.Price);

            //    //FarmStorageController.TakeMoneyOfStorage(tilesTotalCost); // Farm price tbd

            //    var farm = new Farm()
            //    {
            //        Position = this.tileBehaviour.Tile.Position
            //    };

            //    Base.Core.Game.State.World.Farm = farm;

            //    this.tileBehaviour.Tile.Building = farm;

            //    foreach (var surroundingBehvaiour in surroundingTiles)
            //    {
            //        surroundingBehvaiour.Tile.IsOwned = true;
            //        surroundingBehvaiour.Tile.Building = farm;
            //    }
            //    Hide();
            //}
            //else
            //{
            //    FieldViewRequested.Invoke(this.tileBehaviour);
            //    Hide();
            //    Base.Core.Game.LockCameraMovement = true;
            //}
        }

        private void LoadContent()
        {
            var statusText = "Good to go.";
            var errorLevel = -1;

            var isBuildingRequired = false;
            var isBuildingPossible = true;

            var isTilePurchaseRequired = false;
            var isSurroundingTileCheckRequired = false;

            var headerTextBuilder = new StringBuilder();

            headerTextBuilder.AppendLine("This is a fine looking tile of land.");

            if (this.tileBehaviour.Tile.IsOwned)
            {
                headerTextBuilder.AppendLine("And it's yours!");
            }
            else
            {
                isTilePurchaseRequired = true;
                headerTextBuilder.AppendLine("You currently don't own it - but you could.");
            }

            if (Base.Core.Game.State.World.Farm == default)
            {
                isBuildingRequired = true;
                isSurroundingTileCheckRequired = true;
                headerTextBuilder.AppendLine("But you need a farm to start farming...");
            }

            headerTextBuilder.AppendLine(String.Format("Biome type: {0}", GetBiomeDescription()));

            headerText.text = headerTextBuilder.ToString();

            if (isSurroundingTileCheckRequired)
            {
                var surroundingTiles = Base.Core.Game.TileController.GetSurroundingTiles(this.tileBehaviour.Tile);

                if (surroundingTiles.Count < 8)
                {
                    isBuildingPossible = false;
                    errorLevel = 4;
                    statusText = "Not enough space!";
                }
            }

            switch (errorLevel)
            {
                case 1: this.statusText.color = Color.green; break;
                case 2: this.statusText.color = Color.yellow; break;
                case 3: this.statusText.color = this.orangeColor; break;
                case 4:
                default:
                    this.statusText.color = Color.red; break;
            }

            this.statusText.text = statusText;
        }

        private String GetBiomeDescription()
        {
            if (this.tileBehaviour.Tile.Biome?.Type.Name != default)
            {
                return this.tileBehaviour.Tile.Biome.Type.Name;
            }

            return "Unknown";
        }

        private void DisplayBuildFarmContent()
        {
            //buyTileContent.SetActive(false);

            //biomeText.text = "";

            //var statusString = "You can build the Farm.";
            //var errorLevel = -1;

            //var isAbleToBuild = true;
            //var isAbleToBuyAndBuild = true;

            //if (!this.tileBehaviour.Tile.IsOwned)
            //{
            //    statusString = "You dont own the select land...";
            //    errorLevel = 4;

            //    isAbleToBuild = false;
            //    tilesTotalCost += this.tileBehaviour.Tile.Price;
            //}

            //if (isAbleToBuild)
            //{
            //    surroundingTiles = Base.Core.Game.TileController.GetSurroundingTiles(this.tileBehaviour.Tile);

            //    if (surroundingTiles.Count > 0)
            //    {
            //        if (surroundingTiles.Count != 8)
            //        {
            //            statusString = "Not enough space...";
            //            errorLevel = 1;

            //            isAbleToBuild = false;
            //            isAbleToBuyAndBuild = false;
            //        }
            //        else
            //        {
            //            foreach (var surroundingTile in surroundingTiles)
            //            {
            //                if (surroundingTile.Tile.Building != default)
            //                {
            //                    statusString = "One or more of the surrounding tiles are occupied!";
            //                    errorLevel = 2;

            //                    isAbleToBuild = false;
            //                    isAbleToBuyAndBuild = false;
            //                    break;
            //                }
            //                else if (surroundingTile.Tile.Field.Seed != default)
            //                {
            //                    statusString = "One or more of the surrounding fields are in use!";
            //                    errorLevel = 3;

            //                    isAbleToBuild = false;
            //                    isAbleToBuyAndBuild = false;
            //                    break;
            //                }
            //                else if (!surroundingTile.Tile.IsOwned)
            //                {
            //                    errorLevel = 1;
            //                    statusString = "You dont own all the surrounding tiles!";
            //                    tilesTotalCost += surroundingTile.Tile.Price;

            //                    isAbleToBuild = false;
            //                }
            //            }
            //        }
            //    }
            //}

            //if (Base.Core.Game.State.FarmStorage.MoneyBalance < tilesTotalCost)
            //{
            //    isAbleToBuyAndBuild = false;
            //    errorLevel = 4;

            //    statusString = "Not enought money to buy the required tiles!";
            //}

            //this.buildFarmStatusText.text = statusString;
            //this.buildFarmButton.interactable = isAbleToBuild;
            //this.buildBuyAndBuildFarmButton.interactable = isAbleToBuyAndBuild;

            //if (errorLevel == 1)
            //{
            //    this.buildCostBuyAndBuildContainer.SetActive(isAbleToBuyAndBuild);
            //    this.buildCostBuyAndBuildAmount.text = tilesTotalCost.ToString("n0");
            //}
            //else
            //{
            //    this.buildCostBuyAndBuildContainer.SetActive(false);
            //}

            //this.buildFarmButton.gameObject.SetActive(isAbleToBuild);
            //this.buildBuyAndBuildFarmButton.gameObject.SetActive(isAbleToBuyAndBuild);

            //var textColor = Color.green;

            //switch (errorLevel)
            //{
            //    case 1: textColor = Color.yellow; break;
            //    case 2: textColor = new Color(1, 0.647f, 0); break;
            //    case 3:
            //    case 4:
            //        textColor = Color.red;
            //        break;
            //}

            //buildFarmStatusText.color = textColor;

            //buildFarmContent.SetActive(true);
        }

        private void DisplayBuyTileContent()
        {
            //buildFarmContent.SetActive(false);

            //biomeText.text = "The Biome of the Field is unknown";

            //if (tileBehaviour.Tile.Biome != null)
            //{
            //    biomeText.text = "This field is in the " + tileBehaviour.Tile.Biome.Type.Name;
            //}

            //buyCostFundsAvailableText.text = Base.Core.Game.State.FarmStorage.MoneyBalance.ToString("n0");
            //buyCostTotalCostAmount.text = this.tileBehaviour.Tile.Price.ToString("n0");

            //var checkBuyAndBuild = Base.Core.Game.State.World.Farm == default;

            //this.buyTileAndBuildFarmButton.gameObject.SetActive(checkBuyAndBuild);
            //this.buyCostBuyAndBuildContainer.SetActive(checkBuyAndBuild);

            //if (checkBuyAndBuild)
            //{
            //    var isBuyAndBuildPossible = true;

            //    surroundingTiles = Base.Core.Game.TileController.GetSurroundingTiles(this.tileBehaviour.Tile);

            //    if (surroundingTiles.Count > 0)
            //    {
            //        if (surroundingTiles.Count != 8)
            //        {
            //            isBuyAndBuildPossible = false;
            //        }

            //        foreach (var surroundingTile in surroundingTiles)
            //        {
            //            if (surroundingTile.Tile.Building != default)
            //            {
            //                isBuyAndBuildPossible = false;
            //                break;
            //            }
            //            else if (!surroundingTile.Tile.IsOwned)
            //            {
            //                tilesTotalCost += surroundingTile.Tile.Price;
            //            }
            //        }
            //    }

            //    if (isBuyAndBuildPossible && Base.Core.Game.State.FarmStorage.MoneyBalance < tilesTotalCost)
            //    {
            //        isBuyAndBuildPossible = false;
            //    }

            //    this.buyCostBuyAndBuildAmount.text = tilesTotalCost.ToString("n0");

            //    if (isBuyAndBuildPossible)
            //    {
            //        this.buyTileButton.interactable = true;
            //        this.buyTileAndBuildFarmButton.interactable = true;

            //        this.buyCostBuyAndBuildContainer.SetActive(true);

            //        buyCouldBuyText.text = "But it could.\nClick 'Buy' or 'Buy and Build' below, and it is yours!\nMaybe even with the Farm!";
            //    }
            //    else if (Base.Core.Game.State.FarmStorage.MoneyBalance >= this.tileBehaviour.Tile.Price)
            //    {
            //        this.buyTileButton.interactable = true;
            //        this.buyTileAndBuildFarmButton.interactable = false;

            //        this.buyCostBuyAndBuildContainer.SetActive(checkBuyAndBuild);

            //        buyCouldBuyText.text = "But it could.\nClick 'Buy' below, and it is yours!\nDo it. Do it now!";
            //    }
            //    else
            //    {
            //        this.buyTileButton.interactable = false;
            //        this.buyTileAndBuildFarmButton.interactable = false;

            //        buyCouldBuyText.text = "And it will remain this way.\nGo earn some money!\nSlacker.";
            //    }
            //}
            //else
            //{
            //    if (Base.Core.Game.State.FarmStorage.MoneyBalance >= this.tileBehaviour.Tile.Price)
            //    {
            //        this.buyTileButton.interactable = true;
            //        buyCouldBuyText.text = "But it could.\nClick 'Buy' below, and it is yours!\nDo it. Do it now!";
            //    }
            //    else
            //    {
            //        this.buyTileButton.interactable = false;
            //        buyCouldBuyText.text = "And it will remain this way.\nGo earn some money!\nSlacker.";
            //    }
            //}

            //buyTileContent.SetActive(true);
        }

        private void SetVisibility(Boolean isVisible)
        {
            Base.Core.Game.LockCameraMovement = isVisible;

            this.visiblityContainer.SetActive(isVisible);
        }

        private void Awake()
        {
            this.visiblityContainer = transform.Find("TileViewToggle").gameObject;

            this.headerText = this.transform.Find("TileViewToggle/ContentContainer/TopTextArea/HeaderText").GetComponent<TMP_Text>();
            this.statusText = this.transform.Find("TileViewToggle/ContentContainer/TopTextArea/StatusText").GetComponent<TMP_Text>();

            this.buyButton = transform.Find("TileViewToggle/ActionsArea/BuyButton").gameObject;
            this.buyButtonText = buyButton.transform.Find("Text").GetComponent<Text>();
            this.buyAndBuildButton = transform.Find("TileViewToggle/ActionsArea/BuyAndBuildButton").gameObject;
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
