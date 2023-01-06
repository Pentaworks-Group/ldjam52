using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public void ShowSavedGames()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.SavedGames);
    }

    public void ShowModes()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.GameModeScene);
    }

    public void ShowOptions()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.OptionsScene);
    }

    public void ShowCredits()
    {
        Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.Credits);
    }

    public void StartGame()
    {
        if (Assets.Scripts.Base.Core.Game.Options.ShowTutorial)
        {
            Core.Game.PlayButtonSound();
            Core.Game.ChangeScene(SceneNames.Tutorial);
        }
        else
        {
            Core.Game.PlayButtonSound();
            Core.Game.Start();

        }
    }

}
