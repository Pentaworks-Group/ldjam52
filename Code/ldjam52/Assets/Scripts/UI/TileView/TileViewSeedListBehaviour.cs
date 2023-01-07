using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Model;

using UnityEngine;

public class TileViewSeedListBehaviour : ListContainerBehaviour
{
    override public void CustomStart()
    {
        Plant plant1 = new Plant();
        plant1.Name = "Hans";
        plant1.ImageName = "Mais";



        Plant plant2 = new Plant();
        plant2.Name = "adsf";
        plant2.ImageName = "Fertilizer";

        List<System.Object> plants = new();
        plants.Add(plant2);
        plants.Add(plant1);

        SetObjects(plants);
    }

}
