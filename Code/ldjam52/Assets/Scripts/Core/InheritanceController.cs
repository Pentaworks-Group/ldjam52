using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using UnityEngine;

public class InheritanceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void crossPlants(Plant plant1, Plant plant2)
    {

    }

    private Chromosome chooseRandomChromosome(ChromosomePair pair)
    {
        double randomNumber = Random.value;
        return randomNumber < 0.5 ? pair.Chromosome1 : pair.Chromosome2;
    }

    private void updateMeanValue(Chromosome chromomose)
    {
        Normal normal = new Normal();
        normal.Sample();
    }

}
