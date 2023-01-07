using Assets.Scripts.Model;
using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Inventory
{
    public class FormStorageBehaviour
    {
        public static int GetFreeStorageSpace()
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            return storage.StorageSize-getTotalStorageFilled(storage);
        }

        public static List<StorageItem> getStorageInventory()
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            return storage.StorageItems;
        }

        public static int GutPlantInStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitveAmount = amount;
            if (GetFreeStorageSpace() < amount)
            {
                definitveAmount = amount;
            }
            item.StorageAmountPlants += definitveAmount;

            return definitveAmount;
        }

        public static Double TakePlantOfStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitiveAmount = amount;
            if (item.StorageAmountPlants<amount)
                definitiveAmount = item.StorageAmountPlants;
            item.StorageAmountPlants = Math.Max(0, item.StorageAmountPlants - amount);

            return definitiveAmount;
        }

        public static int PutSeedInStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitveAmount = amount;
            if (GetFreeStorageSpace() < amount)
            {
                definitveAmount = amount;
            }
            item.StorageAmountSeeds += definitveAmount;

            return definitveAmount;
        }

        public static Double TakeSeedsOfStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitiveAmount = amount;
            if (item.StorageAmountSeeds < amount)
                definitiveAmount = item.StorageAmountSeeds;
            item.StorageAmountSeeds = Math.Max(0, item.StorageAmountSeeds - amount);

            return definitiveAmount;
        }

        public static void PutMoneyInStorage(int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            storage.MoneyBalance += amount;
        }

        public static void TakeMoneyOfStorage(int amount)
        {
            FarmStorage storage = Base.Core.Game.FarmStorage;
            storage.MoneyBalance -= amount;
        }

        private static StorageItem getStorageItemToPlant(FarmStorage storage, Plant plant)
        {
            var item = storage.StorageItems.First(i => i.Plant.ID == plant.ID);

            if (item == null)
            {
                item = new StorageItem
                {
                    Plant = plant
                };
            }
            return item;
        }

        private static int getTotalStorageFilled(FarmStorage storage)
        {
            int filled = 0;
            foreach (StorageItem item in storage.StorageItems)
            {
                filled += item.StorageAmountSeeds + item.StorageAmountPlants;
            }
            return filled;
        }
    }
}
