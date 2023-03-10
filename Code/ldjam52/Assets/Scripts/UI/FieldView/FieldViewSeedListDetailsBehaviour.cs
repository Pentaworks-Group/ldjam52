using Assets.Scripts.Base;
using Assets.Scripts.Model;

using UnityEngine;
using UnityEngine.UI;

public class FieldViewSeedListDetailsBehaviour : MonoBehaviour
{
    private Image pic;
    private Text plantName;
    public Plant Plant { get; private set; }

    public GameObject InformationPanel;

    private void Awake()
    {
        pic = transform.Find("NameAndPic/Pic").GetComponent<Image>();
        plantName = transform.Find("NameAndPic/Name").GetComponent<Text>();
    }

    private void Start()
    {
        ClearDisplayDetails();
    }

    public void DisplaySlot(FieldViewSeedListSlotBehaviour slot)
    {
        DisplaySeedDetails(slot.GetPlant());
//        Core.Game.PlayButtonSound();
    }

    public void DisplaySeedDetails(Plant plant)
    {

        this.Plant = plant;
        plantName.text = plant.Name;
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.ImageName);
    }

    public void ClearDisplayDetails()
    {
        this.Plant = default;
        plantName.text = "";
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get("Invisible_placeholder");
    }


}
