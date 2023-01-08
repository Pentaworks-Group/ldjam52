using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBar : MonoBehaviour
{

    // Values in [0,1]
    private double mean;
    private double var;
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
        var = _variance;

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
