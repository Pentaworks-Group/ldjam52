using Assets.Scripts.Scenes;

using UnityEngine;

public class OptionsSceneBehaviour : BaseMenuBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMainMenu();
        }
    }
}
