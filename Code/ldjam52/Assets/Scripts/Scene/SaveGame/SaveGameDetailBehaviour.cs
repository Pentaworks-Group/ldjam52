using Assets.Scripts.Core;

using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameDetailBehaviour : MonoBehaviour
    {
        private Text saveGameName;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        public void Awake()
        {
            saveGameName = transform.Find("Name").GetComponent<Text>();
            timeStamp = transform.Find("TimeStamp").GetComponent<Text>();
            timeElapsed = transform.Find("TimeElapsed").GetComponent<Text>();
            moneyAmount = transform.Find("Money").GetComponent<Text>();
        }



        public void DisplayDetails(SaveGameListSlotBehaviour behaviour)
        {
            GameState saveGame = behaviour.GetGameState();
            saveGameName.text = saveGame.CreatedOn.ToString();
            timeStamp.text = saveGame.SavedOn.ToString();
            timeElapsed.text = saveGame.ElapsedTime.ToString();
            moneyAmount.text = saveGame.FarmStorage.MoneyBalance.ToString();
        }
    }
}
