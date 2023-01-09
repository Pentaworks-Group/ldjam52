using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Model;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class InformationPrefabBehaviour : MonoBehaviour
{

    public GameObject Information;

    public StorageItem Item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo(StorageItem item, Field field, bool isPlant)
    {
        this.Item = item;

        Transform information = Information.transform;

        // Always shown
        if (item!=null)
        {
            information.Find("Name").GetComponent<TMP_Text>().text = item.Plant.Name;

            if (isPlant)
                information.Find("Amount").GetComponent<TMP_Text>().text = "Amount: " + item.StorageAmountPlants.ToString();
            else
                information.Find("Amount").GetComponent<TMP_Text>().text = "Amount: " + item.StorageAmountSeeds.ToString();

            information.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            if (isPlant)
                information.Find("Image").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(item.Plant.ImageName);
            else
                information.Find("Image").GetComponent<Image>().sprite = GameFrame.Base.Resources.Manager.Sprites.Get(item.Plant.SeedImageName);


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
            if (field != null)
            {
                drawStatsBar(information, "Temp", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP], field.Temperature, field.IsTemperatureVisible);
                drawStatsBar(information, "Sun", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SUN], field.Sunshine, field.IsSunshineVisible);
                drawStatsBar(information, "Water", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.WATER], field.Humidity, field.IsHumidityVisible);
                drawStatsBar(information, "Fertility", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.FERTILITY], field.Fertility, field.IsFertiliyVisible);
            }
            else
            {
                drawStatsBar(information, "Temp", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.TEMP], 0, false);
                drawStatsBar(information, "Sun", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.SUN], 0, false);
                drawStatsBar(information, "Water", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.WATER], 0, false);
                drawStatsBar(information, "Fertility", item.Plant.Genome[Assets.Scripts.Constants.ChromosomeTypes.FERTILITY], 0, false);
            }
        }
        else
        {
            information.Find("Name").GetComponent<TMP_Text>().text = "";
            information.Find("Amount").GetComponent<TMP_Text>().text = "Amount:";
            information.Find("Image").GetComponent<Image>().sprite = null;
            information.Find("SeedsValue").GetComponent<TMP_Text>().text = "Seed value:";
            information.Find("PlantsValue").GetComponent<TMP_Text>().text = "Plant value ";
            information.Find("Seeds").GetComponent<TMP_Text>().text = "Seeds:";
            information.Find("Harvest").GetComponent<TMP_Text>().text = "Harvest:";

            if (field != null)
            {
                drawStatsBar(information, "Temp", null, field.Temperature, field.IsTemperatureVisible);
                drawStatsBar(information, "Sun", null, field.Sunshine, field.IsSunshineVisible);
                drawStatsBar(information, "Water", null, field.Humidity, field.IsHumidityVisible);
                drawStatsBar(information, "Fertility", null, field.Fertility, field.IsFertiliyVisible);
            }
            else
            {
                drawStatsBar(information, "Temp", null, 0, false);
                drawStatsBar(information, "Sun", null, 0, false);
                drawStatsBar(information, "Water", null, 0, false);
                drawStatsBar(information, "Fertility", null, 0, false);
            }

        }

    }

    private void drawStatsBar(Transform infoPanel, string barName, ChromosomePair pair, double biomeValue, bool isBiomeValueVisible)
    {
        StatsBar bar = infoPanel.Find(barName).GetChild(0).GetComponent<StatsBar>();

        if (pair != null && pair.IsVisible)
        {
            bar.SetPlantValues(GrowthController.getDominantChromosome(pair).Value0, GrowthController.getDominantChromosome(pair).ValueDev);
            bar.ShowPlantValue();
        }
        else
        {
            bar.HidePlantValue();
        }

        if (isBiomeValueVisible)
        {
            bar.SetBiomeValue(biomeValue);
            bar.ShowBiomeValue();
        }
        else
        {
            bar.hideBiomeValue();
        }
    }
}
