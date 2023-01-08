using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Inventory;
using Assets.Scripts.Model;

using UnityEngine;

public class FieldViewSeedListBehaviour : ListContainerBehaviour
{
    override public void CustomStart()
    {

        UpdateList();
    }

    public void UpdateList()
    {
        List<System.Object> plants = new();
        foreach (StorageItem item in FarmStorageController.getStorageInventory())
        {
            if (item.StorageAmountSeeds > 0)
            {
                plants.Add(item);
            }
        }

        SetObjects(plants);
    }

}
