
using Assets.Scripts.Model;

using UnityEngine.UI;

public class TileViewSeedListSlotBehaviour : ListSlotBehaviour
{
    private Text slotText;
    private Image pic;

    private void Awake()
    {
        slotText = transform.Find("Text").GetComponent<Text>();
        pic = transform.Find("Pic").GetComponent<Image>();
    }

    public void SelectSlot()
    {

    }

    public Plant GetPlant()
    {
        return (Plant)obj;
    }

    override public void UpdateUI()
    {
        Plant plant = GetPlant();
        slotText.text = plant.Name;
        pic.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(plant?.ImageName);
    }
}
