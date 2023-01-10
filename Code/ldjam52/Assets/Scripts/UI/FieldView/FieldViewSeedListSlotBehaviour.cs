
using Assets.Scripts.Model;

using GameFrame.Core.UI.List;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewSeedListSlotBehaviour : ListSlotBehaviour<StorageItem>
{
    private Image pic;
    private Text plantName;
    private Text seedAmount;

    public GameObject InformationPanel;

    public GameObject CurrentlyViewedField;

    //private void Awake()
    //{
    //    plantName = transform.Find("PlantName").GetComponent<Text>();
    //    seedAmount = transform.Find("SeedAmount").GetComponent<Text>();
    //    pic = transform.Find("Pic").GetComponent<Image>();
    //}

    override public void RudeAwake()
    {
        plantName = transform.Find("Info/PlantName").GetComponent<Text>();
        seedAmount = transform.Find("Info/SeedAmount").GetComponent<Text>();
        pic = transform.Find("Info/Pic").GetComponent<Image>();
    }

    public void SelectSlot()
    {

    }

    public Plant GetPlant()
    {
        return content.Plant;
    }

    public StorageItem GetStorageItem()
    {
        return content;
    }

    public void ShowItemDetails()
    {
        FieldViewBehaviour fieldView = CurrentlyViewedField.GetComponent<FieldViewBehaviour>();
        InformationPanel.GetComponent<InformationPrefabBehaviour>().UpdateInfo(content, fieldView.GetField(), false);
    }

    override public void UpdateUI()
    {
        StorageItem item = content;
        Plant plant = item.Plant;
        plantName.text = plant.Name;
        seedAmount.text = "Seeds in Storage: "+item.StorageAmountSeeds.ToString();
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.SeedImageName);
    }
}
