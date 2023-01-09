using Assets.Scripts.Scenes;

using UnityEngine;

public class CreditsSceneBehaviour : BaseMenuBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMainMenu();
        }
    }
}
