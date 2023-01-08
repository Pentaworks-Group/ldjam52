using Assets.Scripts.Base;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewBehaviour : MonoBehaviour
{
    private FieldBehaviour currentlyViewedField;

    private GameObject viewToggle;
    private GameObject currentInfo;
    private GameObject plantingOptions;
    private GameObject harvestResult;

    private Image plantImage;
    private Text plantName;
    private Text plantedTime;
    private Button harvestButton;

    private FieldViewSeedListBehaviour seedList;
    private FieldViewSeedListDetailsBehaviour parent1;
    private FieldViewSeedListDetailsBehaviour parent2;

    private Text harvestSeedAmount;
    private Text harvestPlantAmount;
    private Image harvestSeedPic;
    private Image harvestPlantPic;

    private void Awake()
    {
        viewToggle = transform.Find("FieldViewToggle")?.gameObject;
        currentInfo = transform.Find("FieldViewToggle/CurrentInfo")?.gameObject;
        plantingOptions = transform.Find("FieldViewToggle/PlantingOptions")?.gameObject;
        harvestResult = transform.Find("FieldViewToggle/HarvestResult")?.gameObject;

        plantImage = transform.Find("FieldViewToggle/CurrentInfo/CurrentPlant/Image")?.GetComponent<Image>();
        plantName = transform.Find("FieldViewToggle/CurrentInfo/CurrentPlant/Name")?.GetComponent<Text>();
        plantedTime = transform.Find("FieldViewToggle/CurrentInfo/PlantedTime/Value")?.GetComponent<Text>();
        harvestButton = transform.Find("FieldViewToggle/CurrentInfo/Buttons/Harvest")?.GetComponent<Button>();

        seedList = transform.Find("FieldViewToggle/PlantingOptions/SeedList/ListContainer")?.GetComponent<FieldViewSeedListBehaviour>();
        parent1 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected1")?.GetComponent<FieldViewSeedListDetailsBehaviour>();
        parent2 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected2")?.GetComponent<FieldViewSeedListDetailsBehaviour>();


        harvestSeedAmount = transform.Find("FieldViewToggle/HarvestResult/Seeds/Amount")?.GetComponent<Text>();
        harvestPlantAmount = transform.Find("FieldViewToggle/HarvestResult/Plants/Amount")?.GetComponent<Text>();
        harvestSeedPic = transform.Find("FieldViewToggle/HarvestResult/Seeds/Pic")?.GetComponent<Image>();
        harvestPlantPic = transform.Find("FieldViewToggle/HarvestResult/Plants/Pic")?.GetComponent<Image>();
    }

    public void ViewField(FieldBehaviour fieldBehaviour)
    {
        currentlyViewedField = fieldBehaviour;
        viewToggle.SetActive(true);
        Core.Game.LockCameraMovement = true;
        UpdateView();
    }

    private void UpdateView()
    {
        if (currentlyViewedField.Field.Seed != null)
        {
            currentInfo.SetActive(true);
            plantingOptions.SetActive(false);
            plantName.text = currentlyViewedField.Field.Seed.Name;
            plantedTime.text = currentlyViewedField.Field.TimePlanted.ToString();
            plantImage.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(currentlyViewedField.Field.Seed.ImageName);
            if (currentlyViewedField.IsFullyGrown())
            {
                harvestButton.interactable = true;
            }
            else
            {
                harvestButton.interactable = false;
            }
        }
        else
        {
            currentInfo.SetActive(false);
            plantingOptions.SetActive(true);

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
    }


    public void FertilizeCrop()
    {
        currentlyViewedField.FertilizeCrop();
        UpdateView();
    }

    public void DestroyCrop()
    {
        currentlyViewedField.DestroyCrop();
        UpdateView();
    }

    public void CloseView()
    {
        currentlyViewedField = null;
        currentInfo.SetActive(false);
        viewToggle.SetActive(false);
        plantingOptions.SetActive(false);
        harvestResult.SetActive(false);
        Core.Game.LockCameraMovement = false;
    }

    public void SelectParent1(FieldViewSeedListDetailsBehaviour plantBehaviour)
    {
        parent1.DisplaySeedDetails(plantBehaviour.GetPlant());
    }

    public void SelectParent2(FieldViewSeedListDetailsBehaviour plantBehaviour)
    {
        parent2.DisplaySeedDetails(plantBehaviour.GetPlant());
    }

    public void SelectParent1Slot(FieldViewSeedListSlotBehaviour slotBehaviour)
    {
        parent1.DisplaySeedDetails(slotBehaviour.GetPlant());
    }

    public void SelectParent2Slot(FieldViewSeedListSlotBehaviour slotBehaviour)
    {
        parent2.DisplaySeedDetails(slotBehaviour.GetPlant());
    }

    public void PlantSeeds()
    {
        currentlyViewedField.PlantSeeds(parent1.GetPlant(), parent2.GetPlant());
        UpdateView();
    }

    public void HideHarvestResult()
    {
        harvestResult.SetActive(false);
    }

    public void ShowHarvestResult(HarvestResult result)
    {
        harvestResult.SetActive(true);
        harvestSeedAmount.text = result.NumSeeds.ToString();
        harvestPlantAmount.text = result.NumHarvest.ToString();
        harvestSeedPic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(result.Plant.SeedImageName);
        harvestPlantPic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(result.Plant.ImageName);
        seedList.UpdateList();
    }

}
