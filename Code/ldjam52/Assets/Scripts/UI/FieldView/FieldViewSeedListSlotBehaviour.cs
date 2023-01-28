
using Assets.Scripts.Base;
using Assets.Scripts.Model;

using GameFrame.Core.UI.List;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewSeedListSlotBehaviour : ListSlotBehaviour<StorageItem>
{
    public TMP_InputField RenameField;
    private Image pic;
    public Text PlantName;
    private Text seedAmount;

    public GameObject InformationPanel;

    public GameObject CurrentlyViewedField;

    public GameObject RenamePanel;

    [SerializeField]
    private GameObject FavoriteButtonFill;

    //private void Awake()
    //{
    //    plantName = transform.Find("PlantName").GetComponent<Text>();
    //    seedAmount = transform.Find("SeedAmount").GetComponent<Text>();
    //    pic = transform.Find("Pic").GetComponent<Image>();
    //}

    override public void RudeAwake()
    {
//        plantName = transform.Find("Info/PlantName").GetComponent<Text>();
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
        FavoriteButtonFill.SetActive(GetPlant().Favorit);
    }

    public void ShowRenameField()
    {
        if (RenamePanel != null && PlantName != null)
        {
            RenamePanel.gameObject.SetActive(true);
            PlantName.gameObject.SetActive(false);
            RenameField.text = content.Plant.Name;
        }
    }

    public void HideRenameField()
    {
        if (RenamePanel != null && PlantName != null)
        {
            RenamePanel.gameObject.SetActive(false);
            PlantName.gameObject.SetActive(true);
        }
    }


    public void RenamePlant()
    {
        if (RenameField.text != null && RenameField.text.Length > 0)
        {
            content.Plant.Name = RenameField.text;
//            Core.Game.State.KnownPlants[content.Plant.ID].Name = content.Plant.Name;
            HideRenameField();
            UpdateUI();
        }
    }

    override public void UpdateUI()
    {
        StorageItem item = content;
        Plant plant = item.Plant;
        PlantName.text = plant.Name;
        seedAmount.text = "Seeds in Storage: "+item.StorageAmountSeeds.ToString();
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.SeedImageName);
    }
}
