using UnityEngine;

using Assets.Scripts.Constants;

namespace Assets.Scripts.Scenes
{
    public class BaseMenuBehaviour : MonoBehaviour
    {

        private void Awake()
        {
            //if (GameHandler.AvailableGameModes == default)
            //{
            //    Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
            //    return;
            //}

            Debug.Log("Implement Default to MainMenu");

            CustomAwake();
        }

        protected virtual void CustomAwake()
        {

        }

        public void ToMainMenu()
        {
            //Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
    }
}
