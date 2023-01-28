using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SavedGameListSlotBehaviour : ListSlotBehaviour<KeyValuePair<string, SavedGamedPreviewImpl>>
    {
        private Text createdOn;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        public override void RudeAwake()
        {
            createdOn = transform.Find("SlotContainer/Info/Created").GetComponent<Text>();
            timeStamp = transform.Find("SlotContainer/Info/TimeStamp").GetComponent<Text>();
            timeElapsed = transform.Find("SlotContainer/Info/TimeElapsed").GetComponent<Text>();
            moneyAmount = transform.Find("SlotContainer/Info/Money").GetComponent<Text>();
        }

        public SavedGamedPreviewImpl GetSavedGamedPreview()
        {
            return content.Value;
        }

        public void DisplaySlot(SavedGameDetailBehaviour details)
        {
            details.DisplayDetails(GetSavedGamedPreview());
        }

        public override void UpdateUI()
        {
            SavedGamedPreviewImpl savedGame = GetSavedGamedPreview();

            createdOn.text = savedGame.CreatedOn;
            timeStamp.text = savedGame.SavedOn;
            timeElapsed.text = savedGame.TimeElapsed;
            moneyAmount.text = savedGame.Money;
        }

        public void LoadGame()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadSavedGame(content.Key);
        }
    }
}
