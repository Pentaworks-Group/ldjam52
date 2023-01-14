using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine.Events;

namespace Assets.Scripts.Core
{
    public class TileController
    {
        public readonly UnityEvent<TileBehaviour> TileAdded = new UnityEvent<TileBehaviour>();

        private readonly Dictionary<Guid, TileBehaviour> tileCacheByID = new Dictionary<Guid, TileBehaviour>();
        private readonly TwoDimensionCache<Int32, TileBehaviour> tileCacheByCoordinates = new TwoDimensionCache<Int32, TileBehaviour>();

        public TileController()
        {
        }

        public void AddTile(TileBehaviour tileBehaviour)
        {
            if (tileBehaviour.Tile != default)
            {
                tileCacheByID[tileBehaviour.Tile.ID] = tileBehaviour;

                var x = (Int32)tileBehaviour.Tile.Position.X;
                var z = (Int32)tileBehaviour.Tile.Position.Z;

                this.tileCacheByCoordinates.PutSafe(x, z, tileBehaviour);

                TileAdded.Invoke(tileBehaviour);
            }
        }

        public List<TileBehaviour> GetAdjacentTiles(Tile tile)
        {
            var tiles = new List<TileBehaviour>();

            var x = (Int32)tile.Position.X;
            var z = (Int32)tile.Position.Z;

            tiles.AddIfNotNull(GetTileInDirection(x, z + 1, TileDirection.Top));
            tiles.AddIfNotNull(GetTileInDirection(x - 1, z, TileDirection.Left));
            tiles.AddIfNotNull(GetTileInDirection(x, z - 1, TileDirection.Bottom));
            tiles.AddIfNotNull(GetTileInDirection(x + 1, z, TileDirection.Right));

            return tiles;
        }

        public TileBehaviour GetTileInDirection(Tile tile, TileDirection direction)
        {
            var x = (Int32)tile.Position.X;
            var z = (Int32)tile.Position.Z;

            return GetTileInDirection(x, z, direction);
        }

        public TileBehaviour GetTileInDirection(Int32 x, Int32 z, TileDirection direction)
        {
            switch (direction)
            {
                case TileDirection.Top: return tileCacheByCoordinates.GetSafe(x, z + 1);
                case TileDirection.Left: return tileCacheByCoordinates.GetSafe(x - 1, z);
                case TileDirection.Bottom: return tileCacheByCoordinates.GetSafe(x, z - 1);
                case TileDirection.Right: return tileCacheByCoordinates.GetSafe(x + 1, z);
                default: return default;
            }
        }

        public List<TileBehaviour> GetSurroundingTiles(Tile tile)
        {
            var surroundingTiles = new List<TileBehaviour>();

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var x = (Int32)tile.Position.X + i;
                    var z = (Int32)tile.Position.Z + j;

                    var tileBehaviour = tileCacheByCoordinates.GetSafe(x, z);

                    if (tileBehaviour != null && tileBehaviour.Tile.ID != tile.ID)
                    {
                        surroundingTiles.Add(tileBehaviour);
                    }
                }
            }

            return surroundingTiles;
        }

        public TileBehaviour GetBehaviour(Guid tileID)
        {
            if (this.tileCacheByID.TryGetValue(tileID, out var tileBehaviour))
            {
                return tileBehaviour;
            }

            return default;
        }
    }
}
