using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    public TileViewBehaviour tileViewBehaviour;

    public Plant Plant { get; set; }
    public float TimePlanted { get; private set; } = -1;


    private float nextStadium = 2;
    private int currentStadium = -1;
    private int ripe = 4;
    private List<GameObject> flowerPots = new List<GameObject>();

    private float currentGameTime = 0;

    private void Awake()
    {
        Transform PotsParent = transform.Find("FlowerPots");
        foreach (Transform child in PotsParent)
        {
            flowerPots.Add(child.gameObject);
        }
    }


    void Update()
    {
        CheckIfNextStadium();
        currentGameTime += Time.deltaTime;
    }


    public void SelectField()
    {
        tileViewBehaviour.ViewField(this);
    }

    private void CheckIfNextStadium()
    {
        if (TimePlanted >= 0 && currentStadium < ripe && nextStadium < currentGameTime)
        {
            currentStadium += 1;
            GrowToStadium(currentStadium);           
        }
    }

    private void GrowToStadium(int state) 
    {
        string nextModelName = GetNextModelName(state);
        ReplacePlantModel(nextModelName);
        nextStadium = GetNextGrowthTick();
    }

    private float GetNextGrowthTick()
    {
        return nextStadium + 2;
    }

    private string GetNextModelName(int state)
    {
        return "Flower" + state;
    }

    private void ReplacePlantModel(string newModelName)
    {
        foreach (GameObject flowerPot in flowerPots)
        {
            foreach (Transform child in flowerPot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Transform template = transform.Find("Flowers/" + newModelName);
            Transform flower = Instantiate(template, flowerPot.transform);
            flower.gameObject.name = "Flower";
            flower.gameObject.SetActive(true);
        }
    }

    private void ClearField()
    {
        TimePlanted = -1;
        currentStadium = -1;
        foreach (GameObject flowerPot in flowerPots)
        {
            foreach (Transform child in flowerPot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public bool IsFullyGrown()
    {
        return currentStadium == ripe;
    }

    public void HarvestCrop()
    {
        if (IsFullyGrown())
        {
            ClearField();
        }
    }


    public void FertilizeCrop()
    {
        GrowToStadium(ripe);
    }

    public void DestroyCrop()
    {
        ClearField();
    }
}
