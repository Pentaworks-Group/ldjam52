using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListBehaviour : ListContainerBehaviour
    {
        override public void CustomStart()
        {

            UpdateList();
        }

        public void UpdateList()
        {
            List<System.Object> saveGames = new();

            foreach (var savedGame in SaveGameController.SavedGames.OrderByDescending(kvp => kvp.Key))
            {
                saveGames.Add(savedGame);
            }

            SetObjects(saveGames);
        }

        public void SaveGame()
        {
            SaveGameController.SaveGame();
            UpdateList();
        }

       
    }
}
