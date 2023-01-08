using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBar : MonoBehaviour
{

    // Values in [0,1]
    public double Mean { get; set; }
    public double Width { get; set; }
    public double BiomeVal { get; set; }

    private bool knowsPlant = false;
    private bool knowsBiome = false;

    public RectTransform gradientTransform;
    public RectTransform biomeTransform;

    public StatsBar(double mean, double width)
    {
        Mean = mean;
        Width = width;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void TriggerPlant()
    {
        knowsPlant = true;
        gradientTransform.gameObject.SetActive(true);
    }

    public void TriggerBiome()
    {
        knowsPlant = true;
    }

    private void Draw()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
