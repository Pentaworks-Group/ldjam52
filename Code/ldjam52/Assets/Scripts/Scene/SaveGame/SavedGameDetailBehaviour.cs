using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Scene.SaveGame
{
    public class SavedGameDetailBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SavedGameListBehaviour listBehaviour;

        private Text createdOn;
        private Text timeStamp;
        private Text timeElapsed;
        private Text moneyAmount;

        [SerializeField]
        private GameObject container;

        private string key;

        public void Awake()
        {
            createdOn = transform.Find("DetailsContainer/Created/Value").GetComponent<Text>();
            timeStamp = transform.Find("DetailsContainer/TimeStamp/Value").GetComponent<Text>();
            timeElapsed = transform.Find("DetailsContainer/TimeElapsed/Value").GetComponent<Text>();
            moneyAmount = transform.Find("DetailsContainer/Money/Value").GetComponent<Text>();
            var overwriteButton = transform.Find("DetailsContainer/Buttons/Override").GetComponent<Button>();

            overwriteButton.gameObject.SetActive(Base.Core.Game.State != default);
            container.SetActive(false);
        }

        public void DisplayDetails(SavedGamedPreviewImpl preview)
        {
            createdOn.text = preview.CreatedOn;
            timeStamp.text = preview.SavedOn;
            timeElapsed.text = preview.TimeElapsed;
            moneyAmount.text = preview.Money;

            this.key = preview.Key;
            container.SetActive(true);
        }

        public void LoadGame()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadSavedGame(key);
        }

        public void OverrideGame()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.OverwriteSavedGame(key);
            listBehaviour.UpdateList();
            ClearDetails();
        }

        public void DeleteGame()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.DeleteSavedGame(key);
            listBehaviour.UpdateList();
            ClearDetails();
        }

        public void ClearDetails()
        {
            container.SetActive(false);
        }
    }
}
