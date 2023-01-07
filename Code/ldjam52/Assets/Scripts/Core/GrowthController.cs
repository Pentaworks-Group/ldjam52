using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using UnityEngine;
using GameFrame.Core.Extensions;

public class GrowthController : MonoBehaviour
{
    void Start()
    {
        
    }

    private void Update()
    {

    }

    public Double getGrowthRate(Field field, Plant plant)
    {
        //Humidity
        Double humidityFactor = getGrowthFactorForType(ChromosomeTypes.WATER, plant, field.Humidity);
        //Temperature
        Double temperatureFactor = getGrowthFactorForType(ChromosomeTypes.WATER, plant, field.Humidity);
        //Sunshine
        Double sunshineFactor = getGrowthFactorForType(ChromosomeTypes.WATER, plant, field.Humidity);
        //Fertility
        Double fertilityFactor = getGrowthFactorForType(ChromosomeTypes.WATER, plant, field.Humidity);

        Double growthFactor = (humidityFactor + temperatureFactor + sunshineFactor + fertilityFactor) / 4;

        Double growthRate = 0.0;
        ChromosomePair growthPair;
        if (plant.genome.TryGetValue(ChromosomeTypes.GROWTH, out growthPair))
        {
            Chromosome chromosome = getDominantChromosome(growthPair);
            growthRate = chromosome.Value0;
        }
        else
        {
            Debug.LogError($"Key {ChromosomeTypes.GROWTH} not defined for the plant!");
        }

        return growthFactor * growthRate;
    }

    private Double getGrowthFactorForType(String type, Plant plant, Double biomeValue)
    {
        ChromosomePair pair;
        Double factor = 1.0;
        if (plant.genome.TryGetValue(type, out pair))
        {
            return getGrowthForChromosome(getDominantChromosome(pair), biomeValue);
        }
        else
        {
            Debug.LogError($"Key {type} not defined for the plant!");
        }
        return factor;
    }

    private Chromosome getDominantChromosome(ChromosomePair pair)
    {
        Chromosome outValue = new Chromosome
        {
            Value0 = (pair.Chromosome1.Value0 + pair.Chromosome2.Value0)/ 2.0,
            ValueDev = (pair.Chromosome1.ValueDev + pair.Chromosome2.ValueDev) / 2.0,
            IsDominant = pair.Chromosome1.IsDominant
        };
        if (!pair.Chromosome1.IsDominant && pair.Chromosome2.IsDominant)
        {
            outValue = pair.Chromosome2;
        }
        else if (pair.Chromosome1.IsDominant && !pair.Chromosome2.IsDominant)
        {
            outValue = pair.Chromosome1;
        }
        return outValue;
    }

    private Double getGrowthForChromosome(Chromosome chromosome, Double biomeValue)
    {
        Normal normal = new Normal(mean: chromosome.Value0, stddev: chromosome.ValueDev);
        double newMean = normal.Density(biomeValue);

        return newMean;
    }
}
