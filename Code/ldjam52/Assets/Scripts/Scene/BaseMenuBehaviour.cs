using UnityEngine;

using Assets.Scripts.Constants;

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

        public void ToMainMenu()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.MainMenu);
        }
    }
}
