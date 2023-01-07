using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core;

using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public void ShowSavedGames()
    {
        //Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.SavedGames);
    }

    public void ShowModes()
    {
        //Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.GameMode);
    }

    public void ShowOptions()
    {
        //Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.Options);
    }

    public void ShowCredits()
    {
        //Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.Credits);
    }

    public void ShowTestField()
    {
        //Core.Game.PlayButtonSound();
        Core.Game.ChangeScene(SceneNames.TestField);
    }

    public void StartGame()
    {
        //if (Assets.Scripts.Base.Core.Game.Options.ShowTutorial)
        //{
        //    Core.Game.PlayButtonSound();
        //    Core.Game.ChangeScene(SceneNames.Tutorial);
        //}
        //else
        //{
        //Core.Game.PlayButtonSound();
        Core.Game.Start();

        //}
    }

    public void LoadGameSettings()
    {
        if (Core.Game.AvailableGameModes.Count == 0)
        {
            var filePath = Application.streamingAssetsPath + "/GameModes.json";
            StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<List<GameMode>>(filePath, SetGameSettings));
        }
    }

    private List<GameMode> SetGameSettings(List<GameMode> loadedGameModes)
    {
        if (loadedGameModes?.Count > 0)
        {
            foreach (var gameMode in loadedGameModes)
            {
                Core.Game.AvailableGameModes.Add(gameMode);
            }
        }

        if (Core.Game.SelectedGameMode == default)
        {
            Core.Game.SelectedGameMode = loadedGameModes[0];
        }

        return loadedGameModes;
    }

    private void Start()
    {
        LoadGameSettings();
    }
}
