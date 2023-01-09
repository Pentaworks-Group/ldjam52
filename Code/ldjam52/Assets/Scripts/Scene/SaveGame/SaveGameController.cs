using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameController
    {
        private static List<GameState> savedGames;

        public static List<GameState> GetSaveGames()
        {
            if (savedGames == default)
            {
                LoadSaveGamesFromPlayerPrefs();
            }
            return savedGames;
        }

        private static void LoadSaveGamesFromPlayerPrefs()
        {
            var savedGamesJson = PlayerPrefs.GetString("SavedGames");

            if (!System.String.IsNullOrEmpty(savedGamesJson))
            {
                try
                {
                    savedGames = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson);
                    return;
                }
                catch
                {
                }
            }
            savedGames = new();
        }

        public static void SaveNewGame()
        {
            Base.Core.Game.State.SavedOn = DateTime.Now;
            var saveGameJson = GameFrame.Core.Json.Handler.Serialize(Base.Core.Game.State);
            var saveGame = GameFrame.Core.Json.Handler.Deserialize<GameState>(saveGameJson);
            GetSaveGames().Add(saveGame);
            SaveGames();
        }

        public static void SaveGames()
        {
            var savedGamesJson = GameFrame.Core.Json.Handler.Serialize(savedGames);
            PlayerPrefs.SetString("SavedGames", savedGamesJson);
            PlayerPrefs.Save();
        }

        public static void OverrideSave(int index)
        {
            if (Assets.Scripts.Base.Core.Game.State != default)
            {
                Assets.Scripts.Base.Core.Game.State.SavedOn = DateTime.Now;
                savedGames[index] = Assets.Scripts.Base.Core.Game.State;
                SaveGames();
            }
        }

        public static void DeleteSave(int index)
        {
            savedGames.RemoveAt(index);
            SaveGames();
        }
    }
}
