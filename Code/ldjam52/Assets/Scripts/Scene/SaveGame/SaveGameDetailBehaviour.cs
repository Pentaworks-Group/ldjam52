using Assets.Scripts.Core;

using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameDetailBehaviour : MonoBehaviour
    {
        private Text createdOn;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        public void Awake()
        {
            createdOn = transform.Find("Created/Value").GetComponent<Text>();
            timeStamp = transform.Find("TimeStamp/Value").GetComponent<Text>();
            timeElapsed = transform.Find("TimeElapsed/Value").GetComponent<Text>();
            moneyAmount = transform.Find("Money/Value").GetComponent<Text>();
        }



        public void DisplayDetails(SaveGameListSlotBehaviour behaviour)
        {
            GameState saveGame = behaviour.GetGameState();
            createdOn.text = string.Format("{0:G}", saveGame.CreatedOn);
            timeStamp.text = string.Format("{0:G}", saveGame.SavedOn);
            timeElapsed.text = string.Format("{0:F1}s", saveGame.ElapsedTime);
            moneyAmount.text = saveGame.FarmStorage.MoneyBalance.ToString();
        }
    }
}
