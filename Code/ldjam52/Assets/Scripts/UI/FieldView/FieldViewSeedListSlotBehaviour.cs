
using Assets.Scripts.Model;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewSeedListSlotBehaviour : ListSlotBehaviour
{
    private Image pic;
    private Text plantName;
    private Text seedAmount;

    public GameObject InformationPanel;

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
        return GetStorageItem().Plant;
    }

    public StorageItem GetStorageItem()
    {
        return (StorageItem)obj;
    }

    public void ShowItemDetails()
    {
        InformationPanel.GetComponent<InformationPrefabBehaviour>().UpdateInfo(GetStorageItem());
    }

    override public void UpdateUI()
    {
        StorageItem item = GetStorageItem();
        Plant plant = item.Plant;
        plantName.text = plant.Name;
        seedAmount.text = item.StorageAmountSeeds.ToString();
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.SeedImageName);
    }
}
