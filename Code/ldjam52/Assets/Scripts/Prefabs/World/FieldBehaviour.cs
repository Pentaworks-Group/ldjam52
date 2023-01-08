using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Core.Inventory;

public class FieldBehaviour : MonoBehaviour
{
    public TileViewBehaviour tileViewBehaviour;



    public Field Field { get; set; }


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
        //DebugPlant();
        if (Field == null)
        {
            Debug.LogWarning("No Field set, using default");
            Field = new Field
            {
                Humidity = 0.5,
                Temperature = 0.5,
                Fertility = 0.5,
                Sunshine = 0.5
            };
        }
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
        plant1.Genome.Add(ChromosomeTypes.WATER, pair1);
        plant1.Genome.Add(ChromosomeTypes.SUN, pair1);
        plant1.Genome.Add(ChromosomeTypes.TEMP, pair1);
        plant1.Genome.Add(ChromosomeTypes.FERTILITY, pair1);
        plant1.Genome.Add(ChromosomeTypes.GROWTH, pair1);

        PlantCrop(plant1);
    }

    void Update()
    {
        if (Field.Seed != null)
        {
            Field.GrowthProgress = Math.Min(1.0, Field.GrowthProgress + growthRate * Time.deltaTime);
            AdjustStadium();
        }
    }


    public void SelectField()
    {
        tileViewBehaviour.ViewField(this);
    }

    private void AdjustStadium()
    {
        foreach (GrowthStage stadium in GrowthStages.stages)
        {
            if (stadium.ProgressStart <= Field.GrowthProgress && stadium.ProgressEnd > Field.GrowthProgress && !stadium.Equals(currentStadium))
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
        Field.Seed = null;
        Field.TimePlanted = -1;
        currentStadium = null;
        Field.GrowthProgress = 0;

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

    public void PlantSeeds(Plant parent1, Plant parent2)
    {
        FarmStorageController.TakeSeedsOfStorage(parent1, 1);
        FarmStorageController.TakeSeedsOfStorage(parent2, 1);
        Plant child = InheritanceController.crossPlants(parent1, parent2);
        PlantCrop(child);
    }

    public void PlantCrop(Plant newPlant)
    {
        Field.Seed = newPlant;
        if (Assets.Scripts.Base.Core.Game.State != default)
        {
            Field.TimePlanted = Assets.Scripts.Base.Core.Game.State.ElapsedTime;
        } else
        {
            Field.TimePlanted = Time.realtimeSinceStartup;
        }
        currentStadium = GrowthStages.stages[0];
        growthRate = GrowthController.getGrowthRate(Field, Field.Seed);

        foreach (GameObject flowerPot in flowerPots)
        {
            int randomNumber = (int)(UnityEngine.Random.value * 360);
            flowerPot.transform.Rotate(new Vector3(0, randomNumber, 0));
        }
        Debug.Log("GrowthRate: " + growthRate);
        AdjustStadium();
    }

    public void HarvestCrop()
    {
        if (IsFullyGrown())
        {
            HarvestResult harvest = HarvestController.GetHarvestResult(Field.Seed);

            FarmStorageController.PutPlantInStorage(harvest.Plant, harvest.NumHarvest);
            FarmStorageController.PutSeedInStorage(harvest.Plant, harvest.NumSeeds);
            tileViewBehaviour.ShowHarvestResult(harvest);
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
