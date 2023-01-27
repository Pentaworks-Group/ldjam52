using Assets.Scripts.Constants;

using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public class BaseMenuBehaviour : MonoBehaviour
    {

        private void Awake()
        {
            if (Base.Core.Game.AmbienceAudioManager == default)
            {
                Base.Core.Game.ChangeScene(SceneNames.MainMenu);
                return;
            }

            CustomAwake();
        }

        protected virtual void CustomAwake()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToMainMenu();
            }

            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }    

        public void ToMainMenu()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
    }
}
