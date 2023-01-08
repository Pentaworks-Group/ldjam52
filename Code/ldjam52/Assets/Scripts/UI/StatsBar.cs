using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBar : MonoBehaviour
{

    // Values in [0,1]
    private double mean { get; set; }
    private double width { get; set; }
    public double BiomeVal { get; set; }

    private Color col;

    private bool knowsPlant = false;
    private bool knowsBiome = false;

    public RectTransform gradientTransform;
    public RectTransform biomeTransform;

    public StatsBar(double _mean, double _width, Color _col)
    {
        mean = _mean;
        width = _width;
        col = _col;
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

    public void Draw()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
