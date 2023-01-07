using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    private Plant plant;
    private float planted;
    private float nextStadium = 2;
    private string currentStadium = "Flower0";
    private List<GameObject> flowerPots = new List<GameObject>();

    private float currentGameTime = 0;

    private void Awake()
    {
        Transform PotsParent = transform.Find("FlowerPots");
        foreach (Transform child in PotsParent)
        {
            flowerPots.Add(child.gameObject);
        }
    }

    void Update()
    {
        CheckIfNextStadium();
        currentGameTime += Time.deltaTime;
    }

    private void CheckIfNextStadium()
    {
        if (nextStadium < currentGameTime)
        {
            string nextModelName = GetNextModelName();
            ReplacePlantModel(nextModelName);
            nextStadium = GetNextGrowthTick();
        }
    }

    private float GetNextGrowthTick()
    {
        return nextStadium + 100;
    }

    private string GetNextModelName()
    {
        return "Flower1";
    }

    private void ReplacePlantModel(string newModelName)
    {
        foreach (GameObject flowerPot in flowerPots)
        {
            foreach (Transform child in flowerPot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Transform template = transform.Find("Flowers/" + newModelName);
            Transform flower = Instantiate(template, flowerPot.transform);
            flower.gameObject.name = "Flower";
            flower.gameObject.SetActive(true);
        }
    }
}
