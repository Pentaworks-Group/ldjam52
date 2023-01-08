using Assets.Scripts.Model;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewSeedListDetailsBehaviour : MonoBehaviour
{
    private Image pic;
    private Text plantName;
    private Plant plant;


    private void Awake()
    {
        pic = transform.Find("NameAndPic/Pic").GetComponent<Image>();
        plantName = transform.Find("NameAndPic/Name").GetComponent<Text>();
    }

    public void DisplaySlot(FieldViewSeedListSlotBehaviour slot)
    {
        DisplaySeedDetails(slot.GetPlant());
    }

    public void DisplaySeedDetails(Plant plant)
    {
        this.plant = plant;
        plantName.text = plant.Name;
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.ImageName);
    }

    public Plant GetPlant()
    {
        return plant;
    }
}
