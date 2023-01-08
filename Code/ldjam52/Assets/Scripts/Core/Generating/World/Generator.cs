using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generating.World
{
    public class Generator
    {
        private const Int32 TilePrice = 1;

        private readonly GameMode gameMode;
        private List<BiomeSpawn> biomes;

        public Generator(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        private Model.World world;
        public Model.World World { get { return this.world; } }

        public Boolean Generate()
        {
            var isSuccessful = true;

            biomes = new List<BiomeSpawn>();

            var tiles = new Tile[gameMode.World.Width, gameMode.World.Height];

            if (gameMode.World.IsAllBiomesUsageForced)
            {
                foreach (var biome in gameMode.AvailableBiomes)
                {
                    biomes.Add(new BiomeSpawn(biome));
                }
            }

            if (gameMode.World.RandomBiomesAmount > 0)
            {
                for (int i = 0; i < gameMode.World.RandomBiomesAmount; i++)
                {
                    var biome = gameMode.AvailableBiomes.GetRandomEntry();

                    biomes.Add(new BiomeSpawn(biome));
                }
            }

            foreach (var biomeSpawn in biomes)
            {
                GetAvailableSpawnPoint(tiles, out Int32 x, out Int32 z);

                biomeSpawn.Position = new GameFrame.Core.Math.Vector3(x, 0, z);

                var tile = new Tile()
                {
                    ID = Guid.NewGuid(),
                    IsOwned = false,
                    Position = new GameFrame.Core.Math.Vector3(x, 0, z),
                    Color = biomeSpawn.Biome.Color,
                    Price = TilePrice,
                    Field = new Field()
                    {
                        Temperature = UnityEngine.Random.Range(biomeSpawn.Biome.TemperatureMin, biomeSpawn.Biome.TemperatureMax),
                        Fertility = UnityEngine.Random.Range(biomeSpawn.Biome.FertilityMin, biomeSpawn.Biome.FertilityMax),
                        Humidity = UnityEngine.Random.Range(biomeSpawn.Biome.HumidityMin, biomeSpawn.Biome.HumidityMax),
                        Sunshine = UnityEngine.Random.Range(biomeSpawn.Biome.SunshineMin, biomeSpawn.Biome.SunshineMax),
                    }
                };

                tiles[x, z] = tile;
            }

            var tileList = FillEmptyTiles(tiles);

            this.world = new Model.World()
            {
                Height = gameMode.World.Height,
                Width = gameMode.World.Width,
                Tiles = tileList
            };

            return isSuccessful;
        }

        private List<Tile> FillEmptyTiles(Tile[,] tiles)
        {
            var tileList = new List<Tile>();

            for (int x = 0; x < gameMode.World.Width; x++)
            {
                for (int z = 0; z < gameMode.World.Height; z++)
                {
                    var tile = tiles[x, z];

                    if (tile == default)
                    {
                        tile = new Tile()
                        {
                            ID = Guid.NewGuid(),
                            Position = new GameFrame.Core.Math.Vector3(x, 0, z),
                            Price = TilePrice
                        };

                        var closestSpawn = FindBiome(tile);

                        tile.Color = closestSpawn.Biome.Color;

                        tile.Field = new Field()
                        {
                            Temperature = UnityEngine.Random.Range(closestSpawn.Biome.TemperatureMin, closestSpawn.Biome.TemperatureMax),
                            Fertility = UnityEngine.Random.Range(closestSpawn.Biome.FertilityMin, closestSpawn.Biome.FertilityMax),
                            Humidity = UnityEngine.Random.Range(closestSpawn.Biome.HumidityMin, closestSpawn.Biome.HumidityMax),
                            Sunshine = UnityEngine.Random.Range(closestSpawn.Biome.SunshineMin, closestSpawn.Biome.SunshineMax),
                        };

                        closestSpawn.Size++;

                        tiles[x, z] = tile;
                    }

                    tileList.Add(tile);
                }
            }

            return tileList;
        }

        private BiomeSpawn FindBiome(Tile tile)
        {
            var shortestWeightedDistance = Double.MaxValue;
            var closestSpawn = default(BiomeSpawn);

            foreach (var spawn in biomes)
            {
                var weightedDistance = CalculateWeightedDistance(tile, spawn);

                if (weightedDistance < shortestWeightedDistance)
                {
                    shortestWeightedDistance = weightedDistance;
                    closestSpawn = spawn;
                }
            }

            return closestSpawn;
        }

        private Single CalculateWeightedDistance(Tile tile, BiomeSpawn spawn)
        {
            var distanceVector = spawn.Position - tile.Position;

            var distance = distanceVector.Length / spawn.Weight;

            return distance;
        }

        private void GetAvailableSpawnPoint(Tile[,] fields, out Int32 x, out Int32 z)
        {
            x = 0;
            z = 0;

            var positionFound = false;

            for (var counter = 0; counter < 10; counter++)
            {
                x = UnityEngine.Random.Range(0, this.gameMode.World.Width);
                z = UnityEngine.Random.Range(0, this.gameMode.World.Height);

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
