using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListBehaviour : ListContainerBehaviour<KeyValuePair<string, GameState>>
    {
        public Button SaveNewButton;

        override public void CustomStart()
        {

            UpdateList();
        }

        public void UpdateList()
        {
            List<KeyValuePair<string, GameState>> saveGames = SaveGameController.SavedGames.OrderByDescending(kvp => kvp.Key).ToList();
            
            SetContentList(saveGames);

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
