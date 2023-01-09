using Assets.Scripts.Core;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameListSlotBehaviour : ListSlotBehaviour
    {

        private Text createdOn;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        private SaveGameListBehaviour listBehaviour;


        override public void RudeAwake()
        {
            createdOn = transform.Find("SlotContainer/Info/Created").GetComponent<Text>();
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
            createdOn.text = string.Format("{0:G}", saveGame.CreatedOn);
            timeStamp.text = string.Format("{0:G}", saveGame.SavedOn);
            timeElapsed.text = string.Format("{0:F1}s", saveGame.ElapsedTime); 
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
