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
    private Image plantImage;
    private Text plantName;
    private Text plantedTime;
    private Button harvestButton;


    private TileViewSeedListDetailsBehaviour parent1;
    private TileViewSeedListDetailsBehaviour parent2;

    private void Awake()
    {
        viewToggle = transform.Find("TileViewToggle")?.gameObject;
        currentInfo = transform.Find("TileViewToggle/CurrentInfo")?.gameObject;
        plantingOptions = transform.Find("TileViewToggle/PlantingOptions")?.gameObject;
        plantImage = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Image")?.GetComponent<Image>();
        plantName = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Name")?.GetComponent<Text>();
        plantedTime = transform.Find("TileViewToggle/CurrentInfo/PlantedTime/Value")?.GetComponent<Text>();
        harvestButton = transform.Find("TileViewToggle/CurrentInfo/Buttons/Harvest")?.GetComponent<Button>();
        parent1 = transform.Find("TileViewToggle/PlantingOptions/Selection/Selected1")?.GetComponent<TileViewSeedListDetailsBehaviour>();
        parent2 = transform.Find("TileViewToggle/PlantingOptions/Selection/Selected2")?.GetComponent<TileViewSeedListDetailsBehaviour>();
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
        if (currentlyViewedField.Plant != null)
        {
            currentInfo.SetActive(true);
            plantingOptions.SetActive(false);
            plantName.text = currentlyViewedField.Plant.Name;
            plantedTime.text = currentlyViewedField.TimePlanted.ToString();
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
        viewToggle.SetActive(false);
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
}
