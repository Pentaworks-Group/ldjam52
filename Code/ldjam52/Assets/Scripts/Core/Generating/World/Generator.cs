using System;
using System.Collections.Generic;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Buildings;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generating.World
{
    public class Generator
    {
        private const Int32 TilePrice = 300;

        private readonly GameMode gameMode;
        private List<BiomeSpawn> biomes;
        private Tile[,] tileCache;

        public Generator(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        private Model.World world;
        public Model.World World { get { return this.world; } }

        public Boolean Generate()
        {
            var isSuccessful = true;

            this.world = new Model.World()
            {
                Height = gameMode.World.Height,
                Width = gameMode.World.Width,
                Buildings = new List<Building>()
            };

            GenerateBiomes();
            GenerateTiles();
            GenerateBuildings();

            return isSuccessful;
        }

        private void GenerateBiomes()
        {
            biomes = new List<BiomeSpawn>();

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
        }

        private List<Tile> GenerateTiles()
        {
            this.tileCache = new Tile[gameMode.World.Width, gameMode.World.Height];

            foreach (var biomeSpawn in biomes)
            {
                GetAvailableSpawnPoint(this.tileCache, out Int32 x, out Int32 z);

                biomeSpawn.Position = new GameFrame.Core.Math.Vector3(x, 0, z);

                var tile = new Tile()
                {
                    ID = Guid.NewGuid(),
                    IsOwned = false,
                    Position = new GameFrame.Core.Math.Vector3(x, 0, z),
                    Color = biomeSpawn.Biome.Color,
                    Price = GetTilePrice(biomeSpawn.Biome),
                    Field = new Field()
                    {
                        Temperature = UnityEngine.Random.Range(biomeSpawn.Biome.TemperatureMin, biomeSpawn.Biome.TemperatureMax),
                        Fertility = UnityEngine.Random.Range(biomeSpawn.Biome.FertilityMin, biomeSpawn.Biome.FertilityMax),
                        Humidity = UnityEngine.Random.Range(biomeSpawn.Biome.HumidityMin, biomeSpawn.Biome.HumidityMax),
                        Sunshine = UnityEngine.Random.Range(biomeSpawn.Biome.SunshineMin, biomeSpawn.Biome.SunshineMax),
                    }
                };

                this.tileCache[x, z] = tile;
            }

            return FillEmptyTiles();
        }

        private void GenerateBuildings()
        {
            if (gameMode.World.Farm != default)
            {
                this.world.Farm = SpawnBuilding<Farm>(gameMode.World.Farm);
            }

            var shop = SpawnBuilding<Shop>(gameMode.World.Shop);

            if (shop != default)
            {
                this.world.Buildings.Add(shop);
            }

            var laboratory = SpawnBuilding<Laboratory>(gameMode.World.Laboratory);

            if (laboratory != default)
            {
                this.world.Buildings.Add(laboratory);
            }
        }

        private Int32 GetTilePrice(Biome biome)
        {
            if ((biome.TilePriceMin > 0) && (biome.TilePriceMax > 0))
            {
                return UnityEngine.Random.Range(biome.TilePriceMin, biome.TilePriceMax);
            }

            return TilePrice;
        }

        private TBuilding SpawnBuilding<TBuilding>(BuildingSettings buildingSettings) where TBuilding : Building, new()
        {
            var building = default(TBuilding);

            if (buildingSettings != default)
            {
                building = new TBuilding()
                {
                    Position = GetBuildingSpawnPoint(buildingSettings.Size)
                };
            }

            return building;
        }

        private List<Tile> FillEmptyTiles()
        {
            var tileList = new List<Tile>();

            for (int x = 0; x < gameMode.World.Width; x++)
            {
                for (int z = 0; z < gameMode.World.Height; z++)
                {
                    var tile = this.tileCache[x, z];

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

                        this.tileCache[x, z] = tile;
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

        private GameFrame.Core.Math.Vector3 GetBuildingSpawnPoint(GameFrame.Core.Math.Vector3 size)
        {
            var x = 0;
            var z = 0;

            var sizeX = (Int32)size.X;
            var sizeZ = (Int32)size.Z;

            var positionFound = false;

            for (var counter = 0; counter < 10; counter++)
            {
                x = UnityEngine.Random.Range(0, this.gameMode.World.Width - sizeX);
                z = UnityEngine.Random.Range(0, this.gameMode.World.Height - sizeZ);

                var usedTiles = GetAllTiles(x, z, sizeX, sizeZ);

                var areTilesAvailable = true;

                foreach (var usedTile in usedTiles)
                {
                    if (usedTile.Building != default)
                    {
                        areTilesAvailable = false;
                        break;
                    }
                }

                if (areTilesAvailable)
                {
                    positionFound = true;
                    break;
                }
            }

            if (!positionFound)
            {
                throw new Exception("Failed to generate Position!");
            }

            return new GameFrame.Core.Math.Vector3(x, 0, z);
        }

        private List<Tile> GetAllTiles(Int32 x, Int32 z, Int32 sizeX, Int32 sizeZ)
        {
            var tiles = new List<Tile>();

            for (int i = 0; i < sizeX; i++)
            {
                var xCoord = x + i;

                for (int j = 0; j < sizeZ; j++)
                {
                    var zCoord = z + j;

                    if ((xCoord < this.tileCache.GetLength(0)) && (zCoord < this.tileCache.GetLength(1)))
                    {
                        tiles.Add(this.tileCache[xCoord, zCoord]);
                    }
                    else
                    {

                    }
                }
            }

            return tiles;
        }
    }
}
