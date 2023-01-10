using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

using Assets.Scripts.Core;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SaveGameController
    {
        private static List<GameState> savedGames;

        private static Lazy<JsonSerializerSettings> saveGameSerializationSettings = new Lazy<JsonSerializerSettings>(() =>
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.None
            };
        });

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
                    savedGamesJson = Decompress(savedGamesJson);
                    savedGames = GameFrame.Core.Json.Handler.Deserialize<List<GameState>>(savedGamesJson, saveGameSerializationSettings.Value);
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

            var saveGameJson = GameFrame.Core.Json.Handler.Serialize(Base.Core.Game.State, Formatting.None, saveGameSerializationSettings.Value);
            var saveGame = GameFrame.Core.Json.Handler.Deserialize<GameState>(saveGameJson, saveGameSerializationSettings.Value);

            GetSaveGames().Add(saveGame);
            SaveGames();
        }

        public static string Compress(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                bytes = memoryStream.ToArray();
                return Encoding.UTF8.GetString(bytes);

            }
        }

        public static string Decompress(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            using (var memoryStream = new MemoryStream(bytes))
            {

                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }
                    bytes = memoryStream.ToArray();
                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }

        public static void SaveGames()
        {
            var savedGamesJson = GameFrame.Core.Json.Handler.Serialize(savedGames, Formatting.None, saveGameSerializationSettings.Value);
            savedGamesJson = Compress(savedGamesJson);
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
