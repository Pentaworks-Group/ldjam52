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

public class SeedShopBehaviour : MonoBehaviour
{
    public TMP_Text SellQuantityText;
    public TMP_Text BuyQuantityText;

    public TMP_Text SellMoney;
    public TMP_Text BuyMoney;
    public TMP_Text BalanceText;

    public GameObject PlantInfo;
    public GameObject SeedInfo;

    public ScrollRect Plants;
    public ScrollRect Seeds;

    public Button buttonPrefab;

    public Sprite[] plantImages;

    private int sellQuantity = 0;
    private int buyQuantity = 0;
    private int balance;

    private StorageItem chosenPlant;
    private StorageItem chosenSeed;
    private double plantValue;
    private double seedValue;

    private List<StorageItem> inventory;

    // Start is called before the first frame update
    void Start()
    {

        if (Assets.Scripts.Base.Core.Game.State == default)
        {
            InitializeGameState();
        }

        balance = FarmStorageController.GetStorageBalance();
        BalanceText.text = balance.ToString();

        inventory = FarmStorageController.getStorageInventory();
      
        fillPlants();
    }

    public void PlantSelected(StorageItem item)
    {
        chosenPlant = item;

        ChromosomePair pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.PLANTVALUE];
        plantValue = GrowthController.getDominantChromosome(pair).Value0;

        sellQuantity = Mathf.Min(chosenPlant.StorageAmountPlants, sellQuantity);
        SellQuantityText.text = sellQuantity.ToString();

        Debug.Log("PlantSelected: " + item.Plant.Name);

        updatePrice(true);
        updateInfo(true);
    }

    public void SeedSelected(StorageItem item)
    {
        chosenSeed = item;

        ChromosomePair pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDSVALUE];
        seedValue = GrowthController.getDominantChromosome(pair).Value0;

        updatePrice(false);
        updateInfo(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Increase quantity to sell/buy
    public void Up(bool isSell)
    {
        if (isSell)
        {
            if (chosenPlant == null)
                return;
            sellQuantity = Mathf.Min(chosenPlant.StorageAmountPlants, ++sellQuantity);
            SellQuantityText.text = sellQuantity.ToString();
        }
        else
        {
            if (chosenSeed == null)
                return;
            buyQuantity++;
            BuyQuantityText.text = buyQuantity.ToString();
        }

        updatePrice(isSell);
    }

    // Decrease quantity to sell/buy
    public void Down(bool isSell)
    {
        if (isSell)
        {
            if (chosenPlant == null)
                return;
            sellQuantity = Mathf.Max(0, --sellQuantity);
            SellQuantityText.text = sellQuantity.ToString();
        }
        else
        {
            if (chosenSeed == null)
                return;
            buyQuantity = Mathf.Max(0, --buyQuantity);
            BuyQuantityText.text = buyQuantity.ToString();
        }

        updatePrice(isSell);
    }

    // Sell specified amount or all
    public void Sell(bool sellAll)
    {
        int amount = sellAll ? chosenPlant.StorageAmountPlants : sellQuantity;
        FarmStorageController.TakePlantOfStorage(chosenPlant.Plant, amount);
        FarmStorageController.PutMoneyInStorage((int)(plantValue * amount));

        // TODO Update info, plants list, money...
        stateUpdate(true);
    }

    // Buy specified amount
    public void Buy()
    {
        int amount = FarmStorageController.PutSeedInStorage(chosenSeed.Plant, buyQuantity);
        FarmStorageController.TakeMoneyOfStorage((int)(amount * seedValue));

        // TODO Update info, plants list, money...
        stateUpdate(false);
    }

    private void fillPlants()
    {
        foreach (StorageItem item in inventory)
        {
            Debug.Log($"Adding plant {item.Plant.Name} now.");
            Button newItem = Instantiate(buttonPrefab);
            newItem.GetComponentInChildren<TMP_Text>().text = item.Plant.Name;
            newItem.transform.SetParent(Plants.transform.GetChild(0).GetChild(0));

            // TODO need to remove listener when leaving shop
            newItem.onClick.AddListener(() => { PlantSelected(item); });
        }
    }

    private void fillSeeds()
    {
        foreach (StorageItem item in inventory)
        {
            Button newItem = Instantiate(buttonPrefab);
            newItem.GetComponentInChildren<TMP_Text>().text = item.Plant.Name;
            newItem.transform.SetParent(Seeds.transform.GetChild(0).GetChild(0));

            // TODO need to remove listener when leaving shop
            newItem.onClick.AddListener(() => { PlantSelected(item); });
        }
    }

    // TODO complete
    private void stateUpdate(bool isSell)
    {
        balance = FarmStorageController.GetStorageBalance();
        BalanceText.text = balance.ToString();

        if (isSell)
        {
            sellQuantity = Mathf.Min(chosenPlant.StorageAmountPlants, sellQuantity);
            SellQuantityText.text = sellQuantity.ToString();

        }

        updatePrice(isSell);
        updateInfo(isSell);
    }

    private void updatePrice(bool isSell)
    {
        Plant plant = isSell ? chosenPlant.Plant : chosenSeed.Plant;
        double value = isSell ? plantValue : seedValue;
        int quantity = isSell ? sellQuantity : buyQuantity;

        int total = (int)(value * quantity);

        if (isSell)
        {
            SellMoney.text = total.ToString();
        }
        else
        {
            BuyMoney.text = total.ToString();
        }
    }

    // TODO put in separate method that is called with item and iformation
    private void updateInfo(bool isSell)
    {

        StorageItem item = isSell ? chosenPlant : chosenSeed;
        Transform information = isSell ? PlantInfo.transform : SeedInfo.transform;

        // Always shown
        information.Find("Name").GetComponent<TMP_Text>().text = item.Plant.Name;
        information.Find("Amount").GetComponent<TMP_Text>().text = "Amount: " + (isSell ? item.StorageAmountPlants : item.StorageAmountSeeds).ToString();
        information.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        information.Find("Image").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(item.Plant.ImageName);

        ChromosomePair pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDSVALUE];
        information.Find("SeedsValue").GetComponent<TMP_Text>().text = "Seed value: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();

        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.PLANTVALUE];
        information.Find("PlantsValue").GetComponent<TMP_Text>().text = "Plant value: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();

        // Visibility checks needed
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDS];
        if (pair.IsVisible)
            information.Find("Seeds").GetComponent<TMP_Text>().text = "Seeds: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();
        else
            information.Find("Seeds").GetComponent<TMP_Text>().text = "Seeds: ?";

        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.HARVEST];
        if (pair.IsVisible)
            information.Find("Harvest").GetComponent<TMP_Text>().text = "Harvest: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();
        else
            information.Find("Harvest").GetComponent<TMP_Text>().text = "Harvest: ?";

        // Stat bars
        // TODO put into StatsBar
        drawStatsBar(information, "Temp", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Sun", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Water", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Fertility", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);

        /*
        StatsBar bar = information.Find("Temp").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Sun").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SUN];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Water").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.WATER];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Fertility").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.FERTILITY];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }
        */
    }

    private void drawStatsBar(Transform infoPanel, string barName, ChromosomePair pair)
    {
        StatsBar bar = infoPanel.Find(barName).GetChild(0).GetComponent<StatsBar>();

        if (pair.IsVisible || true)
        {
            bar.SetPlantValues(GrowthController.getDominantChromosome(pair).Value0, GrowthController.getDominantChromosome(pair).ValueDev);
            bar.ShowPlantValue();
        }
    }



    // TEST
    protected void InitializeGameState()
    {
        LoadGameSettings();
        // Maybe add a Tutorial scene, where the user can set "skip" for the next time.
        var gameState = new GameState()
        {
            CurrentScene = "SeedShopScene",
            GameMode = Core.Game.AvailableGameModes[0]
        };

        gameState.FarmStorage = gameState.GameMode.Player.StartingFarmStorage;
        Assets.Scripts.Base.Core.Game.PopulateKnownPlants(gameState);

        Assets.Scripts.Base.Core.Game.Start(gameState);
    }

    public void LoadGameSettings()
    {
        if (Core.Game.AvailableGameModes.Count == 0)
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
                Core.Game.AvailableGameModes.Add(gameMode);
            }
        }

        if (Core.Game.SelectedGameMode == default)
        {
            Core.Game.SelectedGameMode = loadedGameModes[0];
        }

        return loadedGameModes;
    }
}