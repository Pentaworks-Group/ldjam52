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

        private readonly TwoDimensionCache<Int32, TileBehaviour> tileCache = new TwoDimensionCache<Int32, TileBehaviour>();
        
        public TileController()
        {
        }

        public void AddTile(TileBehaviour tileBehaviour)
        {
            if (tileBehaviour.Tile != default)
            {
                var x = (Int32)tileBehaviour.Tile.Position.X;
                var z = (Int32)tileBehaviour.Tile.Position.Z;

                this.tileCache.PutSafe(x, z, tileBehaviour);

                TileAdded.Invoke(tileBehaviour);
            }
        }

        public List<TileBehaviour> GetAdjacentTiles(Tile tile)
        {
            var tiles = new List<TileBehaviour>();

            var x = (Int32)tile.Position.X;
            var z = (Int32)tile.Position.Z;

            tiles.AddIfNotNull(tileCache.GetSafe(x - 1, z));
            tiles.AddIfNotNull(tileCache.GetSafe(x - 1, z -1));
            tiles.AddIfNotNull(tileCache.GetSafe(x, z + 1));
            tiles.AddIfNotNull(tileCache.GetSafe(x + 1, z + 1));

            return tiles;
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

                    var tileBehaviour = tileCache.GetSafe(x, z);

                    if (tileBehaviour != null && tileBehaviour.Tile.ID != tile.ID)
                    {
                        surroundingTiles.Add(tileBehaviour);
                    }
                }
            }

            return surroundingTiles;
        }
    }
}
