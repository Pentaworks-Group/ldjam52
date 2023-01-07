using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    public TileViewBehaviour tileViewBehaviour;
    public GrowthController GrowthController;

    public Field Field { get; set; }
    public Plant Plant { get; set; }

    public float TimePlanted { get; private set; } = -1;

    private GrowthStage currentStadium = null;
    private List<GameObject> flowerPots = new List<GameObject>();

    private float currentGameTime = 0;
    private Double growthRate = 0;

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
        if (Plant != null)
            Field.GrowthProgress = Math.Min(1.0, Field.GrowthProgress + growthRate * Time.deltaTime);
        AdjustStadium();
        currentGameTime += Time.deltaTime;
    }


    public void SelectField()
    {
        tileViewBehaviour.ViewField(this);
    }

    private void AdjustStadium()
    {
        foreach (GrowthStage stadium in GrowthStages.stages)
        {
            if (stadium.ProgressStart<=Field.GrowthProgress && stadium.ProgressEnd>Field.GrowthProgress &&
                !currentStadium.Equals(stadium))
            {
                currentStadium = stadium;
                ReplacePlantModel(stadium.ModelName);
            }
        }
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
        currentStadium = null;
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
        return Field.GrowthProgress >= 1.0;
    }

    public void plantCrop(Plant newPlant)
    {
        Plant = newPlant;
        TimePlanted = Time.realtimeSinceStartup;
        currentStadium = GrowthStages.stages[0];
        growthRate = GrowthController.getGrowthRate(Field, Plant);
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
        Field.GrowthProgress = 1;
    }

    public void DestroyCrop()
    {
        ClearField();
    }
}
