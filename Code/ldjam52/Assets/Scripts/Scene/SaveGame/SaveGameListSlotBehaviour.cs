using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListSlotBehaviour : ListSlotBehaviour<KeyValuePair<string, GameState>>
    {
        private Text createdOn;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;
        private Button overwriteButton;
        private SaveGameListBehaviour listBehaviour;


        override public void RudeAwake()
        {
            createdOn = transform.Find("SlotContainer/Info/Created").GetComponent<Text>();
            timeStamp = transform.Find("SlotContainer/Info/TimeStamp").GetComponent<Text>();
            timeElapsed = transform.Find("SlotContainer/Info/TimeElapsed").GetComponent<Text>();
            moneyAmount = transform.Find("SlotContainer/Info/Money").GetComponent<Text>();
            overwriteButton = transform.Find("SlotContainer/Override").GetComponent<Button>();

            listBehaviour = transform.parent.parent.GetComponent<SaveGameListBehaviour>();

            overwriteButton.interactable = Base.Core.Game.State != default;
        }


     
        public GameState GetGameState()
        {
            return content.Value;
        }

        override public void UpdateUI()
        {
            GameState saveGame = GetGameState();

            createdOn.text = string.Format("{0:G}", saveGame.CreatedOn);
            timeStamp.text = string.Format("{0:G}", saveGame.SavedOn);
            timeElapsed.text = string.Format("{0:F1}s", saveGame.ElapsedTime);
            moneyAmount.text = saveGame.FarmStorage.MoneyBalance.ToString();
        }

        public void LoadGame()
        {
            SaveGameController.LoadSavedGame(GetGameState());
        }

        public void OverrideGame()
        {
            SaveGameController.OverwriteSavedGame(content.Key);
            listBehaviour.UpdateList();
        }

        public void DeleteGame()
        {
            SaveGameController.DeleteSavedGame(content.Key);
            listBehaviour.UpdateList();
        }
    }
}
