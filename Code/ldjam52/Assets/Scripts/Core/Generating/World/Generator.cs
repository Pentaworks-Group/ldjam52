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
        private List<Biome> biomes;
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

            if (world != null)
            {
                if (world.Tiles?.Count < 1)
                {
                    isSuccessful = false;
                    UnityEngine.Debug.LogError("Too little or no tiles generated!");
                }

                if (world.Height < 1 || world.Width < 1)
                {
                    isSuccessful = false;
                    UnityEngine.Debug.LogError("World size is off!");
                }
            }

            return isSuccessful;
        }

        private void GenerateBiomes()
        {
            biomes = new List<Biome>();

            if (gameMode.World.IsAllBiomesUsageForced)
            {
                foreach (var biomeType in gameMode.AvailableBiomes)
                {
                    var biome = new Biome()
                    {
                        Type = biomeType,
                        Weight = UnityEngine.Random.Range(biomeType.WeightMin, biomeType.WeightMax),
                        Size = 1,
                    };

                    biomes.Add(biome);
                }
            }

            if (gameMode.World.RandomBiomesAmount > 0)
            {
                for (int i = 0; i < gameMode.World.RandomBiomesAmount; i++)
                {
                    var biomeType = gameMode.AvailableBiomes.GetRandomEntry();

                    var biome = new Biome()
                    {
                        Type = biomeType,
                        Weight = UnityEngine.Random.Range(biomeType.WeightMin, biomeType.WeightMax),
                        Size = 1,
                    };

                    biomes.Add(biome);
                }
            }
        }

        private void GenerateTiles()
        {
            this.tileCache = new Tile[gameMode.World.Width, gameMode.World.Height];

            foreach (var biome in biomes)
            {
                GetAvailableSpawnPoint(this.tileCache, out Int32 x, out Int32 z);

                biome.SpawnPosition = new GameFrame.Core.Math.Vector3(x, 0, z);

                var tile = new Tile()
                {
                    ID = Guid.NewGuid(),
                    Biome = biome,
                    IsOwned = false,
                    Position = new GameFrame.Core.Math.Vector3(x, 0, z),
                    Color = biome.Type.Color,
                    Price = GetTilePrice(biome.Type),
                    Field = new Field()
                    {
                        Temperature = UnityEngine.Random.Range(biome.Type.TemperatureMin, biome.Type.TemperatureMax),
                        Fertility = UnityEngine.Random.Range(biome.Type.FertilityMin, biome.Type.FertilityMax),
                        Humidity = UnityEngine.Random.Range(biome.Type.HumidityMin, biome.Type.HumidityMax),
                        Sunshine = UnityEngine.Random.Range(biome.Type.SunshineMin, biome.Type.SunshineMax),
                    }
                };

                this.tileCache[x, z] = tile;
            }

            this.world.Tiles = FillEmptyTiles();
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

        private Int32 GetTilePrice(BiomeType biome)
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
                    TemplateReference = buildingSettings.TemplateReferences.GetRandomEntry(),
                    Position = GetBuildingSpawnPoint(buildingSettings.Size, out var affectedTiles)
                };

                foreach (var affectedTile in affectedTiles)
                {
                    affectedTile.Building = building;
                }
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

                        tile.Color = closestSpawn.Type.Color;

                        tile.Field = new Field()
                        {
                            Temperature = UnityEngine.Random.Range(closestSpawn.Type.TemperatureMin, closestSpawn.Type.TemperatureMax),
                            Fertility = UnityEngine.Random.Range(closestSpawn.Type.FertilityMin, closestSpawn.Type.FertilityMax),
                            Humidity = UnityEngine.Random.Range(closestSpawn.Type.HumidityMin, closestSpawn.Type.HumidityMax),
                            Sunshine = UnityEngine.Random.Range(closestSpawn.Type.SunshineMin, closestSpawn.Type.SunshineMax),
                        };

                        closestSpawn.Size++;

                        this.tileCache[x, z] = tile;
                    }

                    tileList.Add(tile);
                }
            }

            return tileList;
        }

        private Biome FindBiome(Tile tile)
        {
            var shortestWeightedDistance = Double.MaxValue;
            var closestSpawn = default(Biome);

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

        private Single CalculateWeightedDistance(Tile tile, Biome spawn)
        {
            var distanceVector = spawn.SpawnPosition - tile.Position;

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

        private GameFrame.Core.Math.Vector3 GetBuildingSpawnPoint(GameFrame.Core.Math.Vector3 size, out List<Tile> affectedTiles)
        {
            affectedTiles = new List<Tile>();

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
                    affectedTiles.AddRange(usedTiles);

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
