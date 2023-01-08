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
    private Button plantButton;

    private Text harvestSeedAmount;
    private Text harvestPlantAmount;
    private Image harvestSeedPic;
    private Image harvestPlantPic;

    private void Awake()
    {
        viewToggle = transform.Find("FieldViewToggle").gameObject;
        currentInfo = transform.Find("FieldViewToggle/CurrentInfo").gameObject;
        plantingOptions = transform.Find("FieldViewToggle/PlantingOptions").gameObject;
        harvestResult = transform.Find("FieldViewToggle/HarvestResult").gameObject;

        plantImage = transform.Find("FieldViewToggle/CurrentInfo/CurrentPlant/Image").GetComponent<Image>();
        plantName = transform.Find("FieldViewToggle/CurrentInfo/CurrentPlant/Name").GetComponent<Text>();
        plantedTime = transform.Find("FieldViewToggle/CurrentInfo/PlantedTime/Value").GetComponent<Text>();
        harvestButton = transform.Find("FieldViewToggle/CurrentInfo/Buttons/Harvest").GetComponent<Button>();

        seedList = transform.Find("FieldViewToggle/PlantingOptions/SeedList/ListContainer").GetComponent<FieldViewSeedListBehaviour>();
        parent1 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected1").GetComponent<FieldViewSeedListDetailsBehaviour>();
        parent2 = transform.Find("FieldViewToggle/PlantingOptions/Selection/Selected2").GetComponent<FieldViewSeedListDetailsBehaviour>();
        plantButton = transform.Find("FieldViewToggle/PlantingOptions/Selection/PlantButton").GetComponent<Button>();


        harvestSeedAmount = transform.Find("FieldViewToggle/HarvestResult/Body/Seeds/Amount").GetComponent<Text>();
        harvestPlantAmount = transform.Find("FieldViewToggle/HarvestResult/Body/Plants/Amount").GetComponent<Text>();
        harvestSeedPic = transform.Find("FieldViewToggle/HarvestResult/Body/Seeds/Pic").GetComponent<Image>();
        harvestPlantPic = transform.Find("FieldViewToggle/HarvestResult/Body/Plants/Pic").GetComponent<Image>();
    }

    private void Update()
    {
        if (viewToggle.activeSelf)
        {
            CheckHarvestButton();
            CheckPlantingButton();
        }
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
        }
        else
        {
            currentInfo.SetActive(false);
            plantingOptions.SetActive(true);

        }
    }

    public void CheckHarvestButton()
    {
        if (currentlyViewedField != default && currentlyViewedField.IsFullyGrown())
        {
            if (harvestButton.interactable == false)
            {
                harvestButton.interactable = true;
            }
        }
        else
        {
            if (harvestButton.interactable == true)
            {
                harvestButton.interactable = false;
            }
        }
    }

    public void CheckPlantingButton()
    {
        if (currentlyViewedField != default && (parent1.Plant != default || parent2.Plant != default))
        {
            if (plantButton.interactable == false)
            {
                plantButton.interactable = true;
            }
        }
        else
        {
            if (plantButton.interactable == true)
            {
                plantButton.interactable = false;
            }
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
        parent1.DisplaySeedDetails(plantBehaviour.Plant);
    }

    public void SelectParent2(FieldViewSeedListDetailsBehaviour plantBehaviour)
    {
        parent2.DisplaySeedDetails(plantBehaviour.Plant);
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
        currentlyViewedField.PlantSeeds(parent1.Plant, parent2.Plant);
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


    public void ClearSelectedParent1()
    {
        parent1.ClearDisplayDetails();
    }

    public void ClearSelectedParent2()
    {
        parent2.ClearDisplayDetails();
    }

    public void ClearSelectedParents()
    {
        parent1.ClearDisplayDetails();
        parent2.ClearDisplayDetails();
    }
}
