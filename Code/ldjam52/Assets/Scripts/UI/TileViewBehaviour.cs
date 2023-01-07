using UnityEngine.UI;
using UnityEngine;

public class TileViewBehaviour : MonoBehaviour
{
    private FieldBehaviour currentlyViewedField;
    private GameObject viewToggle;
    private GameObject currentInfo;
    private Image plantImage;
    private Text plantName;
    private Text plantedTime;
    private Button harvestButton;

    private void Awake()
    {
        viewToggle = transform.Find("TileViewToggle")?.gameObject;
        currentInfo = transform.Find("TileViewToggle/CurrentInfo")?.gameObject;
        plantImage = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Image")?.GetComponent<Image>();
        plantName = transform.Find("TileViewToggle/CurrentInfo/CurrentPlant/Name")?.GetComponent<Text>();
        plantedTime = transform.Find("TileViewToggle/CurrentInfo/PlantedTime/Text")?.GetComponent<Text>();
        harvestButton = transform.Find("TileViewToggle/CurrentInfo/Buttons/Harvest")?.GetComponent<Button>();
    }

    public void ViewField(FieldBehaviour fieldBehaviour)
    {
        currentlyViewedField = fieldBehaviour;
        viewToggle.SetActive(true);
        UpdateView();
    }

    private void UpdateView()
    {
        if (currentlyViewedField.Plant != null)
        {
            currentInfo.SetActive(true);
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

        }
    }

    public void HarvestCrop()
    {
        currentlyViewedField.HarvestCrop();
    }


    public void FertilizeCrop()
    {
        currentlyViewedField.FertilizeCrop();
    }

    public void DestroyCrop()
    {
        currentlyViewedField.DestroyCrop();
    }


    public void CloseView()
    {
        currentlyViewedField = null;
        viewToggle.SetActive(false);
    }


}
