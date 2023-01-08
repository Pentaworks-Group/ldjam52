using Assets.Scripts.Core;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListSlotBehaviour : ListSlotBehaviour
    {

        private Text saveGameName;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        private SaveGameListBehaviour listBehaviour;

        override public void RudeAwake()
        {
            saveGameName = transform.Find("SlotContainer/Info/Name").GetComponent<Text>();
            timeStamp = transform.Find("SlotContainer/Info/TimeStamp").GetComponent<Text>();
            timeElapsed = transform.Find("SlotContainer/Info/TimeElapsed").GetComponent<Text>();
            moneyAmount = transform.Find("SlotContainer/Info/Money").GetComponent<Text>();

            listBehaviour = transform.parent.parent.GetComponent<SaveGameListBehaviour>();
        }

        public GameState GetGameState()
        {
            return (GameState)obj;
        }

        override public void UpdateUI()
        {
            GameState saveGame = GetGameState();
            saveGameName.text = saveGame.CreatedOn.ToString();
            timeStamp.text = saveGame.SavedOn.ToString();
            timeElapsed.text = saveGame.ElapsedTime.ToString();
            moneyAmount.text = saveGame.FarmStorage.MoneyBalance.ToString();
        }

        public void LoadGame()
        {
            Assets.Scripts.Base.Core.Game.Start(GetGameState());
        }

        public void OverrideGame()
        {
            SaveGameController.OverrideSave(index);
            listBehaviour.UpdateList();
        }

        public void DeleteGame()
        {
            SaveGameController.DeleteSave(index);
            listBehaviour.UpdateList();
        }
    }
}
