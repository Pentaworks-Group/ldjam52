using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SavedGameListBehaviour : ListContainerBehaviour<KeyValuePair<String, SavedGamedPreviewImpl>>
    {
        public Button SaveNewButton;

        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            List<KeyValuePair<String, SavedGamedPreviewImpl>> savedGames = Base.Core.Game.GetSavedGamePreviews().OrderBy(kvp => kvp.Key).ToList();

            SetContentList(savedGames);

            if (!Assets.Scripts.Base.Core.Game.IsFileAccessPossible)
            {
                if (savedGames.Count >= 5)
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

            Base.Core.Game.SaveGame();
            UpdateList();
        }
    }
}
