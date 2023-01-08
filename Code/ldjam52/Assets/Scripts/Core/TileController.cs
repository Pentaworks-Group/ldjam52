using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

namespace Assets.Scripts.Core
{
    public class TileController
    {
        private TileBehaviour[,] tileMap;

        public TileController(World world)
        {
            this.tileMap = new TileBehaviour[world.Width, world.Height];
        }

        public void AddTile(TileBehaviour tileBehaviour)
        {
            if (tileBehaviour.Tile != default)
            {
                this.tileMap[(Int32)tileBehaviour.Tile.Position.X, (Int32)tileBehaviour.Tile.Position.Z] = tileBehaviour;
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

                    var tileBehaviour = tileMap[x, z];

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
