using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;

using GameFrame.Core.UI.List;

using UnityEngine;

public class FieldViewSeedListBehaviour : ListContainerBehaviour<StorageItem>
{
    override public void CustomStart()
    {

        UpdateList();
    }

    public void UpdateList()
    {
        List<StorageItem> plants = new();
        List<StorageItem> favorites = new ();
        foreach (StorageItem item in FarmStorageController.getStorageInventory())
        {
            if (item.StorageAmountSeeds > 0)
            {
                if (item.Plant.Favorit)
                {
                    favorites.Add(item);
                } else
                {
                    plants.Add(item);
                }
            }
        }
        plants.AddRange(favorites);
        SetContentList(plants);
    }

}
