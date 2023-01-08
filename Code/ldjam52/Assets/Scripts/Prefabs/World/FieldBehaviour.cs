using System;
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;

using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    private GameObject dirtPatch;
    private GrowthStage currentStadium = null;
    private readonly List<GameObject> flowerPots = new List<GameObject>();

    private Double growthRate = 0;

    private Field field;
    public Field Field => this.field;

    public void SetField(Field field)
    {
        this.field = field;

        if (field != default)
        {
            if (GetComponentInParent<TileBehaviour>().Tile.IsOwned)
            {
                this.dirtPatch.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        dirtPatch = transform.Find("DirtPatch").gameObject;

        Transform PotsParent = transform.Find("FlowerPots");

        foreach (Transform child in PotsParent)
        {
            flowerPots.Add(child.gameObject);
        }
    }

    private void Start()
    {
    }

    void Update()
    {
        if (this.field != default)
        {
            if (!this.dirtPatch.activeSelf)
            {
                if (GetComponentInParent<TileBehaviour>().Tile.IsOwned)
                {
                    this.dirtPatch.SetActive(true);
                }
            }

            var isPlanted = this.field.Seed != null;

            if (isPlanted)
            {
                this.field.GrowthProgress = Math.Min(1.0, this.field.GrowthProgress + growthRate * Time.deltaTime);
                AdjustStadium();
            }
        }
    }

    private void AdjustStadium()
    {
        foreach (GrowthStage stadium in GrowthStages.stages)
        {
            if (stadium.ProgressStart <= this.field.GrowthProgress && stadium.ProgressEnd > this.field.GrowthProgress && !stadium.Equals(currentStadium))
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
        this.field.Seed = null;
        this.field.TimePlanted = -1;
        currentStadium = null;
        this.field.GrowthProgress = 0;

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
        return this.field.GrowthProgress >= 1.0;
    }

    public void PlantSeeds(Plant parent1, Plant parent2)
    {
        if (parent1 != default && parent2 != default)
        {
            FarmStorageController.TakeSeedsOfStorage(parent1, 1);
            FarmStorageController.TakeSeedsOfStorage(parent2, 1);
            Plant child = InheritanceController.crossPlants(parent1, parent2);
            PlantCrop(child);
        }
        else if (parent1 != default || parent2 != default)
        {
            if (parent1 != default)
            {
                PlantCrop(parent1);
            }
            if (parent2 != default)
            {
                PlantCrop(parent2);
            }
        }

    }

    public void PlantCrop(Plant newPlant)
    {
        this.field.Seed = newPlant;
        if (Assets.Scripts.Base.Core.Game.State != default)
        {
            this.field.TimePlanted = Assets.Scripts.Base.Core.Game.State.ElapsedTime;
        }
        else
        {
            this.field.TimePlanted = Time.realtimeSinceStartup;
        }

        currentStadium = GrowthStages.stages[0];
        growthRate = GrowthController.getGrowthRate(this.field, this.field.Seed);

        foreach (GameObject flowerPot in flowerPots)
        {
            int randomNumber = (int)(UnityEngine.Random.value * 360);
            flowerPot.transform.Rotate(new Vector3(0, randomNumber, 0));
        }

        Debug.Log("GrowthRate: " + growthRate);
        AdjustStadium();
    }

    public HarvestResult HarvestCrop()
    {
        if (IsFullyGrown())
        {
            HarvestResult harvest = HarvestController.GetHarvestResult(this.field.Seed);

            FarmStorageController.PutPlantInStorage(harvest.Plant, harvest.NumHarvest);
            FarmStorageController.PutSeedInStorage(harvest.Plant, harvest.NumSeeds);

            ClearField();

            return harvest;
        }

        return default;
    }

    public void FertilizeCrop()
    {
        this.field.GrowthProgress = 1;
    }

    public void DestroyCrop()
    {
        ClearField();
    }
}
