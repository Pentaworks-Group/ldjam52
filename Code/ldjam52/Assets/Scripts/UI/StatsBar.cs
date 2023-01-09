using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBar : MonoBehaviour
{

    // Values in [0,1]
    private double mean;
    private double width;
    private double biomeVal;

    private Color col;

    public RectTransform GradientTransform;
    public RectTransform BiomeTransform;
    public GameObject QuestionMark;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetPlantValues(double _mean, double _variance)
    {
        mean = _mean;
        width = _variance;

        GradientTransform.anchorMin = new Vector2((float)(mean - width), 0.0f);
        GradientTransform.anchorMax = new Vector2((float)(mean + width), 1.0f);

    }

    public void SetBiomeValue(double value)
    {
        biomeVal = value;

        BiomeTransform.anchorMin = new Vector2((float)value - 0.005f, 0.0f);
        BiomeTransform.anchorMax = new Vector2((float)value + 0.005f, 1.0f);
    }

    public void ShowPlantValue()
    {
        QuestionMark.SetActive(false);
        GradientTransform.gameObject.SetActive(true);
    }

    public void HidePlantValue()
    {
        QuestionMark.SetActive(true);
        GradientTransform.gameObject.SetActive(false);
    }

    public void ShowBiomeValue()
    {
        QuestionMark.SetActive(false);
        BiomeTransform.gameObject.SetActive(true);
    }

    public void hideBiomeValue()
    {
        BiomeTransform.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
