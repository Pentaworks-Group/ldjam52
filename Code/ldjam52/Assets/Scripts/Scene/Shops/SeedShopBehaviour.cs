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

        stateUpdate(true);
    }

    // Buy specified amount
    public void Buy()
    {
        int amount = FarmStorageController.PutSeedInStorage(chosenSeed.Plant, buyQuantity);
        FarmStorageController.TakeMoneyOfStorage((int)(amount * seedValue));

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

    private void updateInfo(bool isSell)
    {
        StorageItem item = isSell ? chosenPlant : chosenSeed;

        if (isSell)
            PlantInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, null);
        else
            SeedInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, null);
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