using System;
using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;
using Assets.Scripts.Scene.World;

using GameFrame.Core.Extensions;

using UnityEngine;

public class FieldBehaviour : MonoBehaviour, IUpdateME
{
    private GameObject dirtPatch;
    private TileBehaviour parentTile;

    private GrowthStage currentStadium = null;
    private readonly List<GameObject> flowerPots = new List<GameObject>();

    private Double? growthRate;

    private Field field;
    private Boolean isNotified;

    public Field Field => this.field;

    public void SetField(Field field)
    {
        this.field = field;

        if (field != default)
        {
            if (parentTile.Tile.IsOwned)
            {
                ActivateField();
            }
        }
    }

    private void Awake()
    {
        dirtPatch = transform.Find("DirtPatch").gameObject;

        parentTile = GetComponentInParent<TileBehaviour>();

        Transform PotsParent = transform.Find("FlowerPots");

        foreach (Transform child in PotsParent)
        {
            flowerPots.Add(child.gameObject);
        }

        UpdateManager.RegisterBehaviour(this);


    }

    private void Start()
    {
        if (parentTile.Tile.IsOwned)
        {
            this.dirtPatch.SetActive(true);
        }
    }

    public void UpdateME()
    {
        //if (this.field != default)
        //{


        var isPlanted = this.field.Seed != null;

        if (isPlanted)
        {
            if (!growthRate.HasValue)
            {
                growthRate = GrowthController.getGrowthRate(this.field, this.field.Seed);
            }

            this.field.GrowthProgress = Math.Min(1.0, this.field.GrowthProgress + growthRate.Value * Time.deltaTime);
            AdjustStadium();

            if (IsFullyGrown() && !this.isNotified)
            {
                this.isNotified = true;
                Core.Game.EffectsAudioManager.PlayAt("Bell", parentTile.Tile.Position.ToUnity());
            }
        }
        //}
    }


    public void ActivateField()
    {
        this.dirtPatch.SetActive(true);
        this.GetComponent<FieldBehaviour>().enabled = true;
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

            if (newModelName == "Flower4")
            {
                GameObject blossom = flower.gameObject.transform.Find("Sphere").gameObject;
                //TODO: Renderer is null?
                if (blossom.TryGetComponent<Renderer>(out var meshRenderer))
                {
                    Material[] mats = meshRenderer.materials;
                    if (mats[0] != null)
                    {
                        meshRenderer.material.color = field.Seed.BlossomColor.ToUnity();
                        meshRenderer.material.EnableKeyword("_EMISSION");
                        meshRenderer.material.SetColor("_EmissionColor", field.Seed.BlossomColor.ToUnity());
                        //                        meshRenderer.material.
                    }
                }
            }
        }
    }

    private void ClearField()
    {
        this.field.Seed = null;
        this.field.TimePlanted = -1;
        currentStadium = null;
        this.field.GrowthProgress = 0;
        isNotified = false;

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
            if (parent1.ID == parent2.ID)
            {
                if (FarmStorageController.GetSeedCountInStorage(parent1) >= 2)
                {
                    FarmStorageController.TakeSeedsOfStorage(parent1, 2);
                    Plant child = InheritanceController.crossPlants(parent1, parent2);
                    PlantCrop(child);
                }

            }
            else if (FarmStorageController.GetSeedCountInStorage(parent1) >= 1 && FarmStorageController.GetSeedCountInStorage(parent2) >= 1)
            {
                FarmStorageController.TakeSeedsOfStorage(parent1, 1);
                FarmStorageController.TakeSeedsOfStorage(parent2, 1);
                Plant child = InheritanceController.crossPlants(parent1, parent2);
                PlantCrop(child);
            }
        }
        else if (parent1 != default && FarmStorageController.GetSeedCountInStorage(parent1) >= 1)
        {
            FarmStorageController.TakeSeedsOfStorage(parent1, 1);
            PlantCrop(parent1);
        }
        else if (parent2 != default && FarmStorageController.GetSeedCountInStorage(parent2) >= 1)
        {
            FarmStorageController.TakeSeedsOfStorage(parent2, 1);
            PlantCrop(parent2);
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
            flowerPot.transform.Rotate(new UnityEngine.Vector3(0, randomNumber, 0));
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
