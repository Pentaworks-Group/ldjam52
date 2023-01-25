using System;
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

        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            List<KeyValuePair<string, GameState>> saveGames = SaveGameController.SavedGames.OrderBy(kvp => kvp.Key).ToList();

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
            Base.Core.Game.PlayButtonSound();

            SaveGameController.SaveGame();
            UpdateList();
        }
    }
}
