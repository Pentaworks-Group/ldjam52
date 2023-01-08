using System.Collections.Generic;

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
            foreach (GameState savedGame in SaveGameController.GetSaveGames())
            {
                saveGames.Add(savedGame);
            }

            SetObjects(saveGames);
        }

        public void SaveGame()
        {
            SaveGameController.SaveNewGame();
        }

       
    }
}
