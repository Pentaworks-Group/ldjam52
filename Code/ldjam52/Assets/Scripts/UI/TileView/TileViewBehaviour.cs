using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Base;
using Assets.Scripts.Model;

public class TileViewBehaviour : MonoBehaviour
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


    private TileViewSeedListDetailsBehaviour parent1;
    private TileViewSeedListDetailsBehaviour parent2;


    private Text harvestSeedAmount;
    private Text harvestPlantAmount;
    private Image harvestSeedPic;
    private Image harvestPlantPic;

    private void Awake()
    {
        viewToggle = transform.Find("TileViewToggle")?.gameObject;
        currentInfo = transform.Find("TileViewToggle/CurrentInfo")?.gameObject;
        plantingOptions = transform.Find("TileViewToggle/PlantingOptions")?.gameObject;
        harvestResult = transform.Find("TileViewToggle/HarvestResult")?.gameObject;

        plantImage = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Image")?.GetComponent<Image>();
        plantName = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Name")?.GetComponent<Text>();
        plantedTime = transform.Find("TileViewToggle/CurrentInfo/PlantedTime/Value")?.GetComponent<Text>();
        harvestButton = transform.Find("TileViewToggle/CurrentInfo/Buttons/Harvest")?.GetComponent<Button>();

        parent1 = transform.Find("TileViewToggle/PlantingOptions/Selection/Selected1")?.GetComponent<TileViewSeedListDetailsBehaviour>();
        parent2 = transform.Find("TileViewToggle/PlantingOptions/Selection/Selected2")?.GetComponent<TileViewSeedListDetailsBehaviour>();


        harvestSeedAmount = transform.Find("TileViewToggle/HarvestResult/Seeds/Amount")?.GetComponent<Text>();
        harvestPlantAmount = transform.Find("TileViewToggle/HarvestResult/Plants/Amount")?.GetComponent<Text>();
        harvestSeedPic = transform.Find("TileViewToggle/HarvestResult/Seeds/Pic")?.GetComponent<Image>();
        harvestPlantPic = transform.Find("TileViewToggle/HarvestResult/Plants/Pic")?.GetComponent<Image>();
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
        currentlyViewedField.HarvestCrop();
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

    public void SelectParent1(TileViewSeedListDetailsBehaviour plantBehaviour)
    {
        parent1.DisplaySeedDetails(plantBehaviour.GetPlant());
    }

    public void SelectParent2(TileViewSeedListDetailsBehaviour plantBehaviour)
    {
        parent2.DisplaySeedDetails(plantBehaviour.GetPlant());
    }

    public void SelectParent1Slot(TileViewSeedListSlotBehaviour slotBehaviour)
    {
        parent1.DisplaySeedDetails(slotBehaviour.GetPlant());
    }

    public void SelectParent2Slot(TileViewSeedListSlotBehaviour slotBehaviour)
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
    }

}
