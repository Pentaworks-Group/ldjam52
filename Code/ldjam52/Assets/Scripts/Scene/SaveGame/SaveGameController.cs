using System;
using System.Collections.Generic;

using Assets.Scripts.Core;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameController
    {
        private static Lazy<JsonSerializerSettings> saveGameSerializationSettings = new Lazy<JsonSerializerSettings>(() =>
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.None
            };
        });

        private const String SavedGameIndexKey = "SavedGameIndex";

        private static readonly Lazy<Dictionary<string, GameState>> savedGameIndex = new Lazy<Dictionary<string, GameState>>(LoadSaveGamesFromPlayerPrefs);

        public static Dictionary<String, GameState> SavedGames => savedGameIndex.Value;

        public static void SaveGame(GameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            SavedGames[key] = gameState;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(clone, Formatting.None, saveGameSerializationSettings.Value);

            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistIndexAndSave();
        }

        public static void OverwriteSavedGame(String targetKey, GameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            SavedGames.Remove(targetKey);
            SavedGames[key] = gameState;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(clone, Formatting.None, saveGameSerializationSettings.Value);

            PlayerPrefs.DeleteKey(targetKey);
            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistIndexAndSave();
        }

        public static void DeleteSavedGame(String targetKey)
        {
            SavedGames.Remove(targetKey);
            PersistIndexAndSave();
        }

        public static void LoadSave(GameState gameState)
        {
            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            Assets.Scripts.Base.Core.Game.Start(clone);
        }

        private static void PersistIndexAndSave()
        {
            var indexAsJson = GameFrame.Core.Json.Handler.Serialize(SavedGames.Keys, Formatting.None);
            PlayerPrefs.SetString(SavedGameIndexKey, indexAsJson);
            PlayerPrefs.Save();
        }

        private static GameState CloneInAnUglyFashionJsonStyle(GameState gameState)
        {
            var saveGameJson = GameFrame.Core.Json.Handler.Serialize(gameState, Formatting.None, saveGameSerializationSettings.Value);

            return GameFrame.Core.Json.Handler.Deserialize<GameState>(saveGameJson, saveGameSerializationSettings.Value);
        }

        private static Dictionary<String, GameState> LoadSaveGamesFromPlayerPrefs()
        {
            var dictionary = default(Dictionary<String, GameState>);

            var indexList = LoadIndexFromPlayerPrefs();

            //var savedGamesJson = PlayerPrefs.GetString("SavedGames");

            //if (!System.String.IsNullOrEmpty(savedGamesJson))
            //{
            //    try
            //    {
            //        dictionary = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson, saveGameSerializationSettings.Value);
            //    }
            //    catch (Exception exception)
            //    {
            //        dictionary = new Dictionary<String, GameState>();
            //    }
            //}

            return dictionary;
        }

        private static List<String> LoadIndexFromPlayerPrefs()
        {
            var indexListString = PlayerPrefs.GetString(SavedGameIndexKey);

            if (!String.IsNullOrWhiteSpace(indexListString))
            {
                try
                {
                    return GameFrame.Core.Json.Handler.Deserialize<List<String>>(indexListString);
                }
                catch (Exception exception)
                {
                    Debug.LogError("Failed to deserialize stored index!");
                    Debug.LogError(exception.ToString());
                }
            }
        }
    }
}
