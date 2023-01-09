using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Inventory
{
    public class FarmStorageController
    {
        /// <summary>
        /// Returns the free space left in the farm storage
        /// </summary>
        /// <returns></returns>
        public static int GetFreeStorageSpace()
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            return storage.StorageSize - getTotalStorageFilled(storage);
        }

        /// <summary>
        /// Returns all the items in storage
        /// </summary>
        /// <returns></returns>
        public static List<StorageItem> getStorageInventory()
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            return storage.StorageItems;
        }

        // Return money balance
        public static int GetStorageBalance()
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            return storage.MoneyBalance;
        }


        /// <summary>
        /// Returns the amount of this plant which are stored
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        public static int GetPlantCountInStorage(Plant plant)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = GetStorageItemToPlantOrDefault(storage, plant);

            if (item == default)
            {
                return 0;
            }

            return item.StorageAmountPlants;
        }

        /// <summary>
        /// Tries to put in an amount of plants and returns the amount that fitted in the storage
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int PutPlantInStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitveAmount = amount;
            if (GetFreeStorageSpace() < amount)
            {
                definitveAmount = amount;
            }
            item.StorageAmountPlants += definitveAmount;

            return definitveAmount;
        }

        /// <summary>
        /// Tries to take out an amount of plants and returns the amount that was possible to take out
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Double TakePlantOfStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitiveAmount = amount;
            if (item.StorageAmountPlants < amount)
                definitiveAmount = item.StorageAmountPlants;
            item.StorageAmountPlants = Math.Max(0, item.StorageAmountPlants - amount);

            return definitiveAmount;
        }


        /// <summary>
        /// Returns the amount of this seed which are stored
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        public static int GetSeedCountInStorage(Plant plant)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = GetStorageItemToPlantOrDefault(storage, plant);

            if (item == default)
            {
                return 0;
            }

            return item.StorageAmountSeeds;
        }

        /// <summary>
        /// Tries to put in an amount of seeds and returns the amount that fitted in the storage
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int PutSeedInStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitveAmount = amount;
            if (GetFreeStorageSpace() < amount)
            {
                definitveAmount = GetFreeStorageSpace();
            }
            item.StorageAmountSeeds += definitveAmount;

            return definitveAmount;
        }

        /// <summary>
        /// Tries to take out an amount of seeds and returns the amount that was possible to take out
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Double TakeSeedsOfStorage(Plant plant, int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            StorageItem item = getStorageItemToPlant(storage, plant);

            int definitiveAmount = amount;
            if (item.StorageAmountSeeds < amount)
                definitiveAmount = item.StorageAmountSeeds;
            item.StorageAmountSeeds = Math.Max(0, item.StorageAmountSeeds - amount);

            return definitiveAmount;
        }

        /// <summary>
        /// Puts Money on your account
        /// </summary>
        /// <param name="amount"></param>
        public static void PutMoneyInStorage(int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            storage.MoneyBalance += amount;
        }

        //Takes Money on your account
        public static void TakeMoneyOfStorage(int amount)
        {
            FarmStorage storage = Base.Core.Game.State.FarmStorage;
            storage.MoneyBalance -= amount;
        }

        private static StorageItem GetStorageItemToPlantOrDefault(FarmStorage storage, Plant plant)
        {
            return storage.StorageItems.FirstOrDefault(i => i.Plant.ID == plant.ID);
        }

        public static StorageItem getStorageItemToPlant(FarmStorage storage, Plant plant, bool createIfInexistant = true)
        {
            var item = GetStorageItemToPlantOrDefault(storage, plant);

            if (item == null && createIfInexistant)
            {
                item = new StorageItem
                {
                    Plant = plant
                };

                storage.StorageItems.Add(item);
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
