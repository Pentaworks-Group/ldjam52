using Assets.Scripts.Model;
using Assets.Scripts.Constants;
using Assets.Scripts.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using UnityEngine;
using GameFrame.Core.Extensions;

using GameFrame.Core.Media;

public class InheritanceController 
{
    // Start is called before the first frame update
    void Start()
    {
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
        Chromosome ch3 = new Chromosome
        {
            Value0 = 0.3,
            ValueDev = 0.2,
            IsDominant = true
        };
        Chromosome ch4 = new Chromosome
        {
            Value0 = 0.9,
            ValueDev = 0.05
        };
        ChromosomePair pair2 = new ChromosomePair
        {
            Chromosome1 = ch3,
            Chromosome2 = ch4
        };
        Plant plant1 = new Plant
        {
            Name = "Test 1",
            Description = "This is the first plant"
        };
        plant1.Genome.Add("Temp", pair1);
        Plant plant2 = new Plant
        {
            Name = "Test 2",
            Description = "This is the second plant"
        };
        plant2.Genome.Add("Temp", pair2);

        Plant plant3 = crossPlants(plant1, plant2);
        Debug.Log(plant3.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Cross two Plants to create a new one. The new genome is generated randomly based on the parents.
    public static Plant crossPlants(Plant plant1, Plant plant2)
    {
        Plant newPlant = new Plant();
        newPlant.Name = PlantNames.adjectives.GetRandomEntry()+" "+PlantNames.names.GetRandomEntry();
        newPlant.BlossomColor = new GameFrame.Core.Media.Color
        {
            Red = UnityEngine.Random.value,
            Green = UnityEngine.Random.value,
            Blue = UnityEngine.Random.value,
            Alpha = 1
        };
        foreach (KeyValuePair<string, ChromosomePair> pair in plant1.Genome)
        {
            Chromosome chromosome1 = chooseRandomChromosome(pair.Value);
            Chromosome chromosome2 = chooseRandomChromosomeByType(plant2, pair.Key);
            ChromosomePair newPair = new ChromosomePair
            {
                Chromosome1 = chromosome1,
                Chromosome2 = chromosome2
            };
            newPlant.Genome.Add(pair.Key, newPair);
        }
        AddImages(plant1, plant2, newPlant);
        Plant existingPlant = checkIfGenomeExists(newPlant);
        if (existingPlant != null)
        {
            newPlant = existingPlant;
        }
        else
        {
            newPlant.ID = Guid.NewGuid();
        }
        return newPlant;
    }

    public static List<Chromosome> AnalysePlant(Plant plant, Analyzer analyzer)
    {
        List<Chromosome > chromosomes = new List<Chromosome>();
        int newVisibleChromosomes = 0;

        List<string> keys = new List<String>();
        keys.AddRange(plant.Genome.Keys);

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            string element = keys[i];
            if (element == "Durability" ||
                element == "Wither" ||
                element == "Growth" ||
                element == "PlantValue" ||
                element == "SeedsValue" ||
                plant.Genome.GetValueOrDefault(element).IsVisible)
            {
                keys.RemoveAt(i);
            }
        }

        while (newVisibleChromosomes < analyzer.CurrentDevelopmentStage.ValueVisibleCount && keys.Count != 0)
        {
            int rand = UnityEngine.Random.Range(0, keys.Count);
            var randomKey = keys[rand];
            ChromosomePair pair = plant.Genome.GetValueOrDefault(randomKey);

            if (!pair.IsVisible)
            {
                newVisibleChromosomes = newVisibleChromosomes + 1;
                pair.IsVisible = true;
                chromosomes.Add(GrowthController.getDominantChromosome(pair));

                keys.RemoveAt(rand);
            }
        }

        return chromosomes;
    }

    public static void AnalyseField(Field field, Analyzer analyzer)
    {
        int newVisibleProperties = 0;
        int attempts = 0;
        int propertiesCount = 4;
        while (newVisibleProperties < analyzer.CurrentDevelopmentStage.ValueVisibleCount)
        {
            int randomPropertyID = (int) Math.Round(UnityEngine.Random.value * propertiesCount);
            
            //TODO: Unsch�n, ich weiss;-)
            if (randomPropertyID==FieldProperties.PROP_HUMIDITY && !field.IsHumidityVisible)
            {
                newVisibleProperties = newVisibleProperties + 1;
                field.IsHumidityVisible = true;
            }
            else if (randomPropertyID == FieldProperties.PROP_TEMPERATURE && !field.IsTemperatureVisible)
            {
                newVisibleProperties = newVisibleProperties + 1;
                field.IsTemperatureVisible = true;
            }
            else if (randomPropertyID == FieldProperties.PROP_SUNSHINE && !field.IsSunshineVisible)
            {
                newVisibleProperties = newVisibleProperties + 1;
                field.IsSunshineVisible = true;
            }
            else if (randomPropertyID == FieldProperties.PROP_FERTILITY && !field.IsFertiliyVisible)
            {
                newVisibleProperties = newVisibleProperties + 1;
                field.IsFertiliyVisible = true;
            }

            attempts++;
            if (attempts >= propertiesCount)
                break;
        }
    }


    public static Plant checkIfGenomeExists(Plant plant)
    {
        foreach (KeyValuePair<Guid, Plant> pair in Core.Game.State.KnownPlants)
        {
            bool equal = true;
            foreach (KeyValuePair<string, ChromosomePair> chromosomePair in pair.Value.Genome)
            {
                ChromosomePair otherPair;
                if (plant.Genome.TryGetValue(chromosomePair.Key, out otherPair))
                {
                    equal &= chromosomePair.Equals(otherPair);
                    if (!equal)
                    {
                        break;
                    }
                }
                else
                {
                    equal = false;
                }

            }
            if (equal)
            {
                return pair.Value;
            }
        }
        return null;
    }

    private static void AddImages(Plant plant1, Plant plant2, Plant newPlant)
    {
        if (UnityEngine.Random.value < 0.5)
        {
            newPlant.ImageName = plant1.ImageName;
            newPlant.SeedImageName = plant1.SeedImageName;
        } else
        {
            newPlant.ImageName = plant2.ImageName;
            newPlant.SeedImageName = plant2.SeedImageName;
        }

    }

    private static Chromosome chooseRandomChromosomeByType(Plant plant, String type)
    {
        ChromosomePair chromosomePair;
        if (plant.Genome.TryGetValue(type, out chromosomePair))
        {
            return chooseRandomChromosome(chromosomePair);
        } else
        {
            Debug.LogError($"Key {type} not defined for the plant!");
            return null;
        }
    }

    private static Chromosome chooseRandomChromosome(ChromosomePair pair)
    {
        double randomNumber = UnityEngine.Random.value;
        return randomNumber < 0.5 ? pair.Chromosome1 : pair.Chromosome2;
    }

    private void updateMeanValue(Chromosome chromosome)
    {
        Normal normal = new Normal(mean: chromosome.Value0, stddev: chromosome.ValueDev);
        double newMean = normal.Sample();

        //TODO: What to to with the Deviation? => Right now just keep it the same
        chromosome.Value0 = newMean;
    }

}
