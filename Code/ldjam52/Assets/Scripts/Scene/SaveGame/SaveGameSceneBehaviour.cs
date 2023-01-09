using Assets.Scripts.Scenes;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameSceneBehaviour : BaseMenuBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToMainMenu();
            }
        }
    }
}
