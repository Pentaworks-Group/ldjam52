
using Assets.Scripts.Model;

using UnityEngine.UI;

public class TileViewSeedListSlotBehaviour : ListSlotBehaviour
{
    private Image pic;
    private Text plantName;
    private Text seedAmount;

    private void Awake()
    {
        plantName = transform.Find("PlantName").GetComponent<Text>();
        seedAmount = transform.Find("SeedAmount").GetComponent<Text>();
        pic = transform.Find("Pic").GetComponent<Image>();
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

    override public void UpdateUI()
    {
        StorageItem item = GetStorageItem();
        Plant plant = item.Plant;
        plantName.text = plant.Name;
        seedAmount.text = item.StorageAmountSeeds.ToString();
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.SeedImageName);
    }
}
