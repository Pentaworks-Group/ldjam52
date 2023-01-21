using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using UnityEngine;
using GameFrame.Core.Extensions;

public class GrowthController
{
    public static Double getGrowthRate(Field field, Plant plant)
    {
        //Scaling Factor
        double scalingFactor =  0.1;
        //Humidity
        Double humidityFactor = getGrowthFactorForType(ChromosomeTypes.WATER, plant, field.Humidity);
        //Temperature
        Double temperatureFactor = getGrowthFactorForType(ChromosomeTypes.TEMP, plant, field.Temperature);
        //Sunshine
        Double sunshineFactor = getGrowthFactorForType(ChromosomeTypes.SUN, plant, field.Sunshine);
        //Fertility
        Double fertilityFactor = getGrowthFactorForType(ChromosomeTypes.FERTILITY, plant, field.Fertility);

        //        Double growthFactor = (humidityFactor + temperatureFactor + sunshineFactor + fertilityFactor) / 4;
        double growthFactor = humidityFactor * temperatureFactor * sunshineFactor * fertilityFactor;

        Double growthRate = 0.0;
        if (plant.Genome.TryGetValue(ChromosomeTypes.GROWTH, out ChromosomePair growthPair))
        {
            Chromosome chromosome = getDominantChromosome(growthPair);
            growthRate = chromosome.Value0;
        }
        else
        {
            Debug.LogError($"Key {ChromosomeTypes.GROWTH} not defined for the plant!");
        }

        return growthFactor * growthRate * scalingFactor;
    }

    public static Double getGrowthFactorForType(String type, Plant plant, Double biomeValue)
    {
        ChromosomePair pair;
        Double factor = 1.0;
        if (plant.Genome.TryGetValue(type, out pair))
        {
            return getGrowthForChromosome(getDominantChromosome(pair), biomeValue);
        }
        else
        {
            Debug.LogError($"Key {type} not defined for the plant!");
        }
        return factor;
    }

    public static Chromosome getDominantChromosome(ChromosomePair pair)
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

    private static Double getGrowthForChromosome(Chromosome chromosome, Double biomeValue)
    {
        Normal normal = new Normal(mean: chromosome.Value0, stddev: chromosome.ValueDev);
        double newMean = normal.Density(biomeValue) / normal.Density(chromosome.Value0) ;

        return newMean;
    }
}
