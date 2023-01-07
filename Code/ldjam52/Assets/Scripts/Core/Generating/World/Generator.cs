using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generating.World
{
    public class Generator
    {
        private readonly GameMode gameMode;

        public Generator(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        private Model.World world;
        public Model.World World { get; }

        public Boolean Generate()
        {
            var isSuccessful = true;

            var biomesToPlace = new List<Biome>();

            var tiles = new Tile[gameMode.World.Columns, gameMode.World.Rows];

            if (gameMode.World.IsAllBiomesUsageForced)
            {
                biomesToPlace.AddRange(gameMode.AvailableBiomes);
            }

            if (gameMode.World.RandomBiomesAmount > 0)
            {
                biomesToPlace.Add(gameMode.AvailableBiomes.GetRandomEntry());
            }

            foreach (var biome in biomesToPlace)
            {
                GetAvailableSpawnPoint(tiles, out Int32 x, out Int32 z);

                var tile = new Tile()
                {
                    IsOwned = false,
                    Position = new GameFrame.Core.Math.Vector2(x, z),
                    Color = biome.Color,
                    Temperature = UnityEngine.Random.Range(biome.TemperatureMin, biome.TemperatureMax),
                    Fertility = UnityEngine.Random.Range(biome.FertilityMin, biome.FertilityMax),
                    Humidity = UnityEngine.Random.Range(biome.HumidityMin, biome.HumidityMax),
                    Sunshine = UnityEngine.Random.Range(biome.SunshineMin, biome.SunshineMax),
                };

                tiles[x,z] = tile;
            }

            return isSuccessful;
        }

        private void GetAvailableSpawnPoint(Tile[,] fields, out Int32 x, out Int32 z)
        {
            x = 0;
            z = 0;

            var positionFound = false;

            for (var counter = 0; counter < 10; counter++)
            {
                x = UnityEngine.Random.Range(0, this.gameMode.World.Columns);
                z = UnityEngine.Random.Range(0, this.gameMode.World.Rows);

                if (fields[x, z] == default)
                {
                    positionFound = true;
                    break;
                }
            }

            if (!positionFound)
            {
                throw new Exception("Failed to generate Position!");
            }
        }
    }
}
