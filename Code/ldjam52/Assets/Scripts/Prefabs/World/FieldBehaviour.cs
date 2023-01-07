using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class FieldBehaviour : MonoBehaviour
{
    private Plant plant;
    private bool planted = false;
    private float nextStadium = 2;
    private int currentStadium = -1;
    private int ripe = 4;
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


    public void SelectField()
    {
        if (planted)
        {
            if (currentStadium == ripe)
            {
                Debug.Log("Harvest");
                ClearField();
            }
        }
        else
        {
            Debug.Log("Planted");
            planted = true;
            nextStadium = currentGameTime;
        }
    }

    private void OnMouseDown()
    {
        SelectField();
    }

    private void CheckIfNextStadium()
    {
        if (planted && currentStadium < ripe && nextStadium < currentGameTime)
        {
            string nextModelName = GetNextModelName();
            ReplacePlantModel(nextModelName);
            nextStadium = GetNextGrowthTick();
        }
    }

    private float GetNextGrowthTick()
    {
        return nextStadium + 2;
    }

    private string GetNextModelName()
    {
        currentStadium += 1;
        return "Flower" + currentStadium;
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

    private void ClearField()
    {
        planted = false;
        currentStadium = -1;
        foreach (GameObject flowerPot in flowerPots)
        {
            foreach (Transform child in flowerPot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
