using System.Collections.Generic;
using System.Linq;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListBehaviour : ListContainerBehaviour
    {
        public Button SaveNewButton;

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

            if (!Assets.Scripts.Base.Core.Game.IsFileAccessPossible)
            {
                if (saveGames.Count >= 5)
                {
                    SaveNewButton.interactable = false;
                }
                else
                {
                    SaveNewButton.interactable = true;
                }
            }
        }

        public void SaveGame()
        {
            SaveGameController.SaveGame();
            UpdateList();
        }


    }
}
