using System.Collections.Generic;
using Assets.Scripts.Base;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;
using Assets.Scripts.Prefabs.World;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using static UnityEditor.Progress;

public class SeedShopBehaviour : ViewBaseBehaviour
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

    public GameObject AnalyseUI;

    public Button buttonPrefab;
    private Button buttonPressed;

    private GameObject seedShopToggle;

    private int sellQuantity = 0;
    private int buyQuantity = 0;
    private int balance;

    private StorageItem chosenPlant;
    private StorageItem chosenSeed;
    private double plantValue;
    private double seedValue;

    private List<StorageItem> inventory;

    public override void Show()
    {
        base.Show();

        this.seedShopToggle.SetActive(true);

        // Update balance and inventory view
        balance = FarmStorageController.GetStorageBalance();
        inventory = FarmStorageController.getStorageInventory();
        fillList(inventory, Plants, true);
//        updateInfo(true);
//        updateInfo(false);
    }

    public override void Hide()
    {
        base.Hide();

        emptyList(Plants);
        resetInfo(true);
        resetInfo(false);

        this.seedShopToggle.SetActive(false);
    }

    // Start is called before the first frame update
    // TODO change to OnStart
    protected override void OnStart()
    {
        base.OnStart();

        this.seedShopToggle = transform.Find("SeedShopToggle").gameObject;

        // Initial balance and invetory
        balance = FarmStorageController.GetStorageBalance();

        fillList(Core.Game.State.AvailableShopItems, Seeds, false);
    }

    // TODO cleanup these three methods (low priority)
    private void ItemSelected(StorageItem item, bool isPlant, Button button)
    {
        if (isPlant)
        {
            PlantSelected(item);
            updateAnalyseView();
        }
        else
        {
            SeedSelected(item);
        }

        buttonPressed = button;
    }

    private void PlantSelected(StorageItem item)
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

    private void SeedSelected(StorageItem item)
    {
        chosenSeed = FarmStorageController.getStorageItemToPlant(Core.Game.State.FarmStorage, item.Plant);

        ChromosomePair pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDSVALUE];
        seedValue = GrowthController.getDominantChromosome(pair).Value0;

        updatePrice(false);
        updateInfo(false);
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
        if (chosenPlant == null)
            return;

        int amount = sellAll ? chosenPlant.StorageAmountPlants : sellQuantity;

        if (amount == 0)
            return;

        FarmStorageController.TakePlantOfStorage(chosenPlant.Plant, amount);
        FarmStorageController.PutMoneyInStorage((int)(plantValue * amount));

        stateUpdate(true);

        Core.Game.EffectsAudioManager.Play("MoneyRain");
    }

    // Buy specified amount
    public void Buy()
    {
        // Buy
        if (seedValue * buyQuantity > FarmStorageController.GetStorageBalance() || buyQuantity == 0)
        {
            Core.Game.EffectsAudioManager.Play("Error");
            return;
        }


        int amount = FarmStorageController.PutSeedInStorage(chosenSeed.Plant, buyQuantity);
        FarmStorageController.TakeMoneyOfStorage((int)(amount * seedValue));

        stateUpdate(false);

        Core.Game.EffectsAudioManager.Play("MoneyRain");
    }


    // Analyses the chosen plant
    public void Analyse()
    {
        if (chosenPlant == null)
            return;

        Analyzer plantAnalyzer = Core.Game.State.PlantAnalyzer;
        int plantCost = plantAnalyzer.CurrentDevelopmentStage.AnalyticsPlantCost;
        int moneyCost = plantAnalyzer.CurrentDevelopmentStage.AnalyticsCost;

        if (plantCost <= chosenPlant.StorageAmountPlants && moneyCost <= balance)
        {
            InheritanceController.AnalysePlant(chosenPlant.Plant, plantAnalyzer);
            FarmStorageController.TakePlantOfStorage(chosenPlant.Plant, plantCost);
            FarmStorageController.TakeMoneyOfStorage(moneyCost);
        }
        stateUpdate(true);

        // Show updated info if same seed selected
        if (chosenPlant == chosenSeed)
            updateInfo(false);

        updateAnalyseView();

    }

    private bool checkAnalysability(Plant plant)
    {
        List<ChromosomePair> pairs = new List<ChromosomePair>();
        pairs.AddRange(plant.Genome.Values);

        int numVisible = 0;
        for (int i = 0; i < pairs.Count; i++)
        {
            if (pairs[i].IsVisible)
                numVisible++;
        }

        // Not yet all values analysed
        return numVisible < 6;
    }

    private void updateAnalyseView()
    {
        if (chosenPlant == null || !checkAnalysability(chosenPlant.Plant))
        {
            AnalyseUI.SetActive(false);
            return;
        }

        AnalyseUI.SetActive(true);

        // Image
        AnalyseUI.transform.Find("PlantImage").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        AnalyseUI.transform.Find("PlantImage").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(chosenPlant.Plant.ImageName);

        Analyzer plantAnalyzer = Core.Game.State.PlantAnalyzer;
        // Values
        AnalyseUI.transform.Find("PlantCost").GetComponent<TMP_Text>().text = plantAnalyzer.CurrentDevelopmentStage.AnalyticsPlantCost.ToString();
        AnalyseUI.transform.Find("MoneyCost").GetComponent<TMP_Text>().text = plantAnalyzer.CurrentDevelopmentStage.AnalyticsCost.ToString();

    }

    // Fills scrollview with item buttons
    private void fillList(List<StorageItem> items, ScrollRect scrollRect, bool isPlant)
    {
        foreach (StorageItem item in items)
        {
            if (isPlant && item.StorageAmountPlants == 0)
                continue;

            Debug.Log($"Adding plant {item.Plant.Name} now.");
            Button newItem = Instantiate(buttonPrefab);
            newItem.GetComponentInChildren<TMP_Text>().text = item.Plant.Name;
            newItem.transform.SetParent(scrollRect.transform.Find("Viewport").Find("Content"));

            newItem.onClick.AddListener(() => { ItemSelected(item, isPlant, newItem); });
        }
    }

    // Clear scroll view of all buttons DON'T USE, BROKEN!
    private void emptyList(ScrollRect scrollRect)
    {
        Transform content = scrollRect.transform.Find("Viewport").Find("Content");
        foreach (Transform child in content) {
            GameObject.Destroy(child.gameObject);
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
        // Need to update amount
        updateInfo(isSell);

        if (isSell && chosenPlant.StorageAmountPlants == 0)
        {
            resetInfo(true);
            buttonPressed.onClick.RemoveAllListeners();
            Destroy(buttonPressed.gameObject);
            chosenPlant = null;
        }
    }


    // Update price of selling and buying
    private void updatePrice(bool isSell)
    {
        if (isSell && chosenPlant == null || !isSell && chosenSeed == null)
            return;

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

    // Update plant or seed info
    private void updateInfo(bool isSell)
    {
        StorageItem item = isSell ? chosenPlant : chosenSeed;

        if (item == null)
            return;

        if (isSell)
            PlantInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, null, true);
        else
            SeedInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, null, false);
    }

    private void resetInfo(bool isSell)
    {
        if (isSell)
            PlantInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(null, null, true);
        else
            SeedInfo.GetComponent<InformationPrefabBehaviour>().UpdateInfo(null, null, false);


    }
}