using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using UnityEngine;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SavedGameController<TPreview, TGameState> where TPreview : SavedGamePreview<TGameState>, new()
                                                          where TGameState : GameFrame.Core.GameState
    {
        private static readonly Lazy<JsonSerializerSettings> savedGameSerializationSettings = new Lazy<JsonSerializerSettings>(() =>
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.None
            };
        });

        private const String SavedGameIndexKey = "SavedGameIndex";
        private const String SavedGamePreviewIndex = "SavedGamePreviewIndex";


        private TPreview CreatePreview(TGameState gameState, String key)
        {
            var preview = new TPreview();
            preview.Init(gameState, key);
            return preview;
        }

        private Dictionary<String, TPreview> savedGamePreviews;
        public Dictionary<String, TPreview> SaveGamePreviews
        {
            get
            {
                if (savedGamePreviews == default)
                {
                    savedGamePreviews = LoadSaveGamesPreviewsFromPlayerPrefs();
                }

                return savedGamePreviews;
            }
        }

        public void SaveGame(TGameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            TPreview preview = CreatePreview(gameState, key);
            SaveGamePreviews[key] = preview;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(gameState, Formatting.None, savedGameSerializationSettings.Value);

            gameStateAsJson = Compress(gameStateAsJson);

            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistSaveGamePreviews();
        }

        public void OverwriteSavedGame(String targetKey, TGameState gameState)
        {
            gameState.SavedOn = DateTime.Now;

            var key = $"SaveGame-{gameState.SavedOn.Ticks}";

            TPreview preview = CreatePreview(gameState, targetKey);
            this.SaveGamePreviews[targetKey] = preview;

            var gameStateAsJson = GameFrame.Core.Json.Handler.Serialize(gameState, Formatting.None, savedGameSerializationSettings.Value);

            gameStateAsJson = Compress(gameStateAsJson);

            PlayerPrefs.DeleteKey(targetKey);
            PlayerPrefs.SetString(key, gameStateAsJson);
            PersistSaveGamePreviews();
        }

        public void DeleteSavedGame(String targetKey)
        {
            SaveGamePreviews.Remove(targetKey);
            PlayerPrefs.DeleteKey(targetKey);
            PersistSaveGamePreviews();
        }

        public TGameState LoadSavedGame(string key)
        {
            return LoadFromPlayerPrefs<TGameState>(key, savedGameSerializationSettings.Value, true);
        }

        private Dictionary<String, TPreview> LoadSaveGamesPreviewsFromPlayerPrefs()
        {
            var previewList = LoadFromPlayerPrefs<Dictionary<string, TPreview>>(SavedGamePreviewIndex, new JsonSerializerSettings());

            if (previewList == default)
            {
                previewList = new Dictionary<String, TPreview>();
                var indexList = LoadFromPlayerPrefs<List<String>>(SavedGameIndexKey, new JsonSerializerSettings());

                if (indexList != default)
                {
                    foreach (var index in indexList)
                    {
                        var gameState = LoadFromPlayerPrefs<TGameState>(index, savedGameSerializationSettings.Value, true);
                        if (gameState != default)
                        {
                            TPreview preview = CreatePreview(gameState, index);

                            if (preview != default)
                            {
                                previewList[index] = preview;
                            }
                        }
                    }
                }
            }

            return previewList;
        }

        private void PersistSaveGamePreviews()
        {
            var indexAsJson = GameFrame.Core.Json.Handler.Serialize(SaveGamePreviews, Formatting.None, new JsonSerializerSettings());
            PlayerPrefs.SetString(SavedGamePreviewIndex, indexAsJson);
            PlayerPrefs.Save();
        }

        private T LoadFromPlayerPrefs<T>(String key, JsonSerializerSettings serializerSettings, Boolean useCompression = false)
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
                        UnityEngine.Debug.LogError("Failed to deserialize stored index!");
                        UnityEngine.Debug.LogError(exception.ToString());
                    }
                }
            }

            return default;
        }

        private String Compress(String source)
        {
            var payload = Encoding.UTF8.GetBytes(source);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
                {
                    gzipStream.Write(payload, 0, payload.Length);
                }

                //using (var brotliStream = new BrotliStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
                //{
                //    brotliStream.Write(payload, 0, payload.Length);
                //}

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private String Decompress(String compressedString)
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
