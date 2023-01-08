using System.Collections.Generic;

using Assets.Scripts.Base;
using Assets.Scripts.Constants;
using Assets.Scripts.Core;

using GameFrame.Core.Audio.Multi;
using GameFrame.Core.Audio.Single;

using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public EffectsAudioManager EffectsAudioManager;
    public ContinuousAudioManager AmbienceAudioManager;
    public ContinuousAudioManager BackgroundAudioManager;

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
        Core.Game.PlayButtonSound();

        //if (Assets.Scripts.Base.Core.Game.Options.ShowTutorial)
        //{
        //    Core.Game.ChangeScene(SceneNames.Tutorial);
        //}
        //else
        //{
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
        StartAudioManagers();
        LoadGameSettings();
    }

    private void StartAudioManagers()
    {
        if (Core.Game.EffectsAudioManager == default)
        {
            Core.Game.EffectsAudioManager = this.EffectsAudioManager;
            Core.Game.EffectsAudioManager.Volume = Core.Game.Options.EffectsVolume;
            Core.Game.EffectsAudioManager.Initialize();
        }

        if (Core.Game.AmbienceAudioManager == default)
        {
            Core.Game.AmbienceAudioManager = this.AmbienceAudioManager;
            Core.Game.AmbienceAudioManager.Volume = Core.Game.Options.AmbienceVolume;
            Core.Game.AmbienceAudioManager.Initialize();

            this.AmbienceAudioManager.Clips = new List<AudioClip>()
            {
                GameFrame.Base.Resources.Manager.Audio.Get("Background001")
            };

            this.AmbienceAudioManager.Resume();
        }

        if (Core.Game.BackgroundAudioManager == default)
        {
            Core.Game.BackgroundAudioManager = this.BackgroundAudioManager;
            Core.Game.BackgroundAudioManager.Volume = Core.Game.Options.BackgroundVolume;
            Core.Game.BackgroundAudioManager.Initialize();
        }
    }
}
