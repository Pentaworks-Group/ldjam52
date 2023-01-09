using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Model;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class InformationPrefabBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateInfo(StorageItem item)
    {

        Transform information = isSell ? PlantInfo.transform : SeedInfo.transform;

        // Always shown
        information.Find("Name").GetComponent<TMP_Text>().text = item.Plant.Name;
        information.Find("Amount").GetComponent<TMP_Text>().text = "Amount: " + (isSell ? item.StorageAmountPlants : item.StorageAmountSeeds).ToString();
        information.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        information.Find("Image").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(item.Plant.ImageName);

        ChromosomePair pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDSVALUE];
        information.Find("SeedsValue").GetComponent<TMP_Text>().text = "Seed value: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();

        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.PLANTVALUE];
        information.Find("PlantsValue").GetComponent<TMP_Text>().text = "Plant value: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();

        // Visibility checks needed
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SEEDS];
        if (pair.IsVisible)
            information.Find("Seeds").GetComponent<TMP_Text>().text = "Seeds: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();
        else
            information.Find("Seeds").GetComponent<TMP_Text>().text = "Seeds: ?";

        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.HARVEST];
        if (pair.IsVisible)
            information.Find("Harvest").GetComponent<TMP_Text>().text = "Harvest: " + ((int)GrowthController.getDominantChromosome(pair).Value0).ToString();
        else
            information.Find("Harvest").GetComponent<TMP_Text>().text = "Harvest: ?";

        // Stat bars
        // TODO put into StatsBar
        drawStatsBar(information, "Temp", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Sun", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Water", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);
        drawStatsBar(information, "Fertility", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP]);

        /*
        StatsBar bar = information.Find("Temp").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Sun").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SUN];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Water").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.WATER];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }

        bar = information.Find("Fertility").GetChild(0).GetComponent<StatsBar>();
        pair = item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.FERTILITY];
        if (pair.IsVisible || true)
        {
            bar.QuestionMark.SetActive(false);
            bar.GradientTransform.gameObject.SetActive(true);
            bar.BiomeTransform.gameObject.SetActive(true);
            bar.Mean = GrowthController.getDominantChromosome(pair).Value0;
            bar.Width = GrowthController.getDominantChromosome(pair).ValueDev;
        }
        else
        {
            bar.GradientTransform.gameObject.SetActive(false);
            bar.BiomeTransform.gameObject.SetActive(false);
            bar.QuestionMark.SetActive(true);
        }
        */
    }

    private void drawStatsBar(Transform infoPanel, string barName, ChromosomePair pair)
    {
        StatsBar bar = infoPanel.Find(barName).GetChild(0).GetComponent<StatsBar>();

        if (pair.IsVisible || true)
        {
            bar.SetPlantValues(GrowthController.getDominantChromosome(pair).Value0, GrowthController.getDominantChromosome(pair).ValueDev);
            bar.ShowPlantValue();
        }
    }
}