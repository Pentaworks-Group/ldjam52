using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Model;

using UnityEngine;

public class TileViewSeedListBehaviour : ListContainerBehaviour
{
    override public void CustomStart()
    {

        List<System.Object> plants = new();
        foreach (StorageItem item in Assets.Scripts.Base.Core.Game.State.FarmStorage.StorageItems)
        {
            if (item.StorageAmountSeeds > 0)
            {
                plants.Add(item);
            }
        }
   

        SetObjects(plants);
    }

}
