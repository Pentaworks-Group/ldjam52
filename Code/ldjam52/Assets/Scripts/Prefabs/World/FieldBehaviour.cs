using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    public TileViewBehaviour tileViewBehaviour;

    public Field Field { get; set; }
    public Plant Plant { get; set; }

    public float TimePlanted { get; private set; } = -1;

    private GrowthStage currentStadium = null;
    private List<GameObject> flowerPots = new List<GameObject>();

    private Double growthRate = 0;

    private void Awake()
    {
        Transform PotsParent = transform.Find("FlowerPots");
        foreach (Transform child in PotsParent)
        {
            flowerPots.Add(child.gameObject);
        }
    }

    private void Start()
    {
        DebugPlant()
    }

    private void DebugPlant()
    {
        Field = new Field
        {
            Humidity = 0.5,
            Temperature = 0.5,
            Fertility = 0.5,
            Sunshine = 0.5
        };

        //Debug: Test code
        Chromosome ch1 = new Chromosome
        {
            Value0 = 0.1,
            ValueDev = 0.2,
            IsDominant = true
        };
        Chromosome ch2 = new Chromosome
        {
            Value0 = 0.2,
            ValueDev = 0.4
        };
        ChromosomePair pair1 = new ChromosomePair
        {
            Chromosome1 = ch1,
            Chromosome2 = ch2
        };
        Plant plant1 = new Plant
        {
            Name = "Test 1",
            Description = "This is the first plant"
        };
        plant1.genome.Add(ChromosomeTypes.WATER, pair1);
        plant1.genome.Add(ChromosomeTypes.TEMP, pair1);
        plant1.genome.Add(ChromosomeTypes.SUN, pair1);
        plant1.genome.Add(ChromosomeTypes.FERTILITY, pair1);
        plant1.genome.Add(ChromosomeTypes.GROWTH, pair1);

        PlantCrop(plant1);
    }

    void Update()
    {
        if (Plant != null)
            Field.GrowthProgress = Math.Min(1.0, Field.GrowthProgress + growthRate * Time.deltaTime);
        Debug.Log(Field.GrowthProgress);
        AdjustStadium();
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
                !stadium.Equals(currentStadium))
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

    public void PlantCrop(Plant newPlant)
    {
        Plant = newPlant;
        TimePlanted = Time.realtimeSinceStartup;
        currentStadium = GrowthStages.stages[0];
        growthRate = GrowthController.getGrowthRate(Field, Plant);
        Debug.Log("GrothRate: " + growthRate);
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
