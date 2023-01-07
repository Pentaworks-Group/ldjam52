using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Model;

using UnityEngine;

public class HarvestController
{

    public static HarvestProduce GetHarvestProduce(Plant plant)
    {
        HarvestProduce harvestProduce = new();
        harvestProduce.NumSeeds = GetNumSeeds(plant);
        harvestProduce.NumHarvest = GetNumHarvest(plant);

        return harvestProduce;
    }

    private static int GetNumSeeds(Plant plant)
    {
        return getFactorForType("Seeds", plant);
    }

    private static int GetNumHarvest(Plant plant)
    {
        return getFactorForType("Harvest", plant);
    }

    public static int getFactorForType(String type, Plant plant)
    {
        ChromosomePair pair;
        int factor = 1;
        if (plant.Genome.TryGetValue(type, out pair))
        {
            return (int)Math.Round(GrowthController.getDominantChromosome(pair).Value0);
        }
        else
        {
            Debug.LogError($"Key {type} not defined for the plant!");
        }
        return factor;
    }

}
