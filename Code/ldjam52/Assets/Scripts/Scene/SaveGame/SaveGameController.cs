using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public static void SaveGame()
        {
            if (Base.Core.Game.State != default)
            {
                SaveGame(Base.Core.Game.State);
            }
        }

        public static void SaveGame(GameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            SavedGames[key] = clone;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(clone, Formatting.None, saveGameSerializationSettings.Value);

            gameStateAsJson = Compress(gameStateAsJson);

            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistIndexAndSave();
        }

        public static void OverwriteSavedGame(String targetKey)
        {
            if (Base.Core.Game.State != default)
            {
                OverwriteSavedGame(targetKey, Base.Core.Game.State);
            }
        }

        public static void OverwriteSavedGame(String targetKey, GameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            SavedGames.Remove(targetKey);
            SavedGames[key] = clone;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(clone, Formatting.None, saveGameSerializationSettings.Value);

            gameStateAsJson = Compress(gameStateAsJson);

            PlayerPrefs.DeleteKey(targetKey);
            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistIndexAndSave();
        }

        public static void DeleteSavedGame(String targetKey)
        {
            SavedGames.Remove(targetKey);
            PlayerPrefs.DeleteKey(targetKey);
            PersistIndexAndSave();
        }

        public static void LoadSavedGame(GameState gameState)
        {
            var clone = CloneInAnUglyFashionJsonStyle(gameState);

            Assets.Scripts.Base.Core.Game.Start(clone);
        }

        private static void PersistIndexAndSave()
        {
            var indexAsJson = GameFrame.Core.Json.Handler.Serialize(SavedGames.Keys.ToList(), Formatting.None, new JsonSerializerSettings());
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
            var dictionary = new Dictionary<String, GameState>();

            var indexList = LoadFromPlayerPrefs<List<String>>(SavedGameIndexKey, new JsonSerializerSettings());

            if (indexList != default)
            {
                foreach (var index in indexList)
                {
                    var gameState = LoadFromPlayerPrefs<GameState>(index, saveGameSerializationSettings.Value, true);

                    if (gameState != default)
                    {
                        dictionary[index] = gameState;
                    }
                }
            }

            return dictionary;
        }

        private static T LoadFromPlayerPrefs<T>(String key, JsonSerializerSettings serializerSettings, Boolean useCompression = false)
        {
            if (!String.IsNullOrWhiteSpace(key))
            {
                var keyContent = PlayerPrefs.GetString(key);

                if (!String.IsNullOrWhiteSpace(keyContent))
                {
                    try
                    {
                        if (useCompression)
                        {
                            keyContent = Decompress(keyContent);
                        }

                        return GameFrame.Core.Json.Handler.Deserialize<T>(keyContent, serializerSettings);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError("Failed to deserialize stored index!");
                        Debug.LogError(exception.ToString());
                    }
                }
            }

            return default;
        }

        private static String Compress(String source)
        {
            var payload = Encoding.UTF8.GetBytes(source);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
                {
                    gzipStream.Write(payload, 0, payload.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static String Decompress(String compressedString)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(compressedString)))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }

                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
    }
}
