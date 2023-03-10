using Assets.Scripts.Base;
using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;
using Assets.Scripts.Prefabs.World;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewBehaviour : ViewBaseBehaviour
{
    private FieldBehaviour currentlyViewedField;

    private GameObject viewToggle;
    private GameObject currentInfo;
    private GameObject plantingOptions;
    private GameObject harvestResult;
    private GameObject infoPanel;
    private GameObject currentInfoPanel;

    private Image plantImage;
    private Text plantName;
    private Button harvestButton;
    private Button harvestAndReplantButton;

    private FieldViewSeedListBehaviour seedList;
    private FieldViewSeedListDetailsBehaviour parent1;
    private FieldViewSeedListDetailsBehaviour parent2;
    private Button plantButton;

    private Text harvestSeedAmount;
    private Text harvestPlantAmount;
    private Text harvestPlantName;
    private Image harvestSeedPic;
    private Image harvestPlantPic;

    private Text analyzeCosts;
    private Button analyzeButton;

    private Text currentGrowingTime;
    private Text currentGrowingProcess;

    public TMP_Text errorMsg;

    public TMP_Text estimatedDuration;




    private void Awake()
    {
        viewToggle = transform.Find("FieldViewToggle").gameObject;
        currentInfo = transform.Find("FieldViewToggle/CurrentInfo").gameObject;
        plantingOptions = transform.Find("FieldViewToggle/PlantingOptions").gameObject;
        harvestResult = transform.Find("FieldViewToggle/HarvestResult").gameObject;

        plantName = transform.Find("FieldViewToggle/CurrentInfo/CurrentPlant/Name").GetComponent<Text>();
        harvestButton = transform.Find("FieldViewToggle/CurrentInfo/Buttons/Harvest").GetComponent<Button>();
        harvestAndReplantButton = transform.Find("FieldViewToggle/CurrentInfo/Buttons/HarvestAndReplant").GetComponent<Button>();

        seedList = transform.Find("FieldViewToggle/PlantingOptions/SeedList/ListContainer").GetComponent<FieldViewSeedListBehaviour>();
        parent1 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected1").GetComponent<FieldViewSeedListDetailsBehaviour>();
        parent2 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected2").GetComponent<FieldViewSeedListDetailsBehaviour>();
        plantButton = transform.Find("FieldViewToggle/PlantingOptions/Selection/PlantButton").GetComponent<Button>();


        harvestSeedAmount = transform.Find("FieldViewToggle/HarvestResult/Body/Seeds/Amount").GetComponent<Text>();
        harvestPlantAmount = transform.Find("FieldViewToggle/HarvestResult/Body/Plants/Amount").GetComponent<Text>();
        harvestPlantName = transform.Find("FieldViewToggle/HarvestResult/Header/PlantName").GetComponent<Text>();
        harvestSeedPic = transform.Find("FieldViewToggle/HarvestResult/Body/Seeds/Pic").GetComponent<Image>();
        harvestPlantPic = transform.Find("FieldViewToggle/HarvestResult/Body/Plants/Pic").GetComponent<Image>();

        infoPanel = transform.Find("FieldViewToggle/PlantingOptions/SeedList/Details/NameAndPic/Information").gameObject;
        analyzeCosts = transform.Find("FieldViewToggle/PlantingOptions/SeedList/Details/NameAndPic/Information/AnalyzeCosts").GetComponent<Text>();
        analyzeButton = transform.Find("FieldViewToggle/PlantingOptions/SeedList/Details/NameAndPic/Information/AnalyzeButton").GetComponent<Button>();

        currentInfoPanel = transform.Find("FieldViewToggle/CurrentInfo/Information").gameObject;
        currentGrowingTime = transform.Find("FieldViewToggle/CurrentInfo/GrowingTime/Value").GetComponent<Text>();
        currentGrowingProcess = transform.Find("FieldViewToggle/CurrentInfo/GrowingProcess/Value").GetComponent<Text>();

    }

    private void Update()
    {
        if (viewToggle.activeSelf)
        {
            CheckHarvestButtons();

            CheckAnalyzeButton();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBackOrClose();
            }


            if (currentGrowingTime != null && currentlyViewedField != null)
            {
                currentGrowingTime.text = Mathf.RoundToInt(Core.Game.State.ElapsedTime - currentlyViewedField.Field.TimePlanted).ToString() + "s";
                currentGrowingProcess.text = Mathf.RoundToInt((float)currentlyViewedField.Field.GrowthProgress * 100).ToString() + "%";
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && plantButton.IsInteractable())
        {
            PlantSeeds();
        }


    }

    private void GoBackOrClose()
    {
        if (harvestResult.activeSelf)
        {
            HideHarvestResult();
        }
        else
        {
            Hide();
        }
    }

    public void Show(FieldBehaviour fieldBehaviour)
    {
        Show();

        currentlyViewedField = fieldBehaviour;
        viewToggle.SetActive(true);
        UpdateView();
    }

    public Field GetField()
    {
        if (currentlyViewedField != null)
        {
            return currentlyViewedField.Field;
        }

        return null;
    }

    private void UpdateView(StorageItem item = null)
    {
        if (currentlyViewedField.Field.Seed != null)
        {
            if (item == null)
            {
                item = FarmStorageController.getStorageItemToPlant(Core.Game.State.FarmStorage, currentlyViewedField.Field.Seed, false);
                if (item == null)
                {
                    item = new StorageItem
                    {
                        Plant = currentlyViewedField.Field.Seed
                    };
                }
            }
            currentInfo.SetActive(true);
            plantingOptions.SetActive(false);
            plantName.text = currentlyViewedField.Field.Seed.Name;
            currentInfoPanel.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, GetField(), true);
            int growTime = Mathf.RoundToInt((float)(1.0f / GrowthController.getGrowthRate(currentlyViewedField.Field, currentlyViewedField.Field.Seed)));
            estimatedDuration.text = growTime.ToString() + "s";

        }
        else
        {
            currentInfo.SetActive(false);
            plantingOptions.SetActive(true);
        }

        seedList.UpdateList();
        analyzeCosts.text = Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage.AnalyticsCost.ToString();

        infoPanel.GetComponent<InformationPrefabBehaviour>().UpdateInfo(item, GetField(), false);

        CheckPlantingButton();
    }

    public void CheckHarvestButtons()
    {
        if (currentlyViewedField != default && currentlyViewedField.IsFullyGrown())
        {
            if (harvestButton.interactable == false)
            {
                harvestButton.interactable = true;
                harvestAndReplantButton.interactable = true;
            }
        }
        else
        {
            if (harvestButton.interactable == true)
            {
                harvestButton.interactable = false;
                harvestAndReplantButton.interactable = false;
            }
        }
    }

    public void CheckPlantingButton()
    {
        var p1Plant = parent1.Plant;
        var p2Plant = parent2.Plant;
        if (currentlyViewedField != default && (p1Plant != default || p2Plant != default))
        {
            if (p1Plant != default && p2Plant != default)
            {
                if (p1Plant.ID == p2Plant.ID)
                {
                    if (FarmStorageController.GetSeedCountInStorage(p1Plant) >= 2)
                    {
                        plantButton.interactable = true;
                        errorMsg.gameObject.SetActive(false);
                    }
                    else
                    {
                        plantButton.interactable = false;
                        errorMsg.text = "Not enough seeds.";
                        errorMsg.gameObject.SetActive(true);
                    }

                }
                else if (FarmStorageController.GetSeedCountInStorage(p1Plant) >= 1 && FarmStorageController.GetSeedCountInStorage(p2Plant) >= 1)
                {
                    plantButton.interactable = true;
                    errorMsg.gameObject.SetActive(false);
                }
                else
                {
                    plantButton.interactable = false;
                    errorMsg.text = "Not enough seeds.";
                    errorMsg.gameObject.SetActive(true);
                }
            }
            else if (p1Plant != default && FarmStorageController.GetSeedCountInStorage(p1Plant) >= 1)
            {
                plantButton.interactable = true;
                errorMsg.gameObject.SetActive(false);
            }
            else if (p2Plant != default && FarmStorageController.GetSeedCountInStorage(p2Plant) >= 1)
            {
                plantButton.interactable = true;
                errorMsg.gameObject.SetActive(false);
            }
            else
            {
                plantButton.interactable = false;
                errorMsg.text = "Not enough seeds.";
                errorMsg.gameObject.SetActive(true);
            }
        }
        else
        {
            plantButton.interactable = false;
            errorMsg.text = "Choose a seed.";
            errorMsg.gameObject.SetActive(true);
        }
    }

    private void CheckAnalyzeButton()
    {
        if (analyzeButton != null)
        {
            var canAnalyze = true;

            if (Core.Game.State.FarmStorage.MoneyBalance < Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage.AnalyticsCost)
            {
                canAnalyze = false;
            }

            if (canAnalyze)
            {
                var field = GetField();

                if (field.IsSunshineVisible && field.IsTemperatureVisible && field.IsFertiliyVisible && field.IsHumidityVisible)
                {
                    canAnalyze = false;
                }
            }

            analyzeButton.interactable = canAnalyze;
        }
    }

    public void HarvestCrop()
    {
        var harvest = currentlyViewedField.HarvestCrop();

        if (harvest != null)
        {
            ShowHarvestResult(harvest);
        }

        UpdateView();
        Core.Game.PlayButtonSound();
    }

    public void HarvestAndReplantCrop()
    {
        var harvest = currentlyViewedField.HarvestCrop();


        currentlyViewedField.PlantSeeds(harvest.Plant, default);

        Hide();
    }

    public void FertilizeCrop()
    {
        currentlyViewedField.FertilizeCrop();
        UpdateView();
        Core.Game.PlayButtonSound();
    }

    public void DestroyCrop()
    {
        currentlyViewedField.DestroyCrop();
        UpdateView();
        Core.Game.PlayButtonSound();
    }

    public override void Hide()
    {
        base.Hide();

        currentlyViewedField = null;
        currentInfo.SetActive(false);
        viewToggle.SetActive(false);
        plantingOptions.SetActive(false);
        harvestResult.SetActive(false);
    }

    public void SelectParent1(InformationPrefabBehaviour plantBehaviour)
    {
        if (plantBehaviour.Item != null)
        {
            parent1.DisplaySeedDetails(plantBehaviour.Item.Plant);
            Core.Game.PlayButtonSound();
            CheckPlantingButton();
        }
    }

    public void SelectParent2(InformationPrefabBehaviour plantBehaviour)
    {
        if (plantBehaviour.Item != null)
        {
            parent2.DisplaySeedDetails(plantBehaviour.Item.Plant);
            Core.Game.PlayButtonSound();
            CheckPlantingButton();
        }
    }

    public void SelectParent1Slot(FieldViewSeedListSlotBehaviour slotBehaviour)
    {
        parent1.DisplaySeedDetails(slotBehaviour.GetPlant());
        Core.Game.PlayButtonSound();
        CheckPlantingButton();
    }

    public void SelectParent2Slot(FieldViewSeedListSlotBehaviour slotBehaviour)
    {
        parent2.DisplaySeedDetails(slotBehaviour.GetPlant());
        Core.Game.PlayButtonSound();
        CheckPlantingButton();
    }

    public void PlantSeeds()
    {
        currentlyViewedField.PlantSeeds(parent1.Plant, parent2.Plant);
        UpdateView();
        Core.Game.PlayButtonSound();
    }

    public void HideHarvestResult()
    {
        harvestResult.SetActive(false);
        Core.Game.PlayButtonSound();
    }

    public void ShowHarvestResult(HarvestResult result)
    {
        harvestResult.SetActive(true);
        harvestSeedAmount.text = "You harvested " + result.NumSeeds.ToString() + " Seeds";
        harvestPlantAmount.text = "You harvested " + result.NumHarvest.ToString() + " Plants";
        harvestPlantName.text = result.Plant.Name;
        harvestSeedPic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(result.Plant.SeedImageName);
        harvestPlantPic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(result.Plant.ImageName);
        seedList.UpdateList();
    }


    public void ClearSelectedParent1()
    {
        parent1.ClearDisplayDetails();
        Core.Game.PlayButtonSound();
        CheckPlantingButton();
    }

    public void ClearSelectedParent2()
    {
        parent2.ClearDisplayDetails();
        Core.Game.PlayButtonSound();
        CheckPlantingButton();
    }

    public void ClearSelectedParents()
    {
        parent1.ClearDisplayDetails();
        parent2.ClearDisplayDetails();
        Core.Game.PlayButtonSound();
        CheckPlantingButton();
    }

    public void AnalyseField(InformationPrefabBehaviour plantBehaviour)
    {
        if (Core.Game.State.FarmStorage.MoneyBalance >= Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage.AnalyticsCost)
        {
            var field = GetField();

            InheritanceController.AnalyseField(field, Core.Game.State.FieldAnalyzer);

            plantBehaviour.UpdateInfo(plantBehaviour.Item, field, false);

            FarmStorageController.TakeMoneyOfStorage(Core.Game.State.FieldAnalyzer.CurrentDevelopmentStage.AnalyticsCost);

            Core.Game.PlayButtonSound();
        }
    }


}
